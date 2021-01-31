using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer;

[assembly: FunctionsStartup(typeof(SportPlannerIngestion.Startup))]
namespace SportPlannerIngestion
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var password = System.Environment.GetEnvironmentVariable("dbMattiasLijPassword");
            var connectionString = $"Server=tcp:sportplannerserver.database.windows.net,1433;Initial Catalog=sportplannerdb;Persist Security Info=False;User ID=mattiaslij;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var dataLayer = new DataAccess(connectionString);
            builder.Services.AddSingleton<IDataAccess>(dataLayer);
        }
    }
}
