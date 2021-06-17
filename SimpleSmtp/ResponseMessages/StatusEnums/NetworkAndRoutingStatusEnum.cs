using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum NetworkAndRoutingStatusEnum
    {
        [Description("Something went wrong with the networking, " +
            "but it is not clear what the problem is, or the problem cannot be well expressed with any of the other provided detail codes."
        )]
        OTHER = 0,

        [Description("The outbound connection attempt was not answered, because either the remote system was busy, or was unable to take a call. " +
            "This is useful only as a persistent transient error."
        )]
        NO_ANSWER_FROM_HOST = 1,

        [Description("The outbound connection was established, but was unable to complete the message transaction, " +
            "either because of time-out, or inadequate connection quality. This is useful only as apersistent transient error."
        )]
        BAD_CONNECTION = 2,

        [Description("The network system was unable to forward the message, because a directory server was unavailable. " +
            "This is useful only as a persistent transient error." +
            "\r\n" + 
            "The inability to connect to an Internet DNS server is one example of the directory server failure error."
        )]
        DIRECTORY_SERVER_FAILURE = 3,

        [Description("The mail system was unable to determine the next hop for the message because the necessary routing information was unavailable from the directory server. " +
            "This is useful for both permanent and persistent transient errors." +
            "\r\n" +
            "A DNS lookup returning only an SOA (Start of Administration) record for a domain name is one example of the unable to route error."
        )]
        UNABLE_TO_ROUTE = 4,

        [Description("The mail system was unable to deliver the message because the mail system was congested. This is useful only as a persistent transient error.")]
        MAIL_SYSTEM_CONGESTION = 5,

        [Description("A routing loop caused the message to be forwarded too many times, " +
            "either because of incorrect routing tables or a user-forwarding loop. This is useful only as a persistent transient error."
        )]
        ROUTING_LOOP_DETECTED = 6,

        [Description("The message was considered too old by the rejecting system, either because it remained on that host too long or " +
            "because the time-to-live value specified by the sender of the message was exceeded. " +
            "If possible, the code for the actual problem found when delivery was attempted should be returned rather than this code."
        )]
        DELIVERY_TIME_EXPIRED = 7
    }
}
