using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AspNetCoreProxyFunctionApp
{
    public static class KeepItWarm
    {
        [FunctionName("KeepItWarm")]
        public static void Run(
            [TimerTrigger("0 */9 * * * *")]
            TimerInfo timer,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
