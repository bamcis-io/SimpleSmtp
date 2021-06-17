using BAMCIS.SimpleSmtp.ResponseMessages;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class ReceiveDataResult
    {
        public ReceiveDataStatus Status { get; private set; }
        public string Message { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != ReceiveDataStatus.SUCCESSFUL;
            }
        }
        protected ReceiveDataResult(ReceiveDataStatus status)
        {
            this.Status = status;
        }

        protected ReceiveDataResult(string message, ReceiveDataStatus status) : this(status)
        {
            this.Message = message;
        }
    }

    public enum ReceiveDataStatus
    {
        SUCCESSFUL = 0,
        CLIENT_DISCONNECTED = 1,
        CONNECTION_LOST = 2,
        ERROR = 3,
        INVALID_CHARACTER = 4,
        MESSAGE_TOO_LARGE = 5,
        MISSING_ARG = 6
    }

    public class ReceiveDataClientDisconnectedResult : ReceiveDataResult
    {
        public ReceiveDataClientDisconnectedResult() :
            base(SmtpResponseMessage._421_BAD_CONNECTION.ToFormattedString(), ReceiveDataStatus.CLIENT_DISCONNECTED)
        { }
    }

    public class ReceiveDataConnectionLostResult : ReceiveDataResult
    {
        public ReceiveDataConnectionLostResult() :
            base(SmtpResponseMessage._442_CONNECTION_DROPPED.ToFormattedString(), ReceiveDataStatus.CONNECTION_LOST)
        { }
    }

    public class ReceiveDataErrorResult : ReceiveDataResult
    {
        public ReceiveDataErrorResult() :
            base(SmtpResponseMessage._421_BAD_CONNECTION.ToFormattedString(), ReceiveDataStatus.ERROR)
        { }
    }

    public class ReceiveDataInvalidCharacterResult : ReceiveDataResult
    {
        public ReceiveDataInvalidCharacterResult() :
            base(SmtpResponseMessage._500_INVALID_CHARACTER.ToFormattedString(), ReceiveDataStatus.INVALID_CHARACTER)
        { }
    }

    public class ReceiveDataMessageTooLargeResult : ReceiveDataResult
    {
        public ReceiveDataMessageTooLargeResult() :
            base(SmtpResponseMessage._523_MESSAGE_TOO_LARGE.ToFormattedString(), ReceiveDataStatus.MESSAGE_TOO_LARGE)
        { }
    }

    public class ReceiveDataMissingArgResult : ReceiveDataResult
    {
        public ReceiveDataMissingArgResult() :
            base(SmtpResponseMessage._501_MISSING_ARG.ToFormattedString(), ReceiveDataStatus.MISSING_ARG)
        { }
    }

    public class ReceiveDataSuccessResult : ReceiveDataResult
    {
        public ReceiveDataSuccessResult(string message) :
            base(message, ReceiveDataStatus.SUCCESSFUL)
        { }
    }
}
