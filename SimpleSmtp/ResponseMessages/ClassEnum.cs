using System.ComponentModel;

namespace BAMCIS.SimpleSmtp.ResponseMessages
{
    public enum ClassEnum
    {
        [Description("No code.")]
        NONE = 0,

        [Description("Success specifies that the DSN is reporting a positive delivery action. " +
            "Detail sub-codes may provide notification of transformations required for delivery."
        )]
        SUCCESS = 2,

        [Description("A persistent transient failure is one in which the message as sent is valid, " +
            "but persistence of some temporary condition has caused abandonment or delay of attempts to send the message. " +
            "If this code accompanies a delivery failure report, sending in the future may be successful."
        )]
        PERSISTENT_TRANSIENT_FAILURE = 4,

        [Description(" A permanent failure is one which is not likely to be resolved by resending the message in the current form. " +
            "Some change to the message or the destination must be made for successful delivery."
        )]
        PERMANENT_FAILURE = 5
    }
}
