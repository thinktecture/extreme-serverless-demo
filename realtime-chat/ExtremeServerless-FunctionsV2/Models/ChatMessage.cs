using Newtonsoft.Json;

namespace Serverless
{
    public class ChatMessage
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
