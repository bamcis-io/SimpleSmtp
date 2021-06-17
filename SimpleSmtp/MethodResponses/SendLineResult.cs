using BAMCIS.SimpleSmtp.ResponseMessages;

namespace BAMCIS.SimpleSmtp.MethodResponses
{
    public abstract class SendLineResult
    {
        public SendLineStatus Status { get; private set; }
        public bool IsError
        {
            get
            {
                return this.Status != SendLineStatus.SUCCESSFUL;
            }
        }

        protected SendLineResult(SendLineStatus status)
        {
            this.Status = status;
        }
    }

    public enum SendLineStatus
    {
        SUCCESSFUL = 0,
        CLIENT_DISCONNECTED = 1,
        ERROR = 2
    }

    public abstract class SendLineError : SendLineResult
    {
        public string ErrorMessage { get; protected set; }
        

        public SendLineError(string errorMessage, SendLineStatus status) : base(status)
        {
            this.ErrorMessage = errorMessage;
        }
    }

    public class SendLineClientDisconnectedResult : SendLineError
    {
        public SendLineClientDisconnectedResult() :
            base(SmtpResponseMessage._421_BAD_CONNECTION.ToFormattedString(), SendLineStatus.CLIENT_DISCONNECTED)
        { }
    }

    public class SendLineErrorResult : SendLineError
    {
        public SendLineErrorResult() :
            base(SmtpResponseMessage._442_CONNECTION_DROPPED.ToFormattedString(), SendLineStatus.ERROR)
        { }
    }

    public class SendLineSuccessResult : SendLineResult
    {
        public SendLineSuccessResult() :
            base(SendLineStatus.SUCCESSFUL)
        { }
    }
}
