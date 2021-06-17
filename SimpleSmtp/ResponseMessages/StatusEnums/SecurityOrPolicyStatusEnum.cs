using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages.StatusEnums
{
    public enum SecurityOrPolicyStatusEnum
    {
        [Description("Something related to security caused the message to be returned, " +
            "and the problem cannot be well expressed with any of the other provided detail codes. " +
            "This status code may also be used when the condition cannot be further described because of " +
            "security policies in force."
        )]
        OTHER = 0,

        [Description("The sender is not authorized to send to the destination. This can be the result of per-host or per-recipient filtering. " +
            "This memo does not discuss the merits of any such filtering, but provides a mechanism to report such. " +
            "This is useful only as a permanent error."
        )]
        DELIVERY_NOT_AUTHORIZED_MESSAGE_REFUSED = 1,

        [Description("The sender is not authorized to send a message to the intended mailing list. This is useful only as a permanent error.")]
        MAILING_LIST_EXPANSION_PROHIBITED = 2,

        [Description("A conversion from one secure messaging protocol to another was required for delivery and such conversion was not possible. " +
            "This is useful only as a permanent error."
        )]
        SECURITY_CONVERSION_REQUIRED_BUT_NOT_POSSIBLE = 3,

        [Description("A message contained security features such as secure authentication that could not be supported on the delivery protocol. " +
            "This is useful only as a permanent error.")]
        SECURITY_FEATURES_NOT_SUPPORTED = 4,

        [Description("A transport system otherwise authorized to validate or decrypt a message in transport was unable to do so because necessary " +
            "information such as key was not available or such information was invalid."
        )]
        CRYPTOGRAPHIC_FAILURE = 5,

        [Description("A transport system otherwise authorized to validate or decrypt a message was unable to do so " +
            "because the necessary algorithm was not supported."
        )]
        CRYPTOGRAPHIC_ALGORITHM_NOT_SUPPORTED = 6,

        [Description("A transport system otherwise authorized to validate a message was unable to do so because the message was corrupted or altered. " +
            "This may be useful as a permanent, transient persistent, or successful delivery code."
        )]
        MESSAGE_INTEGRITY_FAILURE = 7,

        [Description("This response to the AUTH command indicates that the authentication failed due to invalid or insufficient authentication credentials. " +
            "In this case, the client SHOULD ask the user to supply new credentials (such as by presenting a password dialog box)."
        )]
        AUTH_CREDENTIALS_INVALID = 8,

        [Description("This response to the AUTH command indicates that the selected authentication mechanism is weaker than server policy permits for that user. " +
            "The client SHOULD retry with a new authentication mechanism."
        )]
        AUTH_MECHANISM_TOO_WEAK = 9,

        [Description("This indicates that external strong privacy layer is needed in order to use the requested authentication mechanism. " +
            "This is primarily intended for use with clear text authentication mechanisms. A client which receives this may activate a security layer " +
            "such as TLS prior to authenticating, or attempt to use a stronger mechanism."
        )]
        ENCRYPTION_NEEDED = 10,

        [Description("This response to the AUTH command indicates that the selected authentication mechanism may only be used when the underlying SMTP connection " +
            "is encrypted. Note that this response code is documented here for historical purposes only. Modern implementations SHOULD NOT advertise mechanisms that " +
            "are not permitted due to lack of encryption, unless an encryption layer of sufficient strength is currently being employed."
        )]
        ENCRYPTION_REQUIRED_FOR_REQUESTED_AUTH_MECHANISM = 11,

        [Description("This response to the AUTH command indicates that the user needs to transition to the selected authentication mechanism. " +
            "This is typically done by authenticating once using the [PLAIN] authentication mechanism. " +
            "The selected mechanism SHOULD then work for authentications in subsequent sessions."
        )]
        PASSWORD_TRANSITION_NEEDED = 12,

        [Description("Sometimes a system administrator will have to disable a user's account (e.g., due to lack of payment, abuse, evidence of a break-in attempt, etc). " +
            "This error code occurs after a successful authentication to a disabled account. This informs the client that the failure is permanent until the user contacts " +
            "their system administrator to get the account re-enabled. It differs from a generic authentication failure where the client's best option is to present the " +
            "passphrase entry dialog in case the user simply mistyped their passphrase."
        )]
        USER_ACCOUNT_DISABLED = 13,

        [Description("The submission server requires a configured trust relationship with a third-party server in order to access the message content. " +
            "This value replaces the prior use of X.7.8 for this error condition. thereby updating [RFC4468]."
        )]
        TRUST_RELATIONSHIP_REQUIRED = 14,

        [Description("The specified priority level is below the lowest priority acceptable for the receiving SMTP server. " +
            "This condition might be temporary, for example the server is operating in a mode where only higher priority messages " +
            "are accepted for transfer and delivery, while lower priority messages are rejected."
        )]
        PRIORITY_LEVEL_TOO_LOW = 15,

        [Description("The message is too big for the specified priority. This condition might be temporary, for example the server is " +
            "operating in a mode where only higher priority messages below certain size are accepted for transfer and delivery."
        )]
        MESSAGE_TOO_BIG_FOR_SPECIFIED_PRIORITY = 16,

        [Description("This status code is returned when a message is received with a Require-Recipient-Valid-Since field or RRVS extension " +
            "and the receiving system is able to determine that the intended recipient mailbox has not been under continuous ownership since the specified date-time.")]
        MAILBOX_OWNER_HAS_CHANGED = 17,

        [Description("This status code is returned when a message is received with a Require-Recipient-Valid-Since field or RRVS extension and the receiving " +
            "system wishes to disclose that the owner of the domain name of the recipient has changed since the specified date-time."
        )]
        DOMAIN_OWNER_HAS_CHANGED = 18,

        [Description("This status code is returned when a message is received with a Require-Recipient-Valid-Since field or RRVS extension and the receiving " +
            "system cannot complete the requested evaluation because the required timestamp was not recorded. The message originator needs to decide whether " +
            "to reissue the message without RRVS protection."
        )]
        RRVS_TEST_CANNOT_BE_COMPLETED = 19,

        [Description("This status code is returned when a message did not contain any passing DKIM signatures. (This violates the advice of Section 6.1 of [RFC6376].)")]
        NO_PASSING_DKIM_SIGNATURE_FOUND = 20,

        [Description("This status code is returned when a message contains one or more passing DKIM signatures, but none are acceptable. " +
            "(This violates the advice of Section 6.1 of [RFC6376].)"
        )]
        NO_ACCEPTABLE_DKIM_SIGNATURE_FOUND = 21,

        [Description("This status code is returned when a message contains one or more passing DKIM signatures, but none are acceptable because none have an identifier(s) " +
            "that matches the author address(es) found in the From header field. This is a special case of X.7.21. (This violates the advice of Section 6.1 of [RFC6376].)"
        )]
        NO_VALID_AUTHOR_MATCHED_DKIM_SIGNATURE_FOUND = 22,

        [Description("This status code is returned when a message completed an SPF check that produced a \"fail\" result, contrary to local policy requirements. " +
            "Used in place of 5.7.1 as described in Section 8.4 of [RFC7208]."
        )]
        SPF_VALIDATION_FAILED = 23,

        [Description("This status code is returned when evaluation of SPF relative to an arriving message resulted in an error. " +
            "Used in place of 4.4.3 or 5.5.2 as described in Sections 8.6 and 8.7 of [RFC7208]."
        )]
        SPF_VALIDATION_ERROR = 24,

        [Description("This status code is returned when an SMTP client's IP address failed a reverse DNS validation check, contrary to local policy requirements.")]
        REVERSE_DNS_VALIDATION_FAILED = 25,

        [Description("This status code is returned when a message failed more than one message authentication check, contrary to local policy requirements. " +
            "The particular mechanisms that failed are not specified."
        )]
        MULTIPLE_AUTH_CHECKS_FAILED = 26,

        [Description("This status code is returned when the associated sender address has a null MX, and the SMTP receiver is configured to reject mail from such sender " +
            "(e.g., because it could not return a DSN)."
        )]
        SENDER_ADDRESS_HAS_NULL_MX = 26
    }
}
