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
    public static class ProductsFunctions
    {
        [FunctionName("ListProducts")]
        public static IActionResult ListProducts(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "products")]
            HttpRequest req,
            [Inject]
            CWDBContext dbContext,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var products = dbContext.Products.Select(p => new 
            {
                productId = p.ProductId,
                name = p.Name
            }).ToList();

            return new OkObjectResult(products);
        }

        [FunctionName("GetProduct")]
        public static IActionResult GetProduct(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "products/{id}")]
            HttpRequest req,
            [Inject]
            CWDBContext dbContext,
            long id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var product = dbContext.Products.Where(p => p.ProductId == id).FirstOrDefault();

            return new OkObjectResult(product);
        }
    }
}
