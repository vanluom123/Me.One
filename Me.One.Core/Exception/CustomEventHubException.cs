namespace Me.One.Core.Exception
{
    public class CustomEventHubException
    {
        public string Data { get; set; }

        public System.Exception Exception { get; set; }
    }
}