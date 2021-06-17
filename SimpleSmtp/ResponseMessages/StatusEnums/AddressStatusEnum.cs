using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum AddressStatusEnum
    {
        [Description("Something about the address specified in the message caused this DSN.")]
        OTHER_ADDRESS_STATUS = 0,

        [Description("The mailbox specified in the address does not exist. " +
            "For Internet mail names, this means the address portion to the left of the \"@\" sign is invalid. " +
            "This code is only useful for permanent failures."
        )]
        BAD_DESTINATION_MAILBOX_ADDRESS = 1,

        [Description("The destination system specified in the address does not exist or is incapable of accepting mail. " +
            "For Internet mail names, this means the address portion to the right of the \"@\" is invalid for mail. " +
            "This code is only useful for permanent failures."
        )]
        BAD_DESTINATION_SYSTEM_ADDRESS = 2,

        [Description("The destination address was syntactically invalid. " +
            "This can apply to any field in the address.This code is only useful for permanent failures."
        )]
        BAD_DESTINATION_MAILBOX_ADDRESS_SYNTAX = 3,

        [Description("The mailbox address as specified matches one or more recipients on the destination system. " +
            "This may result if a heuristic address mapping algorithm is used to map the specified address to a local mailbox name."
        )]
        DESTINATION_MAILBOX_ADDRESS_AMBIGUOUS = 4,

        [Description("This mailbox address as specified was valid. This status code should be used for positive delivery reports.")]
        DESTINATION_ADDRESS_VALID = 5,

        [Description("The mailbox address provided was at one time valid, but mail is no longer being accepted for that address. " +
            "This code is only useful for permanent failures."
        )]
        DESTINATION_MAILBOX_MOVED_NO_FORWARDING_ADDRESS = 6,

        [Description("The sender's address was syntactically invalid. This can apply to any field in the address.")]
        BAD_SENDER_MAILBOX_ADDRESS_SYNTAX = 7,

        [Description("The sender's system specified in the address does not exist or is incapable of accepting return mail. " +
            "For domain names, this means the address portion to the right of the \"@\" is invalid for mail."
        )]
        BAD_SENDER_SYSTEM_ADDRESS = 8,

        [Description("The mailbox address specified was valid, but the message has been relayed to a system that does not speak this protocol; " +
            "no further information can be provided."
        )]
        MESSAGE_RELAYED_TO_NON_COMPLIANT_MAILER = 9,

        [Description("This status code is returned when the associated address is marked as invalid using a null MX.")]
        RECIPIENT_ADDRESS_HAS_NULL_MX = 10
    }
}
