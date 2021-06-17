using BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums;
using System;

namespace BAMCIS.SimpleSmtp.ResponseMessages
{
    public class SmtpResponseMessage
    {
        private string _message;
        private int _code;
        private ClassEnum _responseType;
        private SubjectEnum _subject;
        private int _supplementalInformation;
        private int _stringAdditions;

        #region 220 Messages - Server Ready

        public static readonly SmtpResponseMessage _220_BANNER = new SmtpResponseMessage()
        {
            _message = String.Format("{0} ESMTP BAMCIS SimpleSmtp Server Version 0.0.0.1 ready at {{0}}", Configuration.Instance.Domain),
            _code = 220,
            _responseType = ClassEnum.NONE,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _220_READY_TO_START_TLS = new SmtpResponseMessage()
        {
            _message = "Ready to start TLS",
            _code = 220,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        #endregion

        #region 221 Messages - Closing Connection

        public static readonly SmtpResponseMessage _221_CLOSING_CONNECTION = new SmtpResponseMessage()
        {
            _message = String.Format("{0} closing connection", Configuration.Instance.Domain),
            _code = 221,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        #endregion

        #region 235 Messages - Authentication Successful

        public static readonly SmtpResponseMessage _235_AUTH_SUCCESSFUL = new SmtpResponseMessage()
        {
            _message = "Authentication successful",
            _code = 235,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        #endregion

        #region 250 Messages - Action Completed

        public static readonly SmtpResponseMessage _250_HELO_RESPONSE = new SmtpResponseMessage()
        {
            _message = String.Format("{0} Hello [{{0}}], nice to meet you.", Configuration.Instance.Domain),
            _code = 250,
            _responseType = ClassEnum.NONE,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _250_EHLO_RESPONSE = new SmtpResponseMessage()
        {
            _message = String.Format("{0} Hello [{{0}}], nice to meet you.\r\n" +
                "8BITMIME\r\n" +
                "SIZE {1}\r\n" +
                "HELP\r\n" +
                "VRFY\r\n" +
                "EXPN\r\n" +
                "NOOP\r\n" +
                "STARTTLS",
                Configuration.Instance.Domain, Configuration.Instance.MaxMessageSizeInKiB * 1024
            ),
            _code = 250,
            _responseType = ClassEnum.NONE,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _250_SENDER_OK = new SmtpResponseMessage()
        {
            _message = "{{0}}... Sender ok",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.OTHER_ADDRESS_STATUS,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _250_RECIPIENT_OK = new SmtpResponseMessage()
        {
            _message = "{{0}}... Recipient ok",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.DESTINATION_ADDRESS_VALID,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _250_ACCEPTED = new SmtpResponseMessage()
        {
            _message = "Message accepted for delivery",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        public static readonly SmtpResponseMessage _250_NOOP_OK = new SmtpResponseMessage()
        {
            _message = "OK",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        public static readonly SmtpResponseMessage _250_RESET_OK = new SmtpResponseMessage()
        {
            _message = "Reset state",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.UNDEFINED_STATUS,
            _supplementalInformation = 0
        };

        public static readonly SmtpResponseMessage _250_VERIFY_OK = new SmtpResponseMessage()
        {
            _message = "{0}",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.DESTINATION_ADDRESS_VALID,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _250_EXPAND_OK = new SmtpResponseMessage()
        {
            _message = "{0}",
            _code = 250,
            _responseType = ClassEnum.SUCCESS,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.DESTINATION_ADDRESS_VALID,
            _stringAdditions = 1
        };

        #endregion

        #region 354 Messages - Ready for body

        public static readonly SmtpResponseMessage _354_READY_FOR_MAIL_BODY = new SmtpResponseMessage()
        {
            _message = "Enter mail data, end data with <CR><LF>.<CR><LF> (a period on a line by itself)",
            _code = 354,
            _responseType = ClassEnum.NONE
        };

        #endregion

        #region 421 Errors - Server Unavailable

        public static readonly SmtpResponseMessage _421_BAD_CONNECTION = new SmtpResponseMessage()
        {
            _message = "Connection established, but could not complete message transaction",
            _code = 421,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.BAD_CONNECTION
        };

        public static readonly SmtpResponseMessage _421_CONNECTION_LIMIT_EXCEEDED = new SmtpResponseMessage()
        {
            _message = "Connection limit has been exceeded, try again later",
            _code = 421,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.SYSTEM_NOT_ACCEPTING_MESSAGES
        };

        public static readonly SmtpResponseMessage _421_MAX_ERRORS_REACHED = new SmtpResponseMessage()
        {
            _message = "Maximum errors reached, closing connection",
            _code = 421,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.OTHER
        };

        public static readonly SmtpResponseMessage _421_TEMPORARY_SYSTEM_PROBLEM = new SmtpResponseMessage()
        {
            _message = "Temporary system problem, try again later",
            _code = 421,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.SYSTEM_NOT_ACCEPTING_MESSAGES
        };

        public static readonly SmtpResponseMessage _421_SERVER_BUSY = new SmtpResponseMessage()
        {
            _message = "Server busy, try again later",
            _code = 421,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.SYSTEM_NOT_ACCEPTING_MESSAGES
        };

        #endregion

        #region 422 Errors - Mailbox exceeded storage limit

        public static readonly SmtpResponseMessage _422_MAILBOX_FULL = new SmtpResponseMessage()
        {
            _message = "Recipient mailbox full",
            _code = 422,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAILBOX_STATUS,
            _supplementalInformation = (int)MailboxStatusEnum.MAILBOX_FULL
        };

        #endregion

        #region 442 Errors - Connection Dropped

        public static readonly SmtpResponseMessage _442_CONNECTION_DROPPED = new SmtpResponseMessage()
        {
            _message = "Connection has been dropped",
            _code = 442,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.BAD_CONNECTION
        };

        #endregion

        #region 450 Errors - Mailbox unavailable

        public static readonly SmtpResponseMessage _450_CANNOT_FIND_SENDER_DOMAIN = new SmtpResponseMessage()
        {
            _message = "Sender address rejected: Domain not found [{0}]",
            _code = 450,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.BAD_SENDER_SYSTEM_ADDRESS,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _450_CANNOT_FIND_SENDER_HOSTNAME = new SmtpResponseMessage()
        {
            _message = "Client host rejected: cannot find your hostname [{0}]",
            _code = 450,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _450_RECIPIENT_ADDRESS_REJECTED_GREYLIST = new SmtpResponseMessage()
        {
            _message = "<{0}> : Recipient address rejected : Policy Rejection - greylisted, please try again later",
            _code = 450,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _450_MAILBOX_UNAVAILABLE = new SmtpResponseMessage()
        {
            _message = "Mailbox temporarily unavailable",
            _code = 450,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAILBOX_STATUS,
            _supplementalInformation = (int)MailboxStatusEnum.MAILBOX_DISABLED
        };
       

        #endregion

        #region 451 Errors - Requested action aborted: local error in processing

        public static readonly SmtpResponseMessage _451_NO_ANSWER_FROM_HOST = new SmtpResponseMessage()
        {
            _message = "No answer from host",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.NO_ANSWER_FROM_HOST
        };

        public static readonly SmtpResponseMessage _451_DIRECTORY_SERVER_UNAVAILABLE = new SmtpResponseMessage()
        {
            _message = "Unable to connect to directory server",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.DIRECTORY_SERVER_FAILURE
        };

        public static readonly SmtpResponseMessage _451_MAIL_SYSTEM_CONGESTED = new SmtpResponseMessage()
        {
            _message = "Mail system congested, unable to deliver message",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.MAIL_SYSTEM_CONGESTION
        };

        public static readonly SmtpResponseMessage _451_TEMPORARILY_REJECTED = new SmtpResponseMessage()
        {
            _message = "Mail server temporarily rejected the message",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.OTHER
        };

        public static readonly SmtpResponseMessage _451_TOO_MANY_RECIPIENTS = new SmtpResponseMessage()
        {
            _message = "Too many recipients",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.TOO_MANY_RECIPIENTS
        };

        public static readonly SmtpResponseMessage _451_DELIVERY_NOT_AUTHORIZED = new SmtpResponseMessage()
        {
            _message = "Delivery not authorized",
            _code = 451,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED
        };

        public static readonly SmtpResponseMessage _451_SPF_VALIDATION_ERROR = new SmtpResponseMessage()
        {
            _message = "SPF validation error",
            _code = 451,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.SPF_VALIDATION_ERROR
        };

        public static readonly SmtpResponseMessage _451_EXCEEDED_MESSAGING_LIMIT = new SmtpResponseMessage()
        {
            _message = "You have exceeded your messaging limits",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.MAIL_SYSTEM_FULL
        };

        public static readonly SmtpResponseMessage _451_EXCEEDED_VERIFY_LIMIT = new SmtpResponseMessage()
        {
            _message = "You have exceeded the maximum VRFY commands allowed",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAILBOX_STATUS,
            _supplementalInformation = (int)MailboxStatusEnum.OTHER
        };

        public static readonly SmtpResponseMessage _451_EXCEEDED_NOOP_LIMIT = new SmtpResponseMessage()
        {
            _message = "You have exceeded the maximum NOOP commands allowed",
            _code = 451,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.OTHER
        };

        #endregion

        #region 452 Errors - Insufficient system storage

        public static readonly SmtpResponseMessage _452_EXCEEDED_RECIPIENT_LIMIT = new SmtpResponseMessage()
        {
            _message = "You have exceeded the maximum number of recipients which is {0}",
            _code = 452,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.TOO_MANY_RECIPIENTS,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _452_MAIL_SYSTEM_FULL = new SmtpResponseMessage()
        {
            _message = "Mail system storage limit has been exceeded",
            _code = 452,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.MAIL_SYSTEM_FULL
        };

        #endregion

        #region 453 Errors - Not accepting messages

        public static readonly SmtpResponseMessage _453_NOT_ACCEPTING_MESSAGES = new SmtpResponseMessage()
        {
            _message = "System not accepting messages due to: {0}",
            _code = 453,
            _responseType = ClassEnum.PERSISTENT_TRANSIENT_FAILURE,
            _subject = SubjectEnum.MAIL_SYSTEM_STATUS,
            _supplementalInformation = (int)MailSystemStatusEnum.SYSTEM_NOT_ACCEPTING_MESSAGES,
            _stringAdditions = 1
        };

        #endregion

        #region 500 Errors - Syntax error, command unrecognized (This may include errors such as command line too long)

        public static readonly SmtpResponseMessage _500_COMMAND_NOT_RECOGNIZED = new SmtpResponseMessage()
        {
            _message = "Command not recognized",
            _code = 500,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.INVALID_COMMAND
        };

        public static readonly SmtpResponseMessage _500_COMMAND_SYNTAX_ERROR = new SmtpResponseMessage()
        {
            _message = "Command syntax error",
            _code = 500,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.INVALID_COMMAND
        };

        public static readonly SmtpResponseMessage _500_INVALID_CHARACTER = new SmtpResponseMessage()
        {
            _message = "Syntax error - invalid character",
            _code = 500,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.SYNTAX_ERROR
        };

        #endregion

        #region 501 Errors - Syntax error in parameters or arguments

        public static readonly SmtpResponseMessage _501_BAD_MAILBOX_ADDRESS_ARG = new SmtpResponseMessage()
        {
            _message = "Destination address syntatically invalid",
            _code = 501,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.BAD_DESTINATION_MAILBOX_ADDRESS_SYNTAX
        };

        public static readonly SmtpResponseMessage _501_BAD_SENDER_ADDRESS_ARG = new SmtpResponseMessage()
        {
            _message = "Sender address syntatically invalid",
            _code = 501,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.BAD_SENDER_SYSTEM_ADDRESS
        };

        public static readonly SmtpResponseMessage _501_MISSING_ARG = new SmtpResponseMessage()
        {
            _message = "Missing required argument",
            _code = 501,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.OTHER
        };

        public static readonly SmtpResponseMessage _501_INVALID_ARG = new SmtpResponseMessage()
        {
            _message = "Invalid argument provided",
            _code = 501,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.INVALID_COMMAND_ARGUMENTS
        };

        #endregion

        #region 502 Errors - Command not implemented

        public static readonly SmtpResponseMessage _502_COMMAND_NOT_IMPLEMENTED = new SmtpResponseMessage()
        {
            _message = "Command not implemented",
            _code = 502,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.SYNTAX_ERROR
        };

        #endregion

        #region 503 Errors - Bad sequence of commands

        public static readonly SmtpResponseMessage _503_COMMAND_OUT_OF_SEQUENCE = new SmtpResponseMessage()
        {
            _message = "Command sent out of sequence",
            _code = 503,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.INVALID_COMMAND
        };

        #endregion

        #region 504 Errors - Command parameter not implemented

        public static readonly SmtpResponseMessage _504_ARG_NOT_IMPLEMENTED = new SmtpResponseMessage()
        {
            _message = "Command parameter not implemented",
            _code = 504,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAIL_DELIVERY_PROTOCOL_STATUS,
            _supplementalInformation = (int)MailDeliveryProtocolStatusEnum.INVALID_COMMAND_ARGUMENTS
        };

        #endregion

        #region 523 Errors - Message too large

        public static SmtpResponseMessage _523_MESSAGE_TOO_LARGE = new SmtpResponseMessage()
        {
            _message = "Message length exceeds administrative limit",
            _code = 523,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.MAILBOX_STATUS,
            _supplementalInformation = (int)MailboxStatusEnum.MESSAGE_LENGTH_EXCEEDS_LIMIT
        };

        #endregion

        #region 530 Errors - Authentication Required

        public static readonly SmtpResponseMessage _530_AUTH_REQUIRED = new SmtpResponseMessage()
        {
            _message = "Authentication required",
            _code = 530,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.OTHER
        };

        public static readonly SmtpResponseMessage _530_MUST_USE_TLS = new SmtpResponseMessage()
        {
            _message = "Must issue a STARTTLS command first",
            _code = 530,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.OTHER
        };

        #endregion

        #region 535 Errors - Autentication Unsuccessful

        public static readonly SmtpResponseMessage _535_AUTH_FAILED = new SmtpResponseMessage()
        {
            _message = "Authentication failed",
            _code = 535,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.AUTH_CREDENTIALS_INVALID
        };

        #endregion

        #region 550 Errors - Requested action not taken: mailbox unavailable

        public static readonly SmtpResponseMessage _550_RECIPIENT_DOES_NOT_EXIST = new SmtpResponseMessage()
        {
            _message = "<{0}> : Recipient address rejected : User unknown",
            _code = 550,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.ADDRESSING_STATUS,
            _supplementalInformation = (int)AddressStatusEnum.BAD_DESTINATION_MAILBOX_ADDRESS,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _550_CANNOT_FIND_SENDER_DOMAIN = new SmtpResponseMessage()
        {
            _message = "Client host rejected : cannot find your hostname [{0}]",
            _code = 550,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.REVERSE_DNS_VALIDATION_FAILED,
            _stringAdditions = 1
        };

        public static readonly SmtpResponseMessage _550_DOMAIN_SENDING_QUOTA_EXCEEDED = new SmtpResponseMessage()
        {
            _message = "Sender address rejected : Hourly domain sending quota exceeded",
            _code = 550,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.MAIL_SYSTEM_CONGESTION
        };

        public static readonly SmtpResponseMessage _550_USER_SENDING_QUOTA_EXCEEDED = new SmtpResponseMessage()
        {
            _message = "Sender address rejected : Hourly sending quota exceeded",
            _code = 550,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.MAIL_SYSTEM_CONGESTION
        };

        #endregion

        #region 554 Errors - Transaction failed

        public static readonly SmtpResponseMessage _554_VIRUS = new SmtpResponseMessage()
        {
            _message = "Sender address rejected: Message contains virus or spam",
            _code = 554,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED
        };

        public static readonly SmtpResponseMessage _554_DNS_OR_IP_BLACKLIST = new SmtpResponseMessage()
        {
            _message = "Sender address rejected : Verification failed, your IP or DNS appears on our DNSBL",
            _code = 554,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.SECURITY_OR_POLICY_STATUS,
            _supplementalInformation = (int)SecurityOrPolicyStatusEnum.DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED
        };

        public static readonly SmtpResponseMessage _554_EARLY_TALKER = new SmtpResponseMessage()
        {
            _message = "Early talker detected",
            _code = 554,
            _responseType = ClassEnum.PERMANENT_FAILURE,
            _subject = SubjectEnum.NETWORK_AND_ROUTING_STATUS,
            _supplementalInformation = (int)NetworkAndRoutingStatusEnum.BAD_CONNECTION
        };

        #endregion

        public string Message
        {
            get
            {
                return this._message;
            }
        }
        public int Code
        {
            get
            {
                return this._code;
            }
        }
        public int ResponseType
        {
            get
            {
                return (int)this._responseType;
            }
        }
        public int Subject
        {
            get
            {
                return (int)this._subject;
            }
        }
        public int SupplementalInformation
        {
            get
            {
                return this._supplementalInformation;
            }
        }
        public int StringAdditions
        {
            get
            {
                return this._stringAdditions;
            }
        }
        public bool AllowedToFormatString
        {
            get
            {
                return this._stringAdditions > 0;
            }
        }

        public bool IsPermanent
        {
            get
            {
                return (this.Code - 500) >= 0;
            }
        }

        internal SmtpResponseMessage()
        {
            this._message = String.Empty;
            this._code = 250;
            this._responseType = ClassEnum.NONE;
            this._subject = SubjectEnum.UNDEFINED_STATUS;
            this._supplementalInformation = 0;
            this._stringAdditions = 0;
        }

        public string GetReplyCode()
        {
            string ReplyCode = String.Empty;

            if (this._responseType != ClassEnum.NONE)
            {
                ReplyCode = String.Format("{0}.{1}.{2}", (int)this._responseType, (int)this._subject, this._supplementalInformation);
            }

            return ReplyCode;
        }

        public override string ToString()
        {
            string ReplyCode = this.GetReplyCode();

            string[] Parts = this._message.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < Parts.Length; i++)
            {
                string Separator = "-";
                if (i == Parts.Length - 1)
                {
                    Separator = " ";
                }

                Parts[i] = this._code + Separator + (String.IsNullOrEmpty(ReplyCode) ? "" : ReplyCode + " ") + Parts[i];
            }

            return String.Join("\r\n", Parts);
        }

        public string ToFormattedString(params string[] args)
        {
            if (args.Length == this._stringAdditions)
            {
                return String.Format(this.ToString(), args);
            }
            else
            {
                throw new ArgumentOutOfRangeException("args", "The number of arguments must be equal to the number of allowed string additions.");
            }
        }
    }
}
