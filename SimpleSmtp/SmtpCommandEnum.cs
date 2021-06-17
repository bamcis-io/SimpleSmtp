namespace BAMCIS.SimpleSmtp
{
    public enum SmtpCommandEnum
    {
        [SmtpCommand(
            Command = "ATRN",
            Comment = "Authenticated TURN. Optionally takes one or more domains as a parameter. " +
                    "The ATRN command must be rejected if the session has not been authenticated."
        )]
        ATRN,

        [SmtpCommand(
            Command = "AUTH",
            Comment = "Authentication"
        )]
        AUTH,

        [SmtpCommand(
            Command = "BDAT",
            Comment = "Binary data"
        )]
        BDAT,

        [SmtpCommand(
            Command = "BURL",
            Comment = "Remote content"
        )]
        BURL,

        [SmtpCommand(
            Command = "CHUNKING",
            Comment = "An ESMTP command that replaces the DATA command. " +
                    "So that the SMTP host does not have to continuously scan for the end of the data, " +
                    "this command sends a BDAT command with an argument that contains the total number of bytes in a message. " +
                    "The receiving server counts the bytes in the message and, when the message size equals the value sent by the BDAT command, " +
                    "the server assumes it has received all of the message data"
        )]
        CHUNKING,

        [SmtpCommand(
            Command = "DATA",
            Comment = "The actual email message to be sent"
        )]
        DATA,

        [SmtpCommand(
            Command = "DSN",
            Comment = "An ESMTP command that enables delivery status notifications"
        )]
        DSN,

        [SmtpCommand(
            Command = "EHLO",
            Comment = "Extended HELO"
        )]
        EHLO,

        [SmtpCommand(
            Command = "ETRN",
            Comment = "Extended turn. An extension of SMTP. " +
                    "ETRN is sent by an SMTP server to request that another server send any e-mail messages that it has."
        )]
        ETRN,

        /*[SmtpCommand(
            Command = "EXPN",
            Comment = "EXPN asks the server for the membership of a mailing list. Its parameter may be an encoded address or a list name in a server-defined format."
        )]
        EXPN,*/

        [SmtpCommand(
            Command = "HELO",
            Comment = "Identify yourself to the SMTP server"
        )]
        HELO,

        [SmtpCommand(
            Command = "HELP",
            Comment = "Show available commands"
        )]
        HELP,

        [SmtpCommand(
            Command = "\r\n",
            Comment = "Invalid command"
        )]
        INVALID,

        [SmtpCommand(
            Command = "MAIL FROM:",
            Comment = "Send mail from email account"
        )]
        MAIL_FROM,

        [SmtpCommand(
            Command = "NOOP",
            Comment = "No-op. Keeps the connection open"
        )]
        NOOP,

        [SmtpCommand(
            Command = "ONEX",
            Comment = "One message transaction only"
        )]
        ONEX,

        [SmtpCommand(
            Command = "PIPELINING",
            Comment = "Provides the ability to send a stream of commands without waiting for a response after each command"
        )]
        PIPELINING,

        [SmtpCommand(
            Command = "QUIT",
            Comment = "End session"
        )]
        QUIT,

        [SmtpCommand(
            Command = "RCPT TO:",
            Comment = "Send email to recipient"
        )]
        RCPT_TO,

        [SmtpCommand(
            Command = "RSET",
            Comment = "Reset"
        )]
        RSET,

        [SmtpCommand(
            Command = "SIZE",
            Comment = "Provides a mechanism by which the SMTP server can indicate the maximum size message supported. " +
                    "Compliant servers must provide size extensions to indicate the maximum size message that can be accepted. " +
                    "Clients should not send messages that are larger than the size indicated by the server."
        )]
        SIZE,

        [SmtpCommand(
            Command = "STARTTLS",
            Comment = "Starts TLS session"
        )]
        START_TLS,

        [SmtpCommand(
            Command = "SUBMITTER",
            Comment = "SMTP responsible submitter"
        )]
        SUBMITTER,

        [SmtpCommand(
           Command = "VERB",
            Comment = "Verbose"
        )]
        VERB,

        [SmtpCommand(
            Command = "VRFY",
            Comment = "A VRFY request asks the server to verify an address. Its parameter may be an encoded address or a user name in a server-defined format."
        )]
        VRFY,

        [SmtpCommand(
            Command = "X-EXPS GSSAPI",
            Comment = "A method that is used by Microsoft Exchange Server 2003 and Exchange 2000 Server servers to authenticate"
        )]
        X_EXPS_GSSAPI,

        [SmtpCommand(
            Command = "X-EXPS=LOGIN",
            Comment = "A method that is used by Exchange 2000 and Exchange 2003 servers to authenticate."
        )]
        X_EXPS_LOGIN,

        [SmtpCommand(
            Command = "X-EXCH50",
            Comment = "Provides the ability to propagate message properties during server-to-server communication"
        )]
        X_EXCH_50,

        [SmtpCommand(
            Command = "X-LINK2STATE",
            Comment = "Adds support for link state routing in Exchange."
        )]
        X_LINK_2_STATE
    }
}
