using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using SportPlannerFunctionApi.DataLayer.DataAccess;
using SportPlannerFunctionApi.DataLayer.Profiles;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models;
using System;

[assembly: FunctionsStartup(typeof(SportPlannerFunctionApi.Startup))]

namespace SportPlannerFunctionApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IRepository<Address>, Repository<Address>>();
            builder.Services.AddTransient<IRepository<Event>, Repository<Event>>();
            builder.Services.AddTransient<IRepository<User>, Repository<User>>();

            builder.Services.AddAutoMapper(typeof(SportPlannerProfile));
            builder.Services.AddDbContext<SportPlannerContext>(
                o => o.UseSqlServer(Environment.GetEnvironmentVariable("dbConnectionString")));
        }
    }

    public class SportPlannerContextFactory : IDesignTimeDbContextFactory<SportPlannerContext>
    {
        public SportPlannerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SportPlannerContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=sportplannerdb;Trusted_Connection=true");

            return new SportPlannerContext(optionsBuilder.Options);
        }
    }
}
