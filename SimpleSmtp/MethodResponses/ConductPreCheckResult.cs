using BAMCIS.SimpleSmtp.ResponseMessages;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class ConductPreCheckResult
    {
        public ConductPreCheckStatus Status { get; private set; }
        public string Message { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != ConductPreCheckStatus.SUCCESSFUL;
            }
        }
        protected ConductPreCheckResult(ConductPreCheckStatus status)
        {
            this.Status = status;
        }

        protected ConductPreCheckResult(string message, ConductPreCheckStatus status) : this(status)
        {
            this.Message = message;
        }
    }

    public enum ConductPreCheckStatus
    {
        SUCCESSFUL = 0,
        DNS_BLACKLIST = 1,
        IP_BLACKLIST = 2,
        REVERSE_DNS_FAILED = 3,
        EARLY_TALKER = 4,
        NO_MX_RECORD = 5
    }

    public class ConductPreCheckDnsBlacklistResult : ConductPreCheckResult
    {
        public ConductPreCheckDnsBlacklistResult() :
            base(SmtpResponseMessage._554_DNS_OR_IP_BLACKLIST.ToFormattedString(), ConductPreCheckStatus.DNS_BLACKLIST)
        { }
    }

    public class ConductPrecheckIPBlacklistResult : ConductPreCheckResult
    {
        public ConductPrecheckIPBlacklistResult() :
            base (SmtpResponseMessage._554_DNS_OR_IP_BLACKLIST.ToFormattedString(), ConductPreCheckStatus.IP_BLACKLIST)
        { }
    }

    public class ConductPreCheckReverseDnsFailedResult : ConductPreCheckResult
    {
        public ConductPreCheckReverseDnsFailedResult(string domain) :
            base (SmtpResponseMessage._550_CANNOT_FIND_SENDER_DOMAIN.ToFormattedString(domain), ConductPreCheckStatus.REVERSE_DNS_FAILED)
        { }
    }

    public class ConductPreCheckEarlyTalkerResult : ConductPreCheckResult
    {
        public ConductPreCheckEarlyTalkerResult() :
            base (SmtpResponseMessage._554_EARLY_TALKER.ToFormattedString(), ConductPreCheckStatus.EARLY_TALKER)
        { }
    }

    public class ConductPreCheckNoMxRecordResult : ConductPreCheckResult
    {
        public ConductPreCheckNoMxRecordResult() :
            base (SmtpResponseMessage._550_CANNOT_FIND_SENDER_DOMAIN.ToFormattedString(), ConductPreCheckStatus.NO_MX_RECORD)
        { }
    }

    public class ConductPreCheckSuccessResult : ConductPreCheckResult
    {
        public ConductPreCheckSuccessResult() :
            base (string.Empty, ConductPreCheckStatus.SUCCESSFUL)
        { }
    }
}
