namespace green_craze_be_v1.Application.Common.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException()
        {
        }

        public UnAuthorizedException(string message)
            : base(message)
        {
        }

        public UnAuthorizedException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}