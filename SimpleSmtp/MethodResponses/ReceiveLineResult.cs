using BAMCIS.SimpleSmtp.ResponseMessages;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class ReceiveLineResult
    {
        public ReceiveLineStatus Status { get; private set; }
        public string Message { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != ReceiveLineStatus.SUCCESSFUL;
            }
        }

        protected ReceiveLineResult(ReceiveLineStatus status)
        {
            this.Status = status;
        }

        protected ReceiveLineResult(string message, ReceiveLineStatus status) : this(status)
        {
            this.Message = message;
        }
    }

    public enum ReceiveLineStatus
    {
        SUCCESSFUL = 0,
        CLIENT_DISCONNECTED = 1,
        CONNECTION_LOST = 2,
        ERROR = 3,
        INVALID_CHARACTER = 4
    }

    public class ReceiveLineClientDisconnectedResult : ReceiveLineResult
    {
        public ReceiveLineClientDisconnectedResult() :
            base(SmtpResponseMessage._421_BAD_CONNECTION.ToFormattedString(), ReceiveLineStatus.CLIENT_DISCONNECTED)
        { }
    }

    public class ReceiveLineConnectionLostResult : ReceiveLineResult
    {
        public ReceiveLineConnectionLostResult() :
            base (SmtpResponseMessage._442_CONNECTION_DROPPED.ToFormattedString(), ReceiveLineStatus.CONNECTION_LOST)
        { }
    }

    public class ReceiveLineErrorResult : ReceiveLineResult
    {
        public ReceiveLineErrorResult() :
            base (SmtpResponseMessage._421_BAD_CONNECTION.ToFormattedString(), ReceiveLineStatus.ERROR)
        { }
    }

    public class ReceiveLineInvalidCharacterResult : ReceiveLineResult
    {
        public ReceiveLineInvalidCharacterResult() :
            base (SmtpResponseMessage._500_INVALID_CHARACTER.ToFormattedString(), ReceiveLineStatus.INVALID_CHARACTER)
        { }
    }

    public class ReceiveLineSuccessResult : ReceiveLineResult
    {
        public ReceiveLineSuccessResult(string message) :
            base(message, ReceiveLineStatus.SUCCESSFUL)
        { }
    }
}
