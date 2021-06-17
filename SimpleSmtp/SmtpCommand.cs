using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.SimpleSmtp
{
    public class SmtpCommand
    {
        public string Command { get; private set; }
        public string Comment { get; private set; }
        public int Code { get; private set; }

        public static readonly SmtpCommand ATRN = new SmtpCommand()
        {
            Command = "ATRN",
            Comment = "Authenticated TURN. Optionally takes one or more domains as a parameter. " +
                    "The ATRN command must be rejected if the session has not been authenticated.",
            Code = 10
        };

        public static readonly SmtpCommand AUTH = new SmtpCommand()
        {
            Command = "AUTH",
            Comment = "Authentication",
            Code = 20,
        };

        public static readonly SmtpCommand BDAT = new SmtpCommand()
        {
            Command = "BDAT",
            Comment = "Binary data",
            Code = 30,
        };

        public static readonly SmtpCommand BURL = new SmtpCommand()
        {
            Command = "BURL",
            Comment = "Remote content"
        };

        public static readonly SmtpCommand CHUNKING = new SmtpCommand()
        {
            Command = "CHUNKING",
            Comment = "An ESMTP command that replaces the DATA command. " +
                    "So that the SMTP host does not have to continuously scan for the end of the data, " +
                    "this command sends a BDAT command with an argument that contains the total number of bytes in a message. " +
                    "The receiving server counts the bytes in the message and, when the message size equals the value sent by the BDAT command, " +
                    "the server assumes it has received all of the message data"
        };

        public static readonly SmtpCommand DATA = new SmtpCommand()
        {
            Command = "DATA",
            Comment = "The actual email message to be sent"
        };

        public static readonly SmtpCommand DSN = new SmtpCommand()
        {
            Command = "DSN",
            Comment = "An ESMTP command that enables delivery status notifications"
        };

        public static readonly SmtpCommand EHLO = new SmtpCommand()
        {
            Command = "EHLO",
            Comment = "Extended HELO"
        };

        public static readonly SmtpCommand ETRN = new SmtpCommand()
        {
            Command = "ETRN",
            Comment = "Extended turn. An extension of SMTP. " +
                    "ETRN is sent by an SMTP server to request that another server send any e-mail messages that it has."
        };

        public static readonly SmtpCommand EXPN = new SmtpCommand()
        {
            Command = "EXPN",
            Comment = "Expand"
        };

        public static readonly SmtpCommand HELO = new SmtpCommand()
        {
            Command = "HELO",
            Comment = "Identify yourself to the SMTP server"
        };

        public static readonly SmtpCommand HELP = new SmtpCommand()
        {
            Command = "HELP",
            Comment = "Show available commands"
        };

        public static readonly SmtpCommand INVALID = new SmtpCommand()
        {
            Command = "\r\n",
            Comment = "Invalid command"
        };

        public static readonly SmtpCommand MAIL_FROM = new SmtpCommand()
        {
            Command = "MAIL FROM:",
            Comment = "Send mail from email account"
        };

        public static readonly SmtpCommand NOOP = new SmtpCommand()
        {
            Command = "NOOP",
            Comment = "No-op. Keeps the connection open"
        };

        public static readonly SmtpCommand ONEX = new SmtpCommand()
        {
            Command = "ONEX",
            Comment = "One message transaction only"
        };

        public static readonly SmtpCommand PIPELINING = new SmtpCommand()
        {
            Command = "PIPELINING",
            Comment = "Provides the ability to send a stream of commands without waiting for a response after each command"
        };

        public static readonly SmtpCommand QUIT = new SmtpCommand()
        {
            Command = "QUIT",
            Comment = "End session"
        };

        public static readonly SmtpCommand RCPT_TO = new SmtpCommand()
        {
            Command = "RCPT TO:",
            Comment = "Send email to recipient"
        };

        public static readonly SmtpCommand RSET = new SmtpCommand()
        {
            Command = "RSET",
            Comment = "Reset"
        };

        public static readonly SmtpCommand SAML = new SmtpCommand()
        {
            Command = "SAML",
            Comment = "Send and mail"
        };

        public static readonly SmtpCommand SEND = new SmtpCommand()
        {
            Command = "SEND",
            Comment = "Send"
        };

        public static readonly SmtpCommand SIZE = new SmtpCommand()
        {
            Command = "SIZE",
            Comment = "Provides a mechanism by which the SMTP server can indicate the maximum size message supported. " +
                    "Compliant servers must provide size extensions to indicate the maximum size message that can be accepted. " +
                    "Clients should not send messages that are larger than the size indicated by the server."
        };

        public static readonly SmtpCommand SOML = new SmtpCommand()
        {
            Command = "SOML",
            Comment = "Send or mail"
        };

        public static readonly SmtpCommand STARTTLS = new SmtpCommand()
        {
            Command = "STARTTLS",
            Comment = "Starts TLS session"
        };

        public static readonly SmtpCommand SUBMITTER = new SmtpCommand()
        {
            Command = "SUBMITTER",
            Comment = "SMTP responsible submitter"
        };

        public static readonly SmtpCommand TURN = new SmtpCommand()
        {
            Command = "TURN",
            Comment = "Allows the client and server to switch roles and send mail in the reverse direction " +
                    "without having to establish a new connection"
        };

        public static readonly SmtpCommand VERB = new SmtpCommand()
        {
            Command = "VERB",
            Comment = "Verbose"
        };

        public static readonly SmtpCommand VRFY = new SmtpCommand()
        {
            Command = "VRFY",
            Comment = "Verify"
        };

        public static readonly SmtpCommand X_EXPS_GSSAPI = new SmtpCommand()
        {
            Command = "X-EXPS GSSAPI",
            Comment = "A method that is used by Microsoft Exchange Server 2003 and Exchange 2000 Server servers to authenticate"
        };

        public static readonly SmtpCommand X_EXPS_LOGIN = new SmtpCommand()
        {
            Command = "X-EXPS=LOGIN",
            Comment = "A method that is used by Exchange 2000 and Exchange 2003 servers to authenticate."
        };

        public static readonly SmtpCommand X_EXCH_50 = new SmtpCommand()
        {
            Command = "X-EXCH50",
            Comment = "Provides the ability to propagate message properties during server-to-server communication"
        };

        public static readonly SmtpCommand X_LINK_2_STATE = new SmtpCommand()
        {
            Command = "X-LINK2STATE",
            Comment = "Adds support for link state routing in Exchange."
        };

        public override string ToString()
        {
            return this.Command;
        }
    }
}
