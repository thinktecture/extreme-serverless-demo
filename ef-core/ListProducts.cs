using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using Serverless.Model;
using System.Linq;

namespace Serverless
{
    public static class ListProducts
    {
        [FunctionName("ListProducts")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "products")]
            HttpRequest req,
            [Inject]
            CWDBContext dbContext,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var products = dbContext.Products.ToList();

            return new OkObjectResult(products);
        }
    }
}
