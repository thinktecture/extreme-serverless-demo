using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ExtremeServerless.Functions.Functions.Helpers
{
    public static class LetsEncrypt
    {
        [FunctionName("LetsEncrypt")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "letsencrypt/{code}")]
            HttpRequestMessage req, 
            string code,
            TraceWriter log)
        {
            log.Info($"LetsEncrypt HTTP trigger function processed a request: {code}");

            var content = File.ReadAllText(@"D:\home\site\wwwroot\.well-known\acme-challenge\" + code);

            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, "text/plain")
            };

            return resp;
        }
    }
}
