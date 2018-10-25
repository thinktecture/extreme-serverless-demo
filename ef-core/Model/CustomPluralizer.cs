using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Serverless.Model
{
  public class CustomPluralizer : IPluralizer
  {
    public string Pluralize(string name)
    {
        return Inflector.Inflector.Pluralize(name) ?? name;
    }
    public string Singularize(string name)
    {
        return Inflector.Inflector.Singularize(name) ?? name;
    }
  }

  public class CustomDesignTimeServices : IDesignTimeServices
  {
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
      services.AddSingleton<IPluralizer, CustomPluralizer>();
    }
  }
}