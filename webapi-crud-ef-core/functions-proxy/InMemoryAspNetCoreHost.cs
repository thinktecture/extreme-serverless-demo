using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreProxyFunctionApp
{
    public class InMemoryAspNetCoreHost
    {
        public HttpClient CreateClientFor<TStartup>() where TStartup : class
        {
            var functionPath = new FileInfo(typeof(InMemoryAspNetCoreHost).Assembly.Location).Directory.Parent.FullName;
            Directory.SetCurrentDirectory(functionPath);
            var server = CreateServer<TStartup>(functionPath);
            var client = server.CreateClient();

            return client;
        }

        private TestServer CreateServer<TStartup>(string functionPath) where TStartup : class
        {
            return new TestServer(WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config
                        .SetBasePath(functionPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json",
                            optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<TStartup>()
                .UseContentRoot(Path.Combine(functionPath)));
        }
    }
}