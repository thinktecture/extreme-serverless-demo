using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ExtremeServerless.Functions
{
    public static class GetSignalRConfiguration
    {
        private static AzureSignalR signalR = new AzureSignalR(
            Environment.GetEnvironmentVariable("SignalR"));

        [FunctionName("GetSignalRConfiguration")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous)]
            HttpRequestMessage req,
            TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK,
                new
                {
                    hubUrl = signalR.GetClientHubUrl("chatServerlessHub"),
                    accessToken = signalR.GenerateAccessToken("chatServerlessHub")
                });
        }
    }
}
