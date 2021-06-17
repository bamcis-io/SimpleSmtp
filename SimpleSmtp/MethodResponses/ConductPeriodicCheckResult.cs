using BAMCIS.SimpleSmtp.ResponseMessages;
using System;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class ConductPeriodicCheckResult
    {
        public ConductPeriodicCheckStatus Status { get; private set; }
        public string Message { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != ConductPeriodicCheckStatus.SUCCESSFUL;
            }
        }
        protected ConductPeriodicCheckResult(ConductPeriodicCheckStatus status)
        {
            this.Status = status;
        }

        protected ConductPeriodicCheckResult(string message, ConductPeriodicCheckStatus status) : this(status)
        {
            this.Message = message;
        }
    }

    public enum ConductPeriodicCheckStatus
    {
        SUCCESSFUL = 0,
        EXCEEDED_MESSAGING_LIMIT = 1,
        MAX_ERRORS = 2,
        EXCEEDED_VERIFY_LIMIT = 3,
        EXCEEDED_NOOP_LIMIT = 4,
        EXCEEDED_RECIPIENT_LIMIT = 5,
    }

    public class ConductPeriodicCheckSuccessResult : ConductPeriodicCheckResult
    {
        public ConductPeriodicCheckSuccessResult() :
            base(String.Empty, ConductPeriodicCheckStatus.SUCCESSFUL)
        { }
    }
    public class ConductPostCheckExceededMessageLimitResult : ConductPeriodicCheckResult
    {
        public ConductPostCheckExceededMessageLimitResult() :
            base(SmtpResponseMessage._451_EXCEEDED_MESSAGING_LIMIT.ToFormattedString(), ConductPeriodicCheckStatus.EXCEEDED_MESSAGING_LIMIT)
        { }
    }

    public class ConductPostCheckMaxErrorsResult : ConductPeriodicCheckResult
    {
        public ConductPostCheckMaxErrorsResult() :
            base(SmtpResponseMessage._421_MAX_ERRORS_REACHED.ToFormattedString(), ConductPeriodicCheckStatus.MAX_ERRORS)
        { }
    }

    public class ConductPostCheckExceededVerifyLimitResult : ConductPeriodicCheckResult
    {
        public ConductPostCheckExceededVerifyLimitResult() :
            base(SmtpResponseMessage._451_EXCEEDED_VERIFY_LIMIT.ToFormattedString(), ConductPeriodicCheckStatus.EXCEEDED_VERIFY_LIMIT)
        { }
    }

    public class ConductPostCheckExceededNoopLimitResult : ConductPeriodicCheckResult
    {
        public ConductPostCheckExceededNoopLimitResult() :
            base(SmtpResponseMessage._451_EXCEEDED_NOOP_LIMIT.ToFormattedString(), ConductPeriodicCheckStatus.EXCEEDED_NOOP_LIMIT)
        { }
    }

    public class ConductPostCheckExceededRecipientLimitResult : ConductPeriodicCheckResult
    {
        public ConductPostCheckExceededRecipientLimitResult() :
            base(SmtpResponseMessage._451_TOO_MANY_RECIPIENTS.ToFormattedString(), ConductPeriodicCheckStatus.EXCEEDED_RECIPIENT_LIMIT)
        { }
    }

}
