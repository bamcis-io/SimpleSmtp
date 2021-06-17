using BAMCIS.SimpleSmtp.ResponseMessages;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class ConductPostCheckResult
    {
        public ConductPostCheckStatus Status { get; private set; }
        public string Message { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != ConductPostCheckStatus.SUCCESSFUL;
            }
        }
        protected ConductPostCheckResult(ConductPostCheckStatus status)
        {
            this.Status = status;
        }

        protected ConductPostCheckResult(string message, ConductPostCheckStatus status) : this(status)
        {
            this.Message = message;
        }
    }

    public enum ConductPostCheckStatus
    {
        SUCCESSFUL = 0,      
        HEADER_MISMATCH = 6,
        VIRUS = 7
    }

    public class ConductPostCheckSuccessResult : ConductPostCheckResult
    {
        public ConductPostCheckSuccessResult() :
            base(SmtpResponseMessage._250_ACCEPTED.ToFormattedString(), ConductPostCheckStatus.SUCCESSFUL)
        { }
    }
   

    public class ConductPostCheckHeaderMismatchResult : ConductPostCheckResult
    {
        public ConductPostCheckHeaderMismatchResult() :
            base(SmtpResponseMessage._501_BAD_MAILBOX_ADDRESS_ARG.ToFormattedString(), ConductPostCheckStatus.HEADER_MISMATCH)
        { }
    }

    public class ConductPostCheckVirusResult : ConductPostCheckResult
    {
        public ConductPostCheckVirusResult() :
            base (SmtpResponseMessage._554_VIRUS.ToFormattedString(), ConductPostCheckStatus.VIRUS)
        { }
    }
}
