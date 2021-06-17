using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum MailSystemStatusEnum
    {
        [Description("The destination system exists and normally accepts mail, but something about the system has caused the generation of this DSN.")]
        OTHER = 0,

        [Description(" Mail system storage has been exceeded. " +
            "The general semantics imply that the individual recipient may not be able to deletematerial to make room for additional messages. " +
            "This is useful only as a persistent transient error."
        )]
        MAIL_SYSTEM_FULL = 1,

        [Description("The host on which the mailbox is resident is not accepting messages. " +
            "Examples of such conditions include an immanent shutdown, excessive load, or system maintenance. " +
            "This is useful for both permanent and persistent transient errors."
        )]
        SYSTEM_NOT_ACCEPTING_MESSAGES = 2,

        [Description("Selected features specified for the message are not supported by the destination system. " +
            "This can occur in gateways when features from one domain cannot be mapped onto the supported feature in another."
        )]
        SYSTEM_NOT_CAPABLE_OF_SELECTED_FEATURES = 3,

        [Description("The message is larger than per-message size limit. This limit may either be for physical or administrative reasons. " +
            "This is useful only as a permanent error."
        )]
        MESSAGE_TOO_BIG_FOR_SYSTEM = 4,

        [Description("The system is not configured in a manner that will permit it to accept this message.")]
        SYSTEM_INCORRECTLY_CONFIGURED = 5,

        [Description("The message was accepted for relay/delivery, but the requested priority (possibly the implied default) " +
            "was not honoured. The human readable text after the status code contains the new priority, followed by SP (space) and explanatory human readable text.")]
        REQUESTED_PRIORITY_HAS_CHANGED = 6
    }
}
