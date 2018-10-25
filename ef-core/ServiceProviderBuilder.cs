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
        private readonly IConfiguration _configuration;

        public ServiceProviderBuilder(IConfiguration configuration) => _configuration = configuration;

        public IServiceProvider Build()
        {
            var connectionString = _configuration["SqlConnectionString"];

            var services = new ServiceCollection();
            services.AddDbContext<CWDBContext>(options => options.UseSqlServer(connectionString));

            return services.BuildServiceProvider(true);
        }
    }
}
