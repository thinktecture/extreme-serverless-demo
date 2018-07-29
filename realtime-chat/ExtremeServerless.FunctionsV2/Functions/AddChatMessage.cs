using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ExtremeServerless.Functions
{
    public static class AddChatMessage
    {
        [FunctionName("AddChatMessage")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST")]
            ChatMessage chatMessage,
            [CosmosDB("chatsystem", "messages", Id = "id", ConnectionStringSetting = "CosmosDB")]
            out dynamic document,
            TraceWriter log)
        {
            document = new
            {
                id = Guid.NewGuid(),
                user = chatMessage.User,
                message = chatMessage.Message
            };

            return new OkResult();
        }
    }
}
