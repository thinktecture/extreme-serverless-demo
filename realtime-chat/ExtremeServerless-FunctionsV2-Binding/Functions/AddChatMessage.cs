using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ExtremeServerless.Functions
{
    public static class AddChatMessage
    {
        [FunctionName("AddChatMessage")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route="save")]
            ChatMessage chatMessage,
            [CosmosDB("chatsystem", "messages", ConnectionStringSetting = "CosmosDB")]
            out dynamic document,
            ILogger log)
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
