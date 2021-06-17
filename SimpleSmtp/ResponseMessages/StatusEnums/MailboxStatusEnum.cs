using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum MailboxStatusEnum
    {
        [Description("The mailbox exists, but something about the destination mailbox has caused the sending of this DSN.")]
        OTHER = 0,

        [Description("The mailbox exists, but is not accepting messages. " +
            "This may bea permanent error if the mailbox will never be re-enabled or a transient error " +
            "if the mailbox is only temporarily disabled."
        )]
        MAILBOX_DISABLED = 1,

        [Description("The mailbox is full because the user has exceeded a per-mailbox administrative quota or physical capacity. " +
            "The general semantics implies that the recipient can delete messages to make more space available. " +
            "This code should be used as a persistent transient failure."
        )]
        MAILBOX_FULL = 2,

        [Description("A per-mailbox administrative message length limit has been exceeded. " +
            "This status code should be used when the per-mailbox message length limit is less than the general system limit. " +
            "This code should be used as a permanent failure."
        )]
        MESSAGE_LENGTH_EXCEEDS_LIMIT = 3,

        [Description("The mailbox is a mailing list address and the mailing list was unable to be expanded. " +
            "This code may represent a permanent failure or a persistent transient failure."
        )]
        MAILING_LIST_EXPANSION_PROBLEM = 4
    }
}
