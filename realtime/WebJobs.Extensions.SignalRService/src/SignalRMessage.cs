namespace Microsoft.Azure.WebJobs.Extensions.SignalRService
{
    public class SignalRMessage
    {
        public SignalRMessage(string target, object[] arguments)
        {
            Target = target;
            Arguments = arguments;
        }

        public string Target { get; set; }
        public object[] Arguments { get; set; }
    }
}
