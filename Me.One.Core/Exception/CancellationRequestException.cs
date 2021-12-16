namespace Me.One.Core.Exception
{
    public class CancellationRequestException : System.Exception
    {
        public CancellationRequestException()
        {
        }

        public CancellationRequestException(string message)
            : base(message)
        {
        }

        public CancellationRequestException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}