using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer.DataAccess;

[assembly: FunctionsStartup(typeof(SportPlannerIngestion.Startup))]
namespace SportPlannerIngestion
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var password = System.Environment.GetEnvironmentVariable("dbMattiasLijPassword");
            //var connectionString = $"Server=tcp:sportplannerserver.database.windows.net,1433;Initial Catalog=sportplannerdb;Persist Security Info=False;User ID=mattiaslij;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var connectionString = "Server=LAPTOP-IGJ8B9LK\\MSSQLSERVERLOCAL;Database=sportplannerdb;User Id=mattiaslij;Password=Njadoks189;";
            var eventDataAccess = new EventDataAccess(connectionString);
            var userDataAccess = new UserDataAccess(connectionString);
            builder.Services.AddSingleton<IEventDataAccess>(eventDataAccess);
            builder.Services.AddSingleton<IUserDataAccess>(userDataAccess);
        }
    }
}
