using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serverless.Model;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace Serverless
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        public IServiceProvider Build()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("SqlConnectionString");

            var services = new ServiceCollection();
            services.AddDbContext<CWDBContext>(options => options.UseSqlServer(connectionString));

            return services.BuildServiceProvider(true);
        }
    }
}
