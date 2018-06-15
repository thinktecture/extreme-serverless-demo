using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ExtremeServerless.Functions
{
    public static class AddChatMessage
    {
        [FunctionName("AddChatMessage")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)]
            ChatMessage chatMessage,
            [DocumentDB("chatsystem", "messages", Id = "id", ConnectionStringSetting = "CosmosDB")]
            out dynamic document,
            TraceWriter log)
        {
            document = new
            {
                id = Guid.NewGuid(),
                user = chatMessage.user,
                message = chatMessage.message
            };

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
