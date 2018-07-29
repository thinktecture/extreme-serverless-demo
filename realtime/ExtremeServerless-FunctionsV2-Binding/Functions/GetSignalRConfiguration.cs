using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;

namespace ExtremeServerless.Functions
{
    public static class GetSignalRConfiguration
    {
        [FunctionName("GetSignalRConfiguration")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="config")]
            HttpRequest req,
            [SignalRConnectionInfo(HubName = "chatServerlessHub")]
            AzureSignalRConnectionInfo connectionInfo,
            TraceWriter log)
        {
            return connectionInfo != null
                ? (ActionResult)new OkObjectResult(connectionInfo)
                : new NotFoundObjectResult("Could not retrieve SignalR connection info.");
        }
    }
}
