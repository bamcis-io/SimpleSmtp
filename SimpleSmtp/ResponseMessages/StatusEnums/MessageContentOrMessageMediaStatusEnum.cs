using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum MessageContentOrMessageMediaStatusEnum
    {
        [Description("Something about the content of a message caused it to be considered undeliverable " +
            "and the problem cannot be well expressed with any of the other provided detail codes."
        )]
        OTHER = 0,

        [Description("The media of the message is not supported by either the delivery protocol or the next " +
            "system in the forwarding path. This is useful only as a permanent error."
        )]
        MEDIA_NOT_SUPPORTED = 1,

        [Description("The content of the message must be converted before it can be delivered and such conversion is not permitted. " +
            "Such prohibitions may be the expression of the sender in the message itself or the policy of the sending host."
        )]
        CONVERSION_REQUIRED_AND_PROHIBITED = 2,

        [Description("The message content must be converted in order to be forwarded but such conversion is not possible or is not " +
            "practical by a host in the forwarding path.This condition may result when an ESMTP gateway supports 8bit transport but is not able to " +
            "downgrade the message to 7 bit as required for the next hop."
        )]
        CONVERSION_REQUIRED_BUT_NOT_SUPPORTED = 3,

        [Description("This is a warning sent to the sender when message delivery was successfully but when the delivery required " +
            "a conversion in which some data was lost.This may also be a permanent error if the sender has indicated that conversion " +
            "with loss is prohibited for the message."
        )]
        CONVERSION_WITH_LOSS_PERFORMED = 4,

        [Description("A conversion was required but was unsuccessful. This may be useful as a permanent or persistent temporary notification.")]
        CONVERSION_FAILED = 5,

        [Description("The message content could not be fetched from a remote system. This may be useful as a permanent or persistent temporary notification.")]
        MESSAGE_CONTENT_NOT_AVAILABLE = 6,

        [Description("This indicates the reception of a MAIL or RCPT command that non-ASCII addresses are not permitted.")]
        NON_ASCII_CHARACTERS_NOT_PERMITTED = 7,

        [Description("This indicates that a reply containing a UTF-8 string is required to show the mailbox name, but that form of response is not permitted by the SMTP client.")]
        UTF8_REPLY_REQUIRED_BUT_NOT_PERMITTED = 8,

        [Description("This indicates that transaction failed after the final \".\" of the DATA command.")]
        UTF8_HEADER_MESSAGE_CANNOT_BE_TRANSFERRED = 9
    }
}
