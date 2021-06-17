using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace BAMCIS.SimpleSmtp
{
    public static class SmtpMessageStringParser
    {
        private static readonly string _MULTIPART = "multipart/";
        private static readonly string _END_OF_LINE = "(?:\\r\\n)";
        //Gets the first key after the header, then check for a ; or end of line, 
        //then check for optional additional content until it reaches an end of line 
        //or a new line with a character starting the line. This accomodates multiline 
        //headers because their additional lines are all indented 1 space in
        private static readonly string _HEADER_COMPONENT_AFTER_KEY = "\\s*(.*?)(?:;|\\r?\\n)(.*?(?=\\r?\\n\\r?\\n|^[a-zA-Z]))?";
        private static readonly string _BOUNDARY_SECTIONS_STR = "(?:^--{0}" + _END_OF_LINE + "(.*?(?=^--{0})))";

        private static readonly Regex _MESSAGE_MIME_VERSION = new Regex("^MIME-Version:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _X_SENDER = new Regex("^X-Sender:\\s+(.*?)" + _END_OF_LINE, RegexOptions.Multiline);
        private static readonly Regex _SENDER = new Regex("^Sender:\\s+(.*?)" + _END_OF_LINE, RegexOptions.Multiline);
        private static readonly Regex _RECIPIENT = new Regex("^X-Receiver:\\s+(.*?)" + _END_OF_LINE, RegexOptions.Multiline);

        private static readonly Regex _MESSAGE_FROM = new Regex("^From:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_TO = new Regex("^To:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_CC = new Regex("^Cc:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_BCC = new Regex("^Bcc:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_DATE = new Regex("^Date:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_SUBJECT = new Regex("^Subject:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //private static readonly Regex _MESSAGE_CONTENT_TYPE = new Regex("(^Content-Type:\\s*(.*?(?:\\r?\\n\\s+.*?)?)\\r?\\n)", RegexOptions.IgnoreCase |RegexOptions.Multiline | RegexOptions.Singleline);
        private static readonly Regex _BOUNDARY_DELIMITER = new Regex("^Content-Type:\\s*multipart\\/.*?;\\s+boundary=(.+?)" + _END_OF_LINE, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private static readonly Regex _MESSAGE_BODY_ENCODING = new Regex("^Content-Type:.*?;\\s*charset=(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_ATTACHMENT_NAME_FROM_CONTENTTYPE = new Regex("^Content-Type:.*?;\\s*name=(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase);

        private static readonly Regex _MESSAGE_CONTENT_TRANSFER_ENCODING = new Regex("^Content-Transfer-Encoding:\\s+(.*?)" + _END_OF_LINE, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static readonly Regex _MESSAGE_BODY = new Regex("^.*?(?:\\r?\\n\\r?\\n)(.*)$", RegexOptions.Singleline);

        private static readonly Regex _MESSAGE_CONTENT_TYPE = new Regex("(^Content-Type:" + _HEADER_COMPONENT_AFTER_KEY + _END_OF_LINE + ")", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
        private static readonly Regex _MESSAGE_CONTENT_DISPOSITION = new Regex("^Content-Disposition:" + _HEADER_COMPONENT_AFTER_KEY, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        
        public static MailMessage Parse(string smtpString)
        {
            Match Sender = _SENDER.Match(smtpString);
            IEnumerable<Match> Recipient = _RECIPIENT.Matches(smtpString).Cast<Match>();
            Match MessageFrom = _MESSAGE_FROM.Match(smtpString);
            Match MessageTo = _MESSAGE_TO.Match(smtpString);
            Match MessageCc = _MESSAGE_CC.Match(smtpString);
            Match MessageBcc = _MESSAGE_BCC.Match(smtpString);
            Match MessageSubject = _MESSAGE_SUBJECT.Match(smtpString);
            Match MessageDate = _MESSAGE_DATE.Match(smtpString);
            Match MessageMimeVersion = _MESSAGE_MIME_VERSION.Match(smtpString);
            Match MessageContentType = _MESSAGE_CONTENT_TYPE.Match(smtpString);
            Match MessageContentTransferEncoding = _MESSAGE_CONTENT_TYPE.Match(smtpString);
            Match MessageBody = _MESSAGE_BODY.Match(smtpString);

            string SenderAddress = String.Empty;
            List<string> Recipients = new List<string>();
            string MsgFrom = String.Empty;
            string MsgTo = String.Empty;
            string MsgCc = String.Empty;
            string MsgBcc = String.Empty;
            string MsgSubject = String.Empty;
            string MsgDate = String.Empty;
            string MsgMime = String.Empty;
            string MsgBody = String.Empty;

            Debug.Write(smtpString);

            if (Sender.Success)
            {
                SenderAddress = Sender.Groups[1].Value;
            }

            Recipients = Recipient.Where(x => x.Success).Select(x => x.Groups[1].Value).ToList();

            if (MessageFrom.Success)
            {
                MsgFrom = MessageFrom.Groups[1].Value;
            }

            if (MessageTo.Success)
            {
                MsgTo = MessageTo.Groups[1].Value;
            }

            if (MessageCc.Success)
            {
                MsgCc = MessageCc.Groups[1].Value;
            }

            if (MessageBcc.Success)
            {
                MsgBcc = MessageBcc.Groups[1].Value;
            }

            if (MessageSubject.Success)
            {
                MsgSubject = MessageSubject.Groups[1].Value;
            }

            if (MessageDate.Success)
            {
                MsgDate = MessageDate.Groups[1].Value;
            }

            if (MessageMimeVersion.Success)
            {
                MsgMime = MessageMimeVersion.Groups[1].Value;
            }

            if (MessageBody.Success)
            {
                MsgBody = MessageBody.Groups[1].Value.TrimEnd('\r', '\n').TrimStart('\r', '\n');
            }

            MailMessage Msg = new MailMessage(MsgFrom, MsgTo, MsgSubject, MsgBody);

            if (!String.IsNullOrEmpty(SenderAddress))
            {
                Msg.Sender = new MailAddress(SenderAddress);
            }

            if (!String.IsNullOrEmpty(MsgCc))
            {
                Msg.CC.Add(MsgCc);
            }

            //Calculate the users listed in the To line and Cc line. Any other recipients must be Bcc
            List<string> ToandCC = new List<string>();
            ToandCC.AddRange(MsgTo.Split(',').Select(x => x.Trim()));
            ToandCC.AddRange(MsgCc.Split(',').Select(x => x.Trim()));

            if (Recipients.Count > ToandCC.Count)
            {
                Msg.Bcc.Add(String.Join(", ", Recipients.Where(x => !ToandCC.Contains(x))));
            }

            //Determine the actual body of the message
            if (MessageContentType.Success)
            {
                string Header = MessageContentType.Groups[1].Value;

                if (!Header.EndsWith("\r\n"))
                {
                    Header += "\r\n";
                }

                Tuple<List<SmtpTextBodyPart>, List<Attachment>> Result = ParseSmtpBody(String.Format("{0}{1}", Header, MsgBody));

                //Add non attachment parts to the message body
                foreach (SmtpTextBodyPart Part in Result.Item1)
                {
                    Msg.Body += Part.Content + "\r\n";
                }

                if (Result.Item1.Count > 0)
                {
                    //Get the transfer encoding for the body since it's just a single part
                    Msg.BodyTransferEncoding = Result.Item1.First().ContentTransferEncoding;
                    Msg.BodyEncoding = Result.Item1.First().Charset;
                }
                else
                {
                    Msg.BodyEncoding = Encoding.Default;
                }

                
                //Add the attachments
                foreach (Attachment Att in Result.Item2)
                {
                    Msg.Attachments.Add(Att);
                }

                return Msg;
            }
            else
            {
                throw new FormatException("The SMTP message had an improperly formatted Content-Type header.");
            }
        }

        private static Tuple<List<SmtpTextBodyPart>, List<Attachment>> ParseSmtpBody(string body)
        {
            List<SmtpTextBodyPart> BodyContents = new List<SmtpTextBodyPart>();
            List<Attachment> Attachments = new List<Attachment>();

            Match ContentTypeMatch = _MESSAGE_CONTENT_TYPE.Match(body);

            if (ContentTypeMatch.Success)
            {
                string ContentType = ContentTypeMatch.Groups[2].Value;

                //Multipart message, get the boundary delimiter, break up the content into the chunks, and send
                //each of those back to the function to be evaluated
                if (ContentType.StartsWith(_MULTIPART))
                {
                    Match BoundaryMatch = _BOUNDARY_DELIMITER.Match(body);

                    if (BoundaryMatch.Success)
                    {
                        string Boundary = BoundaryMatch.Groups[1].Value;
                        MatchCollection Matches = Regex.Matches(body, String.Format(_BOUNDARY_SECTIONS_STR, Boundary), RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Singleline);

                        foreach (Match Item in Matches)
                        {
                            if (Item.Success)
                            {
                                string SubBodyPart = Item.Groups[1].Value;
                                Tuple<List<SmtpTextBodyPart>, List<Attachment>> Results = ParseSmtpBody(SubBodyPart);
                                BodyContents.AddRange(Results.Item1);
                                Attachments.AddRange(Results.Item2);
                            }
                        }
                    }
                    else
                    {
                        throw new FormatException(String.Format("Content-Type was formatted incorrectly, the boundary delimiter could not be identified: {0}", body));
                    }
                }
                else
                {
                    //This is not a multipart, so we can determine whether the content is body text or an attachment now
                    Match ContentDispositionMatch = _MESSAGE_CONTENT_DISPOSITION.Match(body);

                    //This is an attachment
                    if (ContentDispositionMatch.Success)
                    {
                        //Get the contents of the attachment
                        Match BodyMatch = _MESSAGE_BODY.Match(body);
                        string MessageBody = String.Empty;

                        if (BodyMatch.Success)
                        {
                            MessageBody = BodyMatch.Groups[1].Value;
                        }

                        //Get the attachment name
                        string Name = String.Empty;

                        Match NameMatch = _MESSAGE_ATTACHMENT_NAME_FROM_CONTENTTYPE.Match(body);

                        if (NameMatch.Success)
                        {
                            Name = NameMatch.Groups[1].Value;
                        }

                        /*
                        SmtpSerializableAttachment Attach = new SmtpSerializableAttachment(Convert.FromBase64String(MessageBody), Name);
                        ContentDisposition Disposition = ParseContentDisposition(body);
                        UpdateContentDisposition(Disposition, Attach);

                        Attach.TransferEncoding = ParseTransferEncoding(body);

                        if (Attach.ContentDisposition.Size == 0)
                        {
                            Attach.ContentDisposition.Size = Attach.Content.Length;
                        }
                        */

                        
                        Attachment Att;

                        /*using (MemoryStream MStream = new MemoryStream(Convert.FromBase64String(MessageBody)))
                        {
                            Att = new Attachment(MStream, Name);
                            Att.ContentDisposition.Size = MStream.Length;
                        }*/

                        //This will get disposed when the mail message gets disposed
                        MemoryStream MStream = new MemoryStream(Convert.FromBase64String(MessageBody));
                        Att = new Attachment(MStream, Name);
                        Att.ContentDisposition.Size = MStream.Length;
                        
                        //Get the transfer encoding
                        Att.TransferEncoding = ParseTransferEncoding(body);

                        //Update the Content-Disposition information
                        ContentDisposition Disposition = ParseContentDisposition(body);
                        UpdateContentDisposition(Disposition, Att);                       
                        

                        //Add the new attachment to the list
                        Attachments.Add(Att);
                    }
                    else
                    {
                        BodyContents.Add(new SmtpTextBodyPart(body));
                    }
                }
            }
            else
            {
                throw new FormatException(String.Format("The SMTP message was formatted incorrectly, the Content-Type header could not be identified: {0}", body));
            }

            return new Tuple<List<SmtpTextBodyPart>, List<Attachment>>(BodyContents, Attachments);
        }

        private static Attachment UpdateContentDisposition(ContentDisposition disposition, Attachment attachment)
        {
            attachment.ContentDisposition.DispositionType = disposition.DispositionType;
            attachment.ContentDisposition.FileName = disposition.FileName;

            attachment.ContentDisposition.ReadDate = disposition.ReadDate;
            attachment.ContentDisposition.CreationDate = disposition.CreationDate;
            attachment.ContentDisposition.ModificationDate = disposition.ModificationDate;

            foreach (string Key in disposition.Parameters.Keys)
            {
                if (attachment.ContentDisposition.Parameters.ContainsKey(Key))
                {
                    attachment.ContentDisposition.Parameters[Key] = disposition.Parameters[Key];
                }
                else
                {
                    attachment.ContentDisposition.Parameters.Add(Key, disposition.Parameters[Key]);
                }
            }

            if (disposition.DispositionType.Equals(DispositionTypeNames.Inline))
            {
                attachment.ContentDisposition.Inline = true;
            }

            return attachment;
        }

        private static ContentDisposition ParseContentDisposition(string smtpBody)
        {
            ContentDisposition Disposition = new ContentDisposition();

            Match DispositionMatch = _MESSAGE_CONTENT_DISPOSITION.Match(smtpBody);

            if (DispositionMatch.Success)
            {
                string DispositionType = DispositionMatch.Groups[1].Value;

                switch (DispositionType)
                {
                    case "attachment":
                        {
                            Disposition.DispositionType = DispositionTypeNames.Attachment;
                            break;
                        }
                    case "inline":
                        {
                            Disposition.DispositionType = DispositionTypeNames.Inline;
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException(String.Format("The Content-Disposition was not of type attachment or inline, {0} was provided.", DispositionType));
                        }
                }

                //See if there was additional content
                if (DispositionMatch.Groups.Count > 2)
                {
                    string[] Parts = DispositionMatch.Groups[2].Value.Split(';');

                    foreach (string Part in Parts)
                    {
                        string[] SubParts = Part.Split('=');

                        if (SubParts.Length > 1)
                        {
                            string Key = SubParts[0].Trim().ToLower();
                            string Value = SubParts[1].Trim().Trim('"').ToLower();

                            switch (Key)
                            {
                                case "filename":
                                    {
                                        Disposition.FileName = Value;
                                        break;
                                    }
                                case "creation-date":
                                    {
                                        Disposition.CreationDate = DateTime.Parse(Value);
                                        break;
                                    }
                                case "modification-date":
                                    {
                                        Disposition.ModificationDate = DateTime.Parse(Value);
                                        break;
                                    }
                                case "read-date":
                                    {
                                        Disposition.ReadDate = DateTime.Parse(Value);
                                        break;
                                    }
                                case "size":
                                    {
                                        int Size = 0;
                                        if (Int32.TryParse(Value, out Size))
                                        {
                                            Disposition.Size = Size;
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        Disposition.Parameters[Key] = Value;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            else
            {
                throw new FormatException(String.Format("The provided Content-Disposition header was not formatted properly: {0}", smtpBody));
            }

            return Disposition;
        }

        private static Encoding ParseCharacterSet(string smtpBody)
        {
            Match EncodingMatch = _MESSAGE_BODY_ENCODING.Match(smtpBody);

            if (EncodingMatch.Success)
            {
                string Charset = EncodingMatch.Groups[1].Value;
                return Encoding.GetEncoding(Charset);
            }
            else
            {
                return Encoding.Default;
            }
        }

        private static string ParseMimeType(string contentType)
        {
            Match ContentMatch = _MESSAGE_CONTENT_TYPE.Match(contentType);

            if (ContentMatch.Success)
            {
                string Type = contentType.Split(';')[0];
                return Type;
            }
            else
            {
                return MediaTypeNames.Text.Plain;
            }
        }

        private static TransferEncoding ParseTransferEncoding(string smtpBody)
        {
            Match TransferEncodingMatch = _MESSAGE_CONTENT_TRANSFER_ENCODING.Match(smtpBody);

            if (TransferEncodingMatch.Success)
            {
                string TransferEncoding = TransferEncodingMatch.Groups[1].Value;

                switch (TransferEncoding.ToLower())
                {
                    case "base64":
                        {
                            return System.Net.Mime.TransferEncoding.Base64;
                        }
                    case "8bit":
                        {
                            return System.Net.Mime.TransferEncoding.EightBit;
                        }
                    case "quoted-printable":
                        {
                            return System.Net.Mime.TransferEncoding.QuotedPrintable;
                        }
                    case "7bit":
                        {
                            return System.Net.Mime.TransferEncoding.SevenBit;
                        }
                    default:
                        {
                            return System.Net.Mime.TransferEncoding.Unknown;
                        }
                }
            }
            else
            {
                return TransferEncoding.Unknown;
            }
        }

        private class SmtpTextBodyPart
        {
            internal Encoding Charset { get; set; }
            internal string Content { get; set; }
            internal TransferEncoding ContentTransferEncoding { get; set; }

            internal SmtpTextBodyPart(string bodyPart)
            {
                this.Charset = ParseCharacterSet(bodyPart);

                Match BodyMatch = _MESSAGE_BODY.Match(bodyPart);

                if (BodyMatch.Success)
                {
                    this.Content = BodyMatch.Groups[1].Value.TrimStart('\r', '\n').TrimEnd('\r', '\n');
                }
                else
                {
                    this.Content = String.Empty;
                }

                this.ContentTransferEncoding = ParseTransferEncoding(bodyPart);
            }
        }
    }
}
