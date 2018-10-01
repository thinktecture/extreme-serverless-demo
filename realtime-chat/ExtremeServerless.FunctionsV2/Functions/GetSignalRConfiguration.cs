using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ExtremeServerless.Functions
{
    public static class GetSignalRConfiguration
    {
        private static AzureSignalR signalR = new AzureSignalR(
            Environment.GetEnvironmentVariable("SignalR"));

        [FunctionName("GetSignalRConfiguration")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="config")]
            HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(
                new
                {
                    url = signalR.GetClientHubUrl("chatServerlessHub"),
                    accessToken = signalR.GenerateAccessToken("chatServerlessHub")
                });
        }
    }
}
