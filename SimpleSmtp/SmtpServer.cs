//http://www.codeproject.com/Articles/456380/A-Csharp-SMTP-server-receiver
//http://www.codeproject.com/Tips/286952/create-a-simple-smtp-server-in-csharp

using BAMCIS.SimpleSmtp.MethodResponses;
using BAMCIS.SimpleSmtp.ResponseMessages;
using BAMCIS.VirusScanService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BAMCIS.SimpleSmtp
{
    internal class SmtpServer
    {
        private static readonly int SMTP_PORT = 25;
        private static readonly int SECURE_SMTP_PORT = 587;
        private static readonly string _HELO_CHARS = "[]0123456789.-abcdefghijklmnopqrstuvwxyz_";

        private uint _sessionCount;
        private static object _sessionCountLock = new object();

        private static TcpListenerEx Listener;
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _sreader;
        private StreamWriter _swriter;
        private IPAddress _remoteIP;
        private SmtpCommandEnum _lastCommand;
        private bool _isAuthenticated = false;

        private DateTime _startDate = DateTime.UtcNow;
        private string _helo = String.Empty;
        private string _from = String.Empty;
        private string _fromDomain = String.Empty;
        private List<string> _recipients = new List<string>();
        private string _message = String.Empty;
        private MailMessage _parsedMessage;

        private uint _errorCount = 0;
        private uint _messageCount = 0;
        private uint _noopCount = 0;
        private uint _verifyCount = 0;
        private string _sessionId = Configuration.GetSessionId();

        internal SmtpServer(TcpClient client)
        {
            Debug.WriteLine("Contstructor called");

            this._client = client;

            if (Configuration.Instance.ReceiveTimeout > 0)
            {
                this._client.ReceiveTimeout = Configuration.Instance.ReceiveTimeout;
            }

            this._client.ReceiveBufferSize = 8192; //Default is 8192 bytes
            this._client.SendBufferSize = 8192; //Default is 8192 bytes

            IPAddress temp;
            if (IPAddress.TryParse(this._client.Client.RemoteEndPoint.ToString().Split(':')[0], out temp))
            {
                this._remoteIP = temp;
            }

            this._stream = this._client.GetStream();
            this._sreader = new StreamReader(this._stream);
            this._swriter = new StreamWriter(this._stream);

            this._swriter.NewLine = "\r\n";
            this._swriter.AutoFlush = true;

            this.AddSession();
            Debug.WriteLine(String.Format("{0} : Client {1} connected, session = {2}, sessionId = {3}", DateTime.UtcNow.ToString(), this._remoteIP, this._sessionCount, this._sessionId));
        }

        internal static async Task<TcpClient> InitializeListener()
        {
            if (SmtpServer.Listener == null)
            {
                int Port = (Configuration.Instance.UseSMTPS) ? SmtpServer.SECURE_SMTP_PORT : SmtpServer.SMTP_PORT;
                SmtpServer.Listener = new TcpListenerEx(new IPEndPoint(IPAddress.Any, Port));
            }

            if (!SmtpServer.Listener.IsActive)
            {
                SmtpServer.Listener.Start();
                Debug.WriteLine(String.Format("Listening for connections on {0}", SmtpServer.Listener.LocalEndpoint.ToString()));
            }

            return await Listener.AcceptTcpClientAsync();
        }

        internal async void HandleSession()
        {
            Debug.WriteLine("Handling new SMTP session");

            SendLineResult SLResult;

            //Test session capability
            if (Configuration.Instance.MaxSessions > 0 && this._sessionCount > Configuration.Instance.MaxSessions)
            {
                SLResult = await this.SendLine(SmtpResponseMessage._421_CONNECTION_LIMIT_EXCEEDED.ToFormattedString());
                if (SLResult.Status == SendLineStatus.SUCCESSFUL)
                {
                    this.CloseSession();
                }
            }
            else
            {
                //Perform pre checks on the sender
                ConductPreCheckResult PreChecks = await ConductPreMessageChecks(this._remoteIP);

                if (!PreChecks.IsError)
                {
                    SLResult = await this.SendLine(SmtpResponseMessage._220_BANNER.ToFormattedString(DateTime.Now.ToString()));
                    ReceiveLineResult RLResult;

                    //Make sure the read line didn't have an error, the message we got wasn't null (end of stream), and that our 
                    //send line can still connect to the client
                    while (!(RLResult = await this.ReceiveLine()).IsError &&
                            RLResult.Message != null &&
                            !SLResult.IsError
                        )
                    {
                        //The read line result will return an empty string i
                        if (!String.IsNullOrEmpty(RLResult.Message))
                        {
                            SmtpCommandEnum Id = GetCommandId(RLResult.Message);
                            string Response = GenerateResponse(RLResult.Message, Id);
                            ConductPeriodicCheckResult PeriodicChecks = await ConductPeriodicChecks();

                            if (PeriodicChecks.IsError)
                            {
                                SLResult = await this.SendLine(PeriodicChecks.Message);
                                //Should check is this is a terminating error or not
                                break;
                            }
                            else
                            {
                                this._lastCommand = Id;
                                SLResult = await this.SendLine(Response);

                                //If the client sent a quit, we must obey, stop reading commands and exit
                                if (this._lastCommand == SmtpCommandEnum.QUIT)
                                {
                                    break;
                                }
                            }

                            //If the last command was DATA, go ahead and receive the data
                            if (this._lastCommand == SmtpCommandEnum.DATA)
                            {
                                //Reset the last command so this doesn't enter a loop on some kind of exception later
                                //this._lastCommand = SmtpCommandEnum.INVALID;

                                ReceiveDataResult RDResult = await this.ReceiveData();

                                if (RDResult.Status == ReceiveDataStatus.SUCCESSFUL)
                                {
                                    this._message = RDResult.Message;

                                    StringBuilder Buffer = new StringBuilder();
                                    Buffer.AppendLine("X-Sender: " + this._from);
                                    Buffer.AppendLine(String.Join("\r\n", this._recipients.Select(x => { return "X-Receiver: " + x; }).ToArray()));
                                    Buffer.AppendLine(this._message);

                                    try
                                    {
                                        MailMessage ResultMessage = SmtpMessageStringParser.Parse(Buffer.ToString());
                                        this._parsedMessage = ResultMessage;

                                        ConductPostCheckResult PostChecks = await ConductPostMessageChecks(this._parsedMessage);
                                        if (PostChecks.IsError)
                                        {
                                            ++this._errorCount;
                                            SLResult = await this.SendLine(PostChecks.Message);

                                            if (PostChecks.Status == ConductPostCheckStatus.VIRUS)
                                            {
                                                QuarantineMessage(this._parsedMessage);
                                            }
                                        }
                                        else
                                        {
                                            SLResult = await this.SendLine(PostChecks.Message);
                                            SaveMessage(this._parsedMessage);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        ++this._errorCount;
                                        SLResult = await this.SendLine(SmtpResponseMessage._421_MAX_ERRORS_REACHED.ToFormattedString());
                                    }
                                }
                                else
                                {
                                    ++this._errorCount;
                                    SLResult = await this.SendLine(RDResult.Message);

                                    if (RDResult.Status == ReceiveDataStatus.CONNECTION_LOST ||
                                        RDResult.Status == ReceiveDataStatus.CLIENT_DISCONNECTED)
                                    {
                                        this.CloseSession();
                                    }
                                }
                            }
                        }
                        else //No data received from network stream, must have timed out trying to receive
                        {
                            ++this._errorCount;
                            SLResult = await this.SendLine(SmtpResponseMessage._442_CONNECTION_DROPPED.ToFormattedString());
                            break;
                        }
                    }

                    if (RLResult.IsError)
                    {
                        SLResult = await this.SendLine(RLResult.Message);
                    }
                }
                else
                {
                    SLResult = await this.SendLine(PreChecks.Message);
                }

                this.CloseSession();
            }
        }

        private async Task<ConductPreCheckResult> ConductPreMessageChecks(IPAddress ip)
        {
            if (IsSenderIpAddressAlllowed(ip))
            {
                if (!IsPrivateIP(ip))
                {
                    string HostName = await GetDnsPtrRecord(ip);

                    if (!String.IsNullOrEmpty(HostName) && HostName.IndexOf(".") != -1)
                    {
                        string DomainName = GetDomainFromHostName(HostName);

                        if (!String.IsNullOrEmpty(DomainName))
                        {
                            if (!IsSenderDomainAllowed(DomainName))
                            {
                                return new ConductPreCheckDnsBlacklistResult();
                            }
                        }

                        if (!await DoesDnsMxRecordExist(DomainName))
                        {
                            return new ConductPreCheckNoMxRecordResult();
                        }
                    }
                    else
                    {
                        if (Configuration.Instance.CheckReverseDnsLookup)
                        {
                            return new ConductPreCheckReverseDnsFailedResult(ip.ToString());
                        }
                    }
                }
                else if (!Configuration.Instance.AllowPrivateIPs)
                {
                    return new ConductPrecheckIPBlacklistResult();
                }
            }
            else
            {
                return new ConductPrecheckIPBlacklistResult();
            }

            if (IsEarlyTalker())
            {
                return new ConductPreCheckEarlyTalkerResult();
            }

            return new ConductPreCheckSuccessResult();
        }

        private async Task<ConductPostCheckResult> ConductPostMessageChecks(MailMessage message)
        {
            if (Configuration.Instance.HeadersMustMatch)
            {
                if (this._parsedMessage.Sender.Address != this._parsedMessage.From.Address)
                {
                    return new ConductPostCheckHeaderMismatchResult();
                }
            }

            if (!VirusScanAttachments(this._parsedMessage))
            {
                return new ConductPostCheckVirusResult();
            }

            return new ConductPostCheckSuccessResult();
        }

        private async Task<ConductPeriodicCheckResult> ConductPeriodicChecks()
        {
            if (Configuration.Instance.MaxRecipients > 0 && this._recipients.Count > Configuration.Instance.MaxRecipients)
            {
                return new ConductPostCheckExceededRecipientLimitResult();
            }

            if (Configuration.Instance.MaxMessages > 0 && this._messageCount > Configuration.Instance.MaxMessages)
            {
                return new ConductPostCheckExceededMessageLimitResult();
            }

            if (Configuration.Instance.MaxErrors > 0 && this._errorCount > Configuration.Instance.MaxErrors)
            {
                return new ConductPostCheckMaxErrorsResult();
            }

            if (Configuration.Instance.MaxVerifyRequests > 0 && this._verifyCount > Configuration.Instance.MaxVerifyRequests)
            {
                return new ConductPostCheckExceededVerifyLimitResult();
            }

            return new ConductPeriodicCheckSuccessResult();
        }

        private static bool VirusScanAttachments(MailMessage message)
        {
            bool Clean = true;

            if (Configuration.Instance.VirusScanAttachments && message.Attachments.Count > 0)
            {
                foreach (Attachment Attach in message.Attachments)
                {
                    IVirusScan Scanner = VirusScannerFactory.GetVirusScanner(VirusScannerEnum.CLAM_AV);
                    ScanResult Result = Scanner.Scan(Attach.ContentStream);

                    if (!Result.IsVirusFree)
                    {
                        Clean = false;
                    }
                }
            }

            return Clean;
        }

        /// <summary>
        /// Closes a client session by closing the tcp client connection,
        /// removing the session from the counter, and reseting the session
        /// attributes
        /// </summary>
        private void CloseSession()
        {
            if (this._client != null)
            {
                if (this._client.Connected)
                {
                    try
                    {
                        this._client.Close();
                    }
                    catch
                    { }
                    finally
                    {
                        this._client = null;
                    }
                }
            }

            this.RemoveSession();
            this.ResetSession();
        }

        /// <summary>
        /// Resets all of the private data and disposes the stream
        /// readers and writers
        /// </summary>
        private void ResetSession()
        {
            this._helo = String.Empty;
            this._from = String.Empty;
            this._fromDomain = String.Empty;
            this._recipients = new List<string>();
            this._message = String.Empty;
            this._parsedMessage = new MailMessage();
            this._noopCount = 0;
            this._errorCount = 0;
            this._verifyCount = 0;
            this._messageCount = 0;
            this._isAuthenticated = false;
            this._lastCommand = SmtpCommandEnum.INVALID;

            try
            {
                this._swriter.Dispose();
            }
            catch { }

            try
            {
                this._sreader.Dispose();
            }
            catch { }

            try
            {
                this._stream.Dispose();
            }
            catch { }

        }

        /// <summary>
        /// Tests the client stream to check if there is data available for sending
        /// </summary>
        /// <returns>
        /// Returns true if there is data available or false if there is not
        /// </returns>
        private bool ReceivePeek()
        {
            try
            {
                if (this._client != null && this._client.Connected)
                {
                    return this._client.GetStream().DataAvailable;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The SMTP standards say that at the beginning of a connection, the server first sends a greeting message, 
        /// after which the client sends the HELO or EHLO command. Sloppily written spamware often sends the HELO 
        /// immediately without waiting for the greeting. If the server slightly delays the greeting, it can check 
        /// to see if there's a premature HELO and drop the connection. 
        /// </summary>
        /// <returns>
        /// True if the client is an early talker, false otherwise
        /// </returns>
        private bool IsEarlyTalker()
        {
            Thread.Sleep(10);
            return ReceivePeek();
        }

        /// <summary>
        /// Gets a command line from a connected SMTP client
        /// </summary>
        /// <returns>
        /// Returns a Tuple indicating if the receive was successful and the received message, or if
        /// the receive failed, the associated error message.
        /// </returns>
        private async Task<ReceiveLineResult> ReceiveLine()
        {
            string Line = String.Empty;

            try
            {
                if (this._client != null)
                {
                    //Will return null at the end of the stream
                    if (this._client.Connected)
                    {
                        Line = await this._sreader.ReadLineAsync();
                        Debug.WriteLine(Line);

                        if (!IsASCIIOnly(Line))
                        {
                            return new ReceiveLineInvalidCharacterResult();
                        }
                        else
                        {
                            return new ReceiveLineSuccessResult(Line);
                        }
                    }
                    else
                    {
                        return new ReceiveLineClientDisconnectedResult();
                    }
                }
                else
                {
                    return new ReceiveLineConnectionLostResult();
                }
            }
            catch (Exception)
            {
                return new ReceiveLineErrorResult();
            }
        }

        /// <summary>
        /// Receives the data portion of the SMTP message.
        /// </summary>
        /// <returns>
        /// A ReceiveDataResult with the result status and message
        /// </returns>
        private async Task<ReceiveDataResult> ReceiveData()
        {
            ReceiveLineResult Result;
            StringBuilder Buffer = new StringBuilder();

            try
            {
                int BytesPerChar = this._sreader.CurrentEncoding.GetByteCount("a");

                //Read until the end of the stream, an error occurs, or the data portion delimiter
                while (
                        !(Result = await this.ReceiveLine()).IsError &&
                        Result.Message != null &&
                        Result.Message != "."
                    )
                {
                    //Check to make sure the message hasn't exceeded the size limit
                    if (((double)((Buffer.Length + Result.Message.Length) * BytesPerChar) / 1024) < Configuration.Instance.MaxMessageSizeInKiB)
                    {
                        Buffer.AppendLine(Result.Message);
                    }
                    else
                    {
                        return new ReceiveDataMessageTooLargeResult();
                    }
                }

                switch (Result.Status)
                {
                    case (ReceiveLineStatus.SUCCESSFUL):
                        {
                            //Check to make sure we actually got the delimiter, if the message is null
                            //we got to the end of the stream without the delimiter
                            //since the while loop would break out as soon as we get that
                            if (Result.Message == ".")
                            {
                                return new ReceiveDataSuccessResult(Buffer.ToString());
                            }
                            else
                            {
                                return new ReceiveDataMissingArgResult();
                            }
                        }
                    case (ReceiveLineStatus.CLIENT_DISCONNECTED):
                        {
                            return new ReceiveDataClientDisconnectedResult();
                        }
                    case (ReceiveLineStatus.CONNECTION_LOST):
                        {
                            return new ReceiveDataConnectionLostResult();
                        }
                    default:
                    case (ReceiveLineStatus.ERROR):
                        {
                            return new ReceiveDataErrorResult();
                        }
                    case (ReceiveLineStatus.INVALID_CHARACTER):
                        {
                            return new ReceiveDataInvalidCharacterResult();
                        }
                }
            }
            catch (Exception)
            {
                return new ReceiveDataErrorResult();
            }
        }

        /// <summary>
        /// Tests if the input only contains ASCII characters
        /// </summary>
        /// <param name="line">The string to test</param>
        /// <returns>True if the string is ASCII only, false otherwise</returns>
        private static bool IsASCIIOnly(string line)
        {
            //ASCII Values are between 0 and 127, so any character higher than
            //that is non ASCII
            return !line.Any(x => (int)x > 127);
        }

        /// <summary>
        /// Writes a line to the network stream
        /// </summary>
        /// <param name="line">The line to send to the client</param>
        /// <returns>
        /// A SendLineResult object with the result status and message
        /// </returns>
        private async Task<SendLineResult> SendLine(string line)
        {
            try
            {
                if (this._client.Connected)
                {
                    Debug.WriteLine(line);
                    await this._swriter.WriteLineAsync(line);
                    return new SendLineSuccessResult();
                }
                else
                {
                    return new SendLineClientDisconnectedResult();
                }
            }
            catch
            {
                return new SendLineErrorResult();
            }
        }

        /// <summary>
        /// Adds a new session to the internal session counter
        /// 
        /// This method is thread safe
        /// </summary>
        private void AddSession()
        {
            lock (_sessionCountLock)
            {
                ++this._sessionCount;
            }
        }

        /// <summary>
        /// Removes a session from the internal session counter.
        /// 
        /// This method is thread safe
        /// </summary>
        private void RemoveSession()
        {
            lock (_sessionCountLock)
            {
                if (this._sessionCount > 0)
                {
                    --this._sessionCount;
                }
            }
        }

        #region Writing Email Messages To Disk

        /// <summary>
        /// Writes the message to a pickup folder for sending
        /// </summary>
        /// <param name="message">The message to be sent</param>
        private static void SaveMessage(MailMessage message)
        {
            WriteMessage(message, Configuration.Instance.OutgoingFolder);
        }

        /// <summary>
        /// Writes the message to a quarantine folder so it can be reviewed
        /// </summary>
        /// <param name="message">The message to quarantine</param>
        private static void QuarantineMessage(MailMessage message)
        {
            WriteMessage(message, Configuration.Instance.QuarantineFolder);
        }

        /// <summary>
        /// Writes a message to a specified location as a .eml file
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="location">The destination folder, which will be created if it doesn't exist</param>
        private static void WriteMessage(MailMessage message, string location)
        {
            if (String.IsNullOrEmpty(location))
            {
                throw new ArgumentNullException("location", "The location cannot be null or empty");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message", "The mail message cannot be null.");
            }

            try
            {
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }

                PropertiesContractResolver Resolver = new PropertiesContractResolver();
                Resolver.ExcludeTypes.Add(typeof(Stream));
                string Json = JsonConvert.SerializeObject(message, Resolver.GetSerializerSettings());

                File.WriteAllText(Path.Combine(location, Guid.NewGuid() + ".json"), Json);

                using (SmtpClient client = new SmtpClient())
                {
                    client.UseDefaultCredentials = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = location;
                    client.Send(message);

                    message.Dispose();
                }
            }
            catch (Exception e)
            {

            }
        }

        #endregion

        /// <summary>
        /// Gets the SMTP command from the text received from the client.
        /// 
        /// The command text is compared against the SmtpCommandEnum values.
        /// </summary>
        /// <param name="commandLine">The command text received from the client</param>
        /// <returns>An SmtpCommandEnum oject that represents the SMTP command sent, or INVALID if the command was not recognized</returns>
        private static SmtpCommandEnum GetCommandId(string commandLine)
        {
            string TempBuffer = commandLine.ToLower();
            SmtpCommandEnum Result = SmtpCommandEnum.INVALID;

            foreach (SmtpCommandEnum Cmd in Enum.GetValues(typeof(SmtpCommandEnum)))
            {
                if (TempBuffer.StartsWith(Cmd.GetAttribute<SmtpCommandAttribute>().Command.ToLower()))
                {
                    Result = Cmd;
                    break;
                }
            }

            return Result;
        }

        private string GenerateResponse(string commandLine, SmtpCommandEnum command)
        {
            string Response = String.Empty;

            switch (command)
            {
                case SmtpCommandEnum.HELO:
                case SmtpCommandEnum.EHLO:
                    {
                        Response = Cmd_Helo(commandLine, command);
                        break;
                    }
                case SmtpCommandEnum.MAIL_FROM:
                    {
                        Response = Cmd_MailFrom(commandLine);
                        break;
                    }
                case SmtpCommandEnum.RCPT_TO:
                    {
                        Response = Cmd_RecipientTo(commandLine);
                        break;
                    }
                case SmtpCommandEnum.DATA:
                    {
                        Response = Cmd_Data(commandLine);
                        break;
                    }
                case SmtpCommandEnum.RSET:
                    {
                        this.ResetSession();
                        Response = SmtpResponseMessage._250_RESET_OK.ToFormattedString();
                        break;
                    }
                case SmtpCommandEnum.QUIT:
                    {
                        Response = SmtpResponseMessage._221_CLOSING_CONNECTION.ToFormattedString();
                        break;
                    }
                case SmtpCommandEnum.VRFY:
                    {
                        Response = Cmd_Verify(commandLine, command);
                        break;
                    }
                case SmtpCommandEnum.HELP:
                    {
                        Response = Cmd_Help(commandLine);
                        break;
                    }
                case SmtpCommandEnum.NOOP:
                    {
                        ++this._noopCount;
                        Response = SmtpErrorMessages._250_NOOP_OK_MESSAGE;
                        break;
                    }
                case SmtpCommandEnum.INVALID:
                default: //Unknown command
                    {
                        Response = Cmd_Unknown(commandLine);
                        break;
                    }
            }

            return Response;
        }

        /// <summary>
        /// HELO / EHLO Response creation
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="command"></param>
        /// <returns>The HELO or EHLO response</returns>
        private string Cmd_Helo(string commandLine, SmtpCommandEnum command)
        {
            List<string> Parts = ParseCommandLine(commandLine, command).ToList();
            string Text = Parts.ElementAt(1).ToLower();
            //Require that the HELO has a command and argument
            if (Parts.Count != 2)
            {
                ++this._errorCount;
                return SmtpErrorMessages._501_MISSING_ARG_MESSAGE;
            }
            //Ensure multiple HELOs aren't being sent
            else if (!String.IsNullOrEmpty(this._helo))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            //Ensure proper formatting of the message
            else if (!ValidateHELO(Text))
            {
                ++this._errorCount;
                return String.Format(SmtpErrorMessages._501_BAD_ARG_MESSAGE, Parts.ElementAt(0));
            }
            //Make sure the HELO isn't from the local box, if configuration specifies to block
            //This might be enabled for testing
            else if (!Configuration.Instance.AllowPrivateIPs && (Text.Equals("localhost") ||
                Text.Equals(Environment.MachineName.ToLower()) ||
                Text.StartsWith("[127.") ||
                Text.Equals("[" + SmtpServer.Listener.LocalEndpoint.ToString().Split(':')[0] + "]"))
                )
            {
                ++this._errorCount;
                return SmtpErrorMessages._554_SPOOFED_MESSAGE;
            }
            else
            {
                //Set that a valid HELO has been sent
                this._helo = Text;
                if (command == SmtpCommandEnum.HELO)
                {
                    return SmtpResponseMessage._250_HELO_RESPONSE.ToFormattedString(this._remoteIP.ToString());
                }
                else
                {
                    return SmtpResponseMessage._250_EHLO_RESPONSE.ToFormattedString(this._remoteIP.ToString());
                }
            }
        }

        /// <summary>
        /// MAIL FROM: Response creation
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>The MAIL FROM: response</returns>
        private string Cmd_MailFrom(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else if (String.IsNullOrEmpty(this._helo))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else
            {
                List<string> Parts = ParseCommandLine(commandLine, SmtpCommandEnum.MAIL_FROM).ToList();
                if (Parts.Count < 2 || Parts.Count > 2)
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._501_MISSING_ARG_MESSAGE;
                }
                else if (!IsValidEmailAddressFormat(Parts.ElementAt(1)))
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._501_BAD_ARG_MESSAGE;
                }
                else
                {
                    //Found a valid sender address
                    string Temp = Parts.ElementAt(1).Replace("<", "").Replace(">", "");
                    this._from = Temp;
                    this._fromDomain = Temp.Split('@').ElementAt(1);
                    return String.Format(SmtpErrorMessages._250_SENDER_OK_MESSAGE, Temp);
                }
            }
        }

        /// <summary>
        /// RCPT TO: Response creation
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>The RCPT TO: response</returns>
        private string Cmd_RecipientTo(string commandLine)
        {
            string Response = String.Empty;
            if (String.IsNullOrEmpty(commandLine))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else if (String.IsNullOrEmpty(this._from))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else
            {
                string Domain = String.Empty;
                string Mailbox = String.Empty;
                List<string> Parts = ParseCommandLine(commandLine, SmtpCommandEnum.RCPT_TO).ToList();

                if (Parts.Count != 2)
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._501_MISSING_ARG_MESSAGE;
                }
                else if (!IsValidEmailAddressFormat(Parts.ElementAt(1)))
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._501_BAD_ARG_MESSAGE;
                }
                else if (!IsDestinationDomainAllowed(Parts.ElementAt(1).Split('@').ElementAt(1)))
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._442_DNS_OR_IP_DENIED_MESSAGE;
                }
                //Check if the destination is the local domain and if the local mailbox exists
                else if (Parts.ElementAt(1).Split('@').ElementAt(1).ToLower() == Configuration.Instance.Domain.ToLower() &&
                           !DoesLocalMailboxExist(Parts.ElementAt(1).Split('@').ElementAt(0).ToLower(), Parts.ElementAt(1).Split('@').ElementAt(1).ToLower()))
                {
                    ++this._errorCount;
                    return SmtpErrorMessages._550_INVALID_RECIPIENT_MESSAGE;
                }
                else
                {
                    this._recipients.Add(Parts.ElementAt(1).Replace("<", "").Replace(">", ""));
                    return String.Format(SmtpErrorMessages._250_RECIPIENT_OK_MESSAGE, Parts.ElementAt(1).Replace("<", "").Replace(">", ""));
                }
            }
        }

        /// <summary>
        /// DATA response creation
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>The DATA response</returns>
        private string Cmd_Data(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine))
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else if (this._recipients.Count < 1)
            {
                ++this._errorCount;
                return SmtpErrorMessages._503_CMD_OUT_OF_ORDER_MESSAGE;
            }
            else
            {
                return SmtpErrorMessages._354_READY_FOR_MAIL_BODY_MESSAGE;
            }
        }

        /// <summary>
        /// VRFY response creation
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        private string Cmd_Verify(string commandLine, SmtpCommandEnum command)
        {
            ++this._verifyCount;

            List<string> Parts = ParseCommandLine(commandLine, command).ToList();

            if (Parts.Count != 2)
            {
                ++this._errorCount;
                return SmtpErrorMessages._501_MISSING_ARG_MESSAGE;
            }
            else if (!IsValidEmailAddressFormat(Parts.ElementAt(1)))
            {
                this._errorCount++;
                return String.Format(SmtpErrorMessages._550_VERIFY_FAILED_MESSAGE, Parts.ElementAt(1));
            }
            else if (!DoesLocalMailboxExist(Parts.ElementAt(1).Split('@').ElementAt(0), (Parts.ElementAt(1).Split('@').ElementAt(1))))
            {
                return String.Format(SmtpErrorMessages._550_VERIFY_FAILED_MESSAGE, Parts.ElementAt(1));
            }
            else
            {
                return SmtpErrorMessages._250_VERIFY_OK_MESSAGE;
            }
        }

        /// <summary>
        /// HELP response creation, manages general help and command specific details
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        private string Cmd_Help(string commandLine)
        {
            StringBuilder Buffer = new StringBuilder();

            if (commandLine.IndexOf(" ") == -1)
            {

                Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-2.0.0");
                Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-2.0.0 Availble help topics:");

                foreach (SmtpCommandEnum Cmd in Enum.GetValues(typeof(SmtpCommandEnum)))
                {
                    Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-" + Cmd.GetAttribute<SmtpCommandAttribute>().Command);
                }

                Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-2.0.0 For more info use \"HELP <topic>\"");
                Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + " 2.0.0 End of HELP info");
            }
            else
            {
                string Command = commandLine.Split(' ').ElementAt(1).ToUpper();

                if (Enum.GetValues(typeof(SmtpCommandEnum)).Cast<SmtpCommandEnum>().Any(x => x.GetAttribute<SmtpCommandAttribute>().Command.ToUpper().Equals(Command)))
                {
                    Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-2.0.0 " + Command);

                    string Text = Enum.GetValues(typeof(SmtpCommandEnum)).Cast<SmtpCommandEnum>()
                        .First(x => x.GetAttribute<SmtpCommandAttribute>().Command.ToUpper().Equals(Command))
                        .GetAttribute<SmtpCommandAttribute>().Comment;

                    Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + "-2.0.0 " + Text);
                    Buffer.AppendLine((int)SmtpErrorCodeEnum._214_HELP_MESSAGE + " 2.0.0 End of HELP info");
                }
                else
                {
                    Buffer.AppendLine(SmtpErrorMessages._504_ARG_NOT_IMPLEMENTED_MESSAGE);
                }
            }

            return Buffer.ToString();
        }

        /// <summary>
        /// Response for unknown commands
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        private string Cmd_Unknown(string commandLine)
        {
            ++this._errorCount;
            return SmtpErrorMessages._502_COMMAND_NOT_IMPLEMENTED_MESSAGE;
        }

        private static bool ValidateHELO(string helo)
        {
            IPAddress Temp;

            if (
                    String.IsNullOrEmpty(helo) ||
                    helo.StartsWith(".") ||
                    helo.StartsWith("-") ||
                    helo.Any(x => !_HELO_CHARS.Contains(x)) ||
                    (helo.StartsWith("[") && !helo.EndsWith("]")) ||
                    (helo.StartsWith("[") && helo.EndsWith("]") && !IPAddress.TryParse(helo.Replace("[", "").Replace("]", ""), out Temp))
                )
            {
                return false;
            }
            else if (!Configuration.Instance.AllowNonDomainSenders &&
                        (
                            helo.IndexOf(".") == -1 ||
                            !IsValidEmailAddressFormat("postmaster@" + helo)
                        )
                    )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool IsValidEmailAddressFormat(string email)
        {
            bool Valid = true;

            try
            {
                MailAddress Test = new MailAddress(email);
            }
            catch (FormatException)
            {
                Valid = false;
            }

            return Valid;
        }

        private static bool IsDestinationDomainAllowed(string domain)
        {
            bool Allowed = true;
            if (!String.IsNullOrEmpty(domain))
            {
                if (domain.ToLower().Equals(Configuration.Instance.Domain))
                {
                    return true;
                }
                else
                {
                    if (Configuration.Instance.AllowedRelayDomains.Any())
                    {
                        Allowed = Configuration.Instance.AllowedRelayDomains.Where(x => Regex.Match(domain, x, RegexOptions.IgnoreCase).Success).Any();

                        if (!Allowed)
                        {
                            return false;
                        }
                    }

                    if (Configuration.Instance.BlockedRelayDomains.Any())
                    {
                        Allowed = !Configuration.Instance.BlockedRelayDomains.Where(x => Regex.Match(domain, x, RegexOptions.IgnoreCase).Success).Any();

                        if (!Allowed)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                throw new ArgumentNullException("domain", "The destination domain cannot be null or empty.");
            }
        }

        private static bool DoesLocalMailboxExist(string mailbox, string domain)
        {

            return true;
        }

        private static IEnumerable<string> ParseCommandLine(string commandLine, SmtpCommandEnum command)
        {
            List<string> Parts = new List<string>();

            if (!String.IsNullOrEmpty(commandLine))
            {
                //Get the command string so we can test if it has a colon or space delimiter
                int Position = -1;

                if (command.GetAttribute<SmtpCommandAttribute>().Command.IndexOf(':') != -1)
                {
                    Position = commandLine.IndexOf(":");
                }
                else
                {
                    Position = commandLine.IndexOf(" ");
                }

                if (Position != -1)
                {
                    Parts.Add(FormatCommandLine(commandLine.Substring(0, Position)));
                    Parts.Add(FormatCommandLine(commandLine.Substring(Position + 1)));
                }
                else
                {
                    Parts.Add(FormatCommandLine(commandLine));
                }
            }

            return Parts;
        }

        /// <summary>
        /// Removes control characters and makes sure everything is single spaced
        /// </summary>
        /// <param name="commandLine">The command line to format</param>
        /// <returns></returns>
        private static string FormatCommandLine(string commandLine)
        {
            if (!String.IsNullOrEmpty(commandLine))
            {
                //Replace control characters, like tabs or end of lines with a space, then trim 
                commandLine = new string(commandLine.Trim().Select(x => { return (char.IsControl(x)) ? ' ' : x; }).ToArray()).Trim();

                //Replace any double spaces with a single space
                while (commandLine.Contains("  "))
                {
                    commandLine = commandLine.Replace("  ", " ");
                }
            }

            return commandLine;
        }

        #region IP Address Checks

        private static bool IsSenderIpAddressAlllowed(IPAddress ip)
        {
            if (ip != null)
            {
                //If a white list is defined, check it
                if (Configuration.Instance.IPWhiteList.Count > 0)
                {
                    if (IsIPWhiteListed(ip))
                    {
                        return true;
                    }
                }

                //If a black list is defined, check it
                if (Configuration.Instance.IPBlackList.Count > 0)
                {
                    if (IsIPBlackListed(ip))
                    {
                        return false;
                    }
                }

                //If neither are defined, the IP is ok
                return true;
            }
            else
            {
                return true;
            }
        }

        private static bool IsIPBlackListed(IPAddress ipAddress)
        {
            return (Configuration.Instance.IPBlackList.Select(x =>
            {
                IPAddress Temp;
                if (IPAddress.TryParse(x, out Temp))
                {
                    return Temp;
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null).Contains(ipAddress));
        }

        private static bool IsIPWhiteListed(IPAddress ipAddress)
        {
            return (Configuration.Instance.IPWhiteList.Select(x =>
            {
                IPAddress Temp;
                if (IPAddress.TryParse(x, out Temp))
                {
                    return Temp;
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null).Contains(ipAddress));
        }

        private static bool IsPrivateIP(IPAddress ipAddress)
        {
            bool isPrivate = false;
            List<IPAddressRange> ranges = new List<IPAddressRange>() {
                new IPAddressRange(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.255.255")),
                new IPAddressRange(IPAddress.Parse("172.16.0.0"), IPAddress.Parse("172.31.255.255")),
                new IPAddressRange(IPAddress.Parse("10.0.0.0"), IPAddress.Parse("10.255.255.255")),
                new IPAddressRange(IPAddress.Parse("169.254.0.0"), IPAddress.Parse("169.254.255.255"))
            };

            if (IPAddress.IsLoopback(ipAddress))
            {
                isPrivate = true;
            }
            else
            {
                foreach (IPAddressRange range in ranges)
                {
                    if (range.IsInRange(ipAddress))
                    {
                        isPrivate = true;
                        break;
                    }
                }
            }

            return isPrivate;
        }

        #endregion

        private bool IsSenderBlocked(string Sender)
        {
            return Configuration.Instance.UnallowedSenders.Select(x => x.ToLower()).Contains(Sender.ToLower());
        }

        #region DNS Checks

        private static async Task<string> GetDnsPtrRecord(IPAddress ip)
        {
            if (ip != null)
            {
                try
                {
                    IPHostEntry Entry = await Dns.GetHostEntryAsync(ip);

                    if (Entry != null)
                    {
                        return Entry.HostName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        private static async Task<bool> DoesDnsMxRecordExist(string domain)
        {
            return true;
        }

        private static bool IsSenderDomainAllowed(string domain)
        {
            if (!String.IsNullOrEmpty(domain))
            {
                //If a white list is defined, check it
                if (Configuration.Instance.DnsWhiteList.Count > 0)
                {
                    if (IsDnsWhiteListed(domain))
                    {
                        return true;
                    }
                }

                //If a black list is defined, check it
                if (Configuration.Instance.DnsBlackList.Count > 0)
                {
                    if (IsDnsBlackListed(domain))
                    {
                        return false;
                    }
                }

                //If neither are defined, the host name is ok
                return true;
            }
            else
            {
                throw new ArgumentNullException("domain", "The domain name cannot be null or empty.");
            }
        }

        private static bool IsDnsBlackListed(string domain)
        {
            if (!String.IsNullOrEmpty(domain))
            {
                return Configuration.Instance.DnsBlackList.Select(x => x.ToLower()).Contains(domain.ToLower());
            }
            else
            {
                throw new ArgumentNullException("domain", "Domain name cannot be null or empty.");
            }
        }

        private static bool IsDnsWhiteListed(string domain)
        {
            if (!String.IsNullOrEmpty(domain))
            {
                return Configuration.Instance.DnsWhiteList.Select(x => x.ToLower()).Contains(domain.ToLower());
            }
            else
            {
                throw new ArgumentNullException("domain", "Domain name cannot be null or empty.");
            }
        }

        private static string GetDomainFromHostName(string HostName)
        {
            if (HostName.IndexOf(".") != -1)
            {
                return HostName.Substring(HostName.IndexOf("."));
            }
            else
            {
                return HostName;
            }
        }

        private static async Task<string> GetHostNameFromDns(IPAddress ipAddress)
        {
            string Result = String.Empty;

            try
            {
                IPHostEntry Entry = await Dns.GetHostEntryAsync(ipAddress);
                return Entry.HostName;
            }
            catch
            {
                return String.Empty;
            }
        }

        #endregion

        internal class IPAddressRange
        {
            readonly AddressFamily addressFamily;
            readonly byte[] lowerBytes;
            readonly byte[] upperBytes;

            internal IPAddressRange(IPAddress lower, IPAddress upper)
            {
                // Assert that lower.AddressFamily == upper.AddressFamily

                this.addressFamily = lower.AddressFamily;
                this.lowerBytes = lower.GetAddressBytes();
                this.upperBytes = upper.GetAddressBytes();
            }

            internal bool IsInRange(IPAddress address)
            {
                if (address.AddressFamily != addressFamily)
                {
                    return false;
                }

                byte[] addressBytes = address.GetAddressBytes();

                bool lowerBoundary = true, upperBoundary = true;

                for (int i = 0; i < this.lowerBytes.Length &&
                    (lowerBoundary || upperBoundary); i++)
                {
                    if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                        (upperBoundary && addressBytes[i] > upperBytes[i]))
                    {
                        return false;
                    }

                    lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                    upperBoundary &= (addressBytes[i] == upperBytes[i]);
                }

                return true;
            }
        }
    }
}
