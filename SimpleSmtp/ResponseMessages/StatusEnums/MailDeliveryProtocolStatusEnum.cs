using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum MailDeliveryProtocolStatusEnum
    {
        [Description("Something was wrong with the protocol necessary to deliver the message to the next hop " +
            "and the problem cannot be well expressed with any of the other provided detail codes."
        )]
        OTHER = 0,

        [Description("A mail transaction protocol command was issued which was either out of sequence or unsupported. " +
            "This is useful only as a permanent error."
        )]
        INVALID_COMMAND = 1,

        [Description("A mail transaction protocol command was issued which could not be interpreted, " +
            "either because the syntax was wrong or the command is unrecognized.This is useful only as a permanent error."
        )]
        SYNTAX_ERROR = 2,

        [Description("More recipients were specified for the message than could have been delivered by the protocol. " +
            "This error should normally result in the segmentation of the message into two, the remainder of the recipients " +
            "to be delivered on a subsequent delivery attempt. It is included in this list in the event that such segmentation is not possible."
        )]
        TOO_MANY_RECIPIENTS = 3,

        [Description("A valid mail transaction protocol command was issued with invalid arguments, " +
            "either because the arguments were out of range or represented unrecognized features. " +
            "This is useful only as a permanent error.")]
        INVALID_COMMAND_ARGUMENTS = 4,

        [Description("A protocol version mis-match existed which could not be automatically resolved by the communicating parties.")]
        WRONG_PROTOCOL_VERSION = 5,

        [Description("This enhanced status code SHOULD be returned when the server fails the AUTH command due to the client sending a " +
            "[BASE64] response which is longer than the maximum buffer size available for the currently selected SASL mechanism. " +
            "This is useful for both permanent and persistent transient errors."
        )]
        AUTHENTICATION_EXCHANGE_LINE_TOO_LONG = 6
    }
}
