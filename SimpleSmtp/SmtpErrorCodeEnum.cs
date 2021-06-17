namespace BAMCIS.SimpleSmtp
{
    public enum SmtpErrorCodeEnum
    {
        _101_UNABLE_TO_CONNECT = 101,           //The server is unable to connect
        _111_CONNECTION_REFUSED = 111,          //Connection refused or inability to open an SMTP stream

        _211_SYSTEM_STATUS = 211,               //System status message or help reply
        _214_HELP_MESSAGE = 214,                //Response to the HELP command
        _220_SERVER_READY = 220,                //The server is ready
        _221_CLOSING_CONNECTION = 221,          //Server is closing its transmission channel
        _235_AUTH_SUCCESSFUL = 235,             //Authentication successful
        _250_MAIL_ACTION_COMPLETED = 250,       //Server has transmitted the message
        _251_WILL_FORWARD = 251,                //User is not local, the server will forward the email
        _252_USER_NOT_VERIFIED = 252,           //The server cannot verfy the user

        _354_DATA_CMD_RESPONSE = 354,           //The server has received the From and To and is ready for the body

        _421_SERVER_UNAVAILABLE = 421,          //The server is not available due to a connection problem
        _422_EXCEEDED_STORAGE_LIMIT = 422,      //The recipient's mailbox has exceeded its storage limit
        _431_OUT_OF_MEMORY = 431,               //Not enough space on disk or out of memory condition
        _442_CONNECTION_DROPPED = 442,          //The connection was dropped during the transmission
        _446_MAX_HOP_COUNT = 446,               //The maximum hop count was exceeded
        _447_OUTGOING_TIMEOUT = 447,            //Outgoing message timed out, possible exceeded server's recipient limit
        _449_ROUTING_ERROR = 449,               //MS Exchange routing error
        _450_USER_MAILBOX_UNAVAILABLE = 450,    //User's mailbox hs been corrupted or placed offline
        _451_ACTION_ABORTED = 451,              //Message rejected
        _452_STORAGE_LIMIT_EXCEEDED = 452,      //Too many emails sent or too many recipients

        _500_SYNTAX_ERROR_COMMAND = 500,        //Syntax error, server couldn't recognize the command
        _501_SYNTAX_ERROR_ARG = 501,            //Syntax error, argument invalid, likely bad email address
        _502_COMMAND_NOT_IMPLEMENTED = 502,     //Command is not implemented
        _503_BAD_COMMAND_SEQUENCE = 503,        //Server encountered a bad sequence of commands
        _504_ARG_NOT_IMPLEMENTED = 504,         //Command parameter is not implemented
        _510_BAD_RECIPIENT_ADDRESS = 510,       //Bad email address
        _512_DNS_NOT_FOUND = 512,               //The host server for recipient cannot be found
        _513_ADDRESS_TYPE_INCORRECT = 513,      //The recipient address is incorrect
        _523_SIZE_EXCEEDED = 523,               //The total size of the mailing exceeds recipient server's limits
        _530_ACCESS_DENIED = 530,               //Authentication problem
        _535_AUTH_UNSUCCESSFUL = 535,           //Authentication unsuccessful
        _541_MESSAGE_REJECTED = 541,            //Recipient rejected the message, probably anti-spam
        _550_RECIPIENT_DOES_NOT_EXIST = 550,    //Non-existent email address
        _551_USER_NOT_LOCAL = 551,              //User not local, please try forward path
        _552_EXCEEDED_STORAGE_ALLOCATION = 552, //Mail action aborted, exceeded user storage allocation
        _553_MAILBOX_NAME_INVALID = 553,        //Mailbox name invalid
        _554_TRANSACTION_FAILED = 554           //Transaction failed, due to spam or blacklist
    }
}
