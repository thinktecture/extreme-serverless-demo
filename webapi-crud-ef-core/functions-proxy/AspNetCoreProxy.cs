using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using TodoApi;

namespace AspNetCoreProxyFunctionApp
{
    public static class AspNetCoreProxy
    {
        private static readonly HttpClient Client =
            new InMemoryAspNetCoreProxy().CreateClientFor<Startup>();

        [FunctionName("AspNetCoreProxy")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS",
                Route = "{*x:regex(^(?!admin|debug|monitoring).*$)}")]
            HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("***ASP.NET Core Proxy: function processed a request***");

            var response = await Client.SendAsync(req);

            return response;
        }
    }
}