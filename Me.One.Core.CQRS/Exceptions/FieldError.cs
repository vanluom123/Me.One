namespace Me.One.Core.CQRS.Exceptions
{
    public class FieldError
    {
        public string PropertyName { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}