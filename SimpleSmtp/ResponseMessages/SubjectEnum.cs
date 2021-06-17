using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages
{
    public enum SubjectEnum
    {
        [Description("There is no additional subject information available.")]
        UNDEFINED_STATUS = 0,

        [Description("The address status reports on the originator or destination address. " +
            "It may include address syntax or validity. These errors can generally be corrected by the sender and retried."
        )]
        ADDRESSING_STATUS = 1,

        [Description("Mailbox status indicates that something having to do with the mailbox has caused this DSN. " +
            "Mailbox issues are assumed to be under the general control of the recipient."
        )]
        MAILBOX_STATUS = 2,

        [Description("Mail system status indicates that something having to do with the destination system has caused this DSN. " +
            "System issues are assumed to be under the general control of the destination system administrator."
        )]
        MAIL_SYSTEM_STATUS = 3,

        [Description("The networking or routing codes report status about the delivery system itself.These system components include any " +
            "necessary infrastructure such as directory and routing services. Network issues are assumed to be under the control of the " +
            "destination or intermediate system administrator."
        )]
        NETWORK_AND_ROUTING_STATUS = 4,

        [Description("The mail delivery protocol status codes report failures involving the message delivery protocol. " +
            "These failures include the full range of problems resulting from implementation errors or an unreliable connection."
        )]
        MAIL_DELIVERY_PROTOCOL_STATUS = 5,

        [Description("The message content or media status codes report failures involving the content of the message. " +
            "These codes report failures due to translation, transcoding, or otherwise unsupported message media. " +
            "Message content or media issues are under the control of both the sender and the receiver, " +
            "both of which must support a common set of supported content-types."
        )]
        MESSAGE_CONTENT_OR_MEDIA_STATUS = 6,

        [Description("The security or policy status codes report failures involving policies such as per-recipient or per-host filtering and " +
            "cryptographic operations.Security and policy status issues are assumed to be under the control of either or both the sender and " +
            "recipient. Both the sender and recipient must permit the exchange of messages and arrange the exchange of necessary keys and " +
            "certificates for cryptographic operations."
        )]
        SECURITY_OR_POLICY_STATUS = 7
    }
}
