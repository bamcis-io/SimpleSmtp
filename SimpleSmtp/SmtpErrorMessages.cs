using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.SimpleSmtp
{
    public static class SmtpErrorMessages
    {
        #region 220 Messages - Server Ready

        public static readonly string _220_BANNER_MESSAGE = String.Format("{0} {1} ESMTP BAMCIS SimpleSmtp Server Version 0.0.0.1 ready at {{0}}", (int)SmtpErrorCodeEnum._220_SERVER_READY, Configuration.Instance.Domain);
        public static readonly string _220_READY_TO_START_TLS_MESSAGE = String.Format("{0} 2.0.0 Ready to start TLS", (int)SmtpErrorCodeEnum._220_SERVER_READY);

        #endregion

        #region 221 Messages - Closing Connection

        public static readonly string _221_CLOSING_CONNECTION_MESSAGE = String.Format("{0} 2.0.0 {1} closing connection", (int)SmtpErrorCodeEnum._221_CLOSING_CONNECTION, Configuration.Instance.Domain);

        #endregion

        #region 235 Messages - Authentication Successful

        public static readonly string _235_AUTH_SUCCESSFUL_MESSAGE = String.Format("{0} Authentication successful", (int)SmtpErrorCodeEnum._235_AUTH_SUCCESSFUL);

        #endregion

        #region 250 Messages - Action Completed

        public static readonly string _250_HELO_RESPONSE_MESSAGE = String.Format("{0}-{1} Hello [{{0}}], nice to meet you.", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED, Configuration.Instance.Domain);
        //All lines for multiline must have a hypen, CODE-FirstLine\r\nCODE-SecondLine\r\n...CODE LastLine, except the last line, which signals the end of the response
        public static readonly string _250_EHLO_RESPONSE_MESSAGE = String.Format("{0}-{1} Hello [{{0}}], nice to meet you.\r\n" +
            "{0}-8BITMIME\r\n" +
            "{0}-SIZE {2}\r\n" +
            "{0}-HELP\r\n" +
            "{0}-VRFY\r\n" + 
            "{0}-EXPN\r\n" +
            "{0}-NOOP\r\n" +
            "{0} STARTTLS" ,
            (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED, Configuration.Instance.Domain, Configuration.Instance.MaxMessageSizeInKiB * 1024);

        public static readonly string _250_SENDER_OK_MESSAGE = String.Format("{0} 2.1.0 {{0}}... Sender ok", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_RECIPIENT_OK_MESSAGE = String.Format("{0} 2.1.5 {{0}}... Recipient ok", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_MESSAGE_ACCEPTED_MESSAGE = String.Format("{0} 2.0.0 Message accepted for delivery", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_NOOP_OK_MESSAGE = String.Format("{0} 2.0.0 OK", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_RESET_OK_MESSAGE = String.Format("{0} 2.0.0 Reset state", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_VERIFY_OK_MESSAGE = String.Format("{0} 2.1.5 {{0}}", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);
        public static readonly string _250_EXPAND_OK_MESSAGE = String.Format("{0} 2.1.5 {{0}}", (int)SmtpErrorCodeEnum._250_MAIL_ACTION_COMPLETED);

        #endregion

        #region 354 Messages - Ready for body

        public static readonly string _354_READY_FOR_MAIL_BODY_MESSAGE = String.Format("{0} Enter mail, end data with <CR><LF>.<CR><LF> (a period on a line by itself)", (int)SmtpErrorCodeEnum._354_DATA_CMD_RESPONSE);

        #endregion

        #region 421 Errors - Server Unavailable

        public static readonly string _421_TEMPORARY_SYSTEM_PROBLEM_MESSAGE = String.Format("{0} 4.3.2 Temporary System Problem. Try again later.", (int)SmtpErrorCodeEnum._421_SERVER_UNAVAILABLE);
        public static readonly string _421_TRY_AGAIN_LATER_MESSAGE = String.Format("{0} 4.3.2 Try again later, closing connection.", (int)SmtpErrorCodeEnum._421_SERVER_UNAVAILABLE);
        public static readonly string _421_SERVER_BUSY_MESSAGE = String.Format("{0} 4.3.2 Server busy, try again later.", (int)SmtpErrorCodeEnum._421_SERVER_UNAVAILABLE);

        #endregion

        #region 442 Errors - Connection Dropped

        public static readonly string _442_DNS_OR_IP_DENIED_MESSAGE = String.Format("{0} 5.3.2 Connection dropped, host denied", (int)SmtpErrorCodeEnum._442_CONNECTION_DROPPED);
        public static readonly string _442_TIMEOUT_MESSAGE = String.Format("{0} 4.4.2 Connection timed out", (int)SmtpErrorCodeEnum._442_CONNECTION_DROPPED);

        #endregion

        #region 451 Errors - Action Aborted

        public static readonly string _451_TEMPORARILY_REJECTED_MESSAGE = String.Format("{0} 4.3.0 Mail server temporarily rejected message", (int)SmtpErrorCodeEnum._451_ACTION_ABORTED);
        public static readonly string _451_EXCEEDED_MESSAGING_LIMIT_MESSAGE = String.Format("{0} 4.3.1 You have exceeded your messaging limits", (int)SmtpErrorCodeEnum._451_ACTION_ABORTED);
        public static readonly string _451_EXCEEDED_VERIFY_LIMIT_MESSAGE = String.Format("{0} 4.2.0 You have exceeded the maximum VRFY commands allowed", (int)SmtpErrorCodeEnum._451_ACTION_ABORTED);

        #endregion

        #region 452 Errors - Too many recipients or emails

        public static readonly string _452_EXCEEDED_RECIPIENT_LIMIT_MESSAGE = String.Format("{0} 4.2.0 You have exceeded the maximum number of recipients.", (int)SmtpErrorCodeEnum._452_STORAGE_LIMIT_EXCEEDED);

        #endregion

        #region 500 Erros - Syntax Error - Commands

        public static readonly string _500_COMMAND_SYNTAX_ERROR_MESSAGE = String.Format("{0} 5.5.1 Command syntax error", (int)SmtpErrorCodeEnum._500_SYNTAX_ERROR_COMMAND);
        public static readonly string _500_SYNTAX_ERROR_INVALID_CHARACTER = String.Format("{0} 5.5.1 Syntax error - invalid character", (int)SmtpErrorCodeEnum._500_SYNTAX_ERROR_COMMAND);

        #endregion

        #region 501 Errors - Sytanx Error - Arguments

        public static readonly string _501_MISSING_ARG_MESSAGE = String.Format("{0} 5.5.4 Missing required argument", (int)SmtpErrorCodeEnum._501_SYNTAX_ERROR_ARG);
        public static readonly string _501_BAD_ARG_MESSAGE = String.Format("{0} 5.5.4 Invalid argument provided", (int)SmtpErrorCodeEnum._501_SYNTAX_ERROR_ARG);

        #endregion

        #region 502 Errors - Command not implemented

        public static readonly string _502_COMMAND_NOT_IMPLEMENTED_MESSAGE = String.Format("{0} 5.5.1 Command not implemented", (int)SmtpErrorCodeEnum._502_COMMAND_NOT_IMPLEMENTED);

        #endregion

        #region 503 Errors - Command Sequence

        public static readonly string _503_CMD_OUT_OF_ORDER_MESSAGE = String.Format("{0} 5.5.1 Command sent out of order", (int)SmtpErrorCodeEnum._503_BAD_COMMAND_SEQUENCE);

        #endregion

        #region 504 Errors - Argument not implemented

        public static readonly string _504_ARG_NOT_IMPLEMENTED_MESSAGE = String.Format("{0} 5.5.4 Argument not implemented", (int)SmtpErrorCodeEnum._504_ARG_NOT_IMPLEMENTED);

        #endregion

        #region 523 Errors - Message too large

        public static readonly string _523_MESSAGE_SIZE_EXCEEDED_MESSAGE = String.Format("{0} 5.2.3 Message length exceeds administrative limit", (int)SmtpErrorCodeEnum._523_SIZE_EXCEEDED);

        #endregion

        #region 530 Errors - Authentication Required

        public static readonly string _530_AUTH_REQUIRED_MESSAGE = String.Format("{0} 5.7.0 Authentication Required", (int)SmtpErrorCodeEnum._530_ACCESS_DENIED);
        public static readonly string _530_MUST_USE_STARTTLS_MESSAGE = String.Format("{0} 5.7.0 Must issue a STARTTLS command first", (int)SmtpErrorCodeEnum._530_ACCESS_DENIED);

        #endregion

        #region 535 Errors - Autentication Unsuccessful

        public static readonly string _535_AUTH_UNSUCCESSFUL_MESSAGE = String.Format("{0} 5.7.3 Authentication unsuccessful", (int)SmtpErrorCodeEnum._535_AUTH_UNSUCCESSFUL);

        #endregion

        #region 550 Errors - Recipient Does Not Exist

        public static readonly string _550_MAILBOX_UNAVAILABLE_MESSAGE = String.Format("{0} 5.1.1 Requested action not taken: mailbox unavailable [E.g. mailbox not found, no access]", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_UNABLE_TO_RELAY_MESSAGE = String.Format("{0} 5.7.1 Unable to relay for", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_ACCOUNT_DISABLED_MESSAGE = String.Format("{0} 5.2.1 The email account that you tried to reach is disabled.", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_INVALID_RECIPIENT_MESSAGE = String.Format("{0} 5.1.1 Invalid recipient", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_NO_SUCH_USER_MESSAGE = String.Format("{0} 5.1.1 No such user here", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_DAILY_SENDING_QUOTA_EXCEEDED_MESSAGE = String.Format("{0} 5.4.5 Daily sending quota exceeded", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_EMAIL_QUOTA_EXCEEDED_MESSAGE = String.Format("{0} 5.7.1 Email quota exceeded", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_SMTP_RELAY_LIMIT_EXCEEDED_MESSAGE = String.Format("{0} 5.7.1 Daily SMTP relay limit exceeded for customer", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_VERIFY_FAILED_MESSAGE = String.Format("{0} 5.1.1 {{0}} ... User unknown", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        public static readonly string _550_EXPAND_FAILED_MESSAGE = String.Format("{0} 5.1.1 {{0}} ... User unknown", (int)SmtpErrorCodeEnum._550_RECIPIENT_DOES_NOT_EXIST);
        

        #endregion

        #region 553 Errors - Invalid Mailbox Name

        public static readonly string _553_MAILBOX_NAME_NOT_ALLOWED_MESSAGE = String.Format("{0} 5.1.3 Requested action not taken: mailbox name not allowed", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);
        public static readonly string _553_MAILBOX_NAME_INVALID_MESSAGE = String.Format("{0} 5.1.3 Mailbox name invalid", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);
        public static readonly string _553_OVER_DAILY_LIMIT_MESSAGE = String.Format("{0} Sorry, over your daily relay limit", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);
        public static readonly string _553_QUOTA_EXCEEDED_MESSAGE = String.Format("{0} Quota exceeded", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);

        public static readonly string _553_DOMAIN_NAME_REQUIRED_MESSAGE = String.Format("{0} 5.1.8 {{0}}... Domain name required for sender address {{0}}", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);
        public static readonly string _553_SENDER_DOMAIN_NON_EXISTENT_MESSAGE = String.Format("{0} 5.1.8 {{0}}... Domain of sender address {{0}} does not exist", (int)SmtpErrorCodeEnum._553_MAILBOX_NAME_INVALID);

        #endregion

        #region 554 Errors - Transaction Failed

        public static readonly string _554_MAX_ERRORS_REACHED_MESSAGE = String.Format("{0} Maximum session errors reached, closing connection", (int)SmtpErrorCodeEnum._554_TRANSACTION_FAILED);
        public static readonly string _554_EARLY_TALKER_MESSAGE = String.Format("{0} 5.4.2 Misbehaved SMTP session (EarlyTalker)", (int)SmtpErrorCodeEnum._554_TRANSACTION_FAILED);
        public static readonly string _554_SPOOFED_MESSAGE = String.Format("{0} 5.7.1 Message spoofed", (int)SmtpErrorCodeEnum._554_TRANSACTION_FAILED);

        #endregion
    }
}
