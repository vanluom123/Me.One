namespace Demo.Core.CQRS.Result
{
    public class GetStudentResult
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string DoB { get; set; }
        public string Gender { get; set; }
        public string Classmate { get; set; }
    }
}