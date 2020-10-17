using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModelsCore.Interfaces;

[assembly: FunctionsStartup(typeof(SportPlannerIngestion.Startup))]
namespace SportPlannerIngestion
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDataLayer, DataLayer.DataLayer>();
        }
    }
}
