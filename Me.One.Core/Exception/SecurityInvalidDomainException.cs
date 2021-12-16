namespace Me.One.Core.Exception
{
    public class SecurityInvalidDomainException : System.Exception
    {
        public SecurityInvalidDomainException()
        {
        }

        public SecurityInvalidDomainException(string message)
            : base(message)
        {
        }
    }
}