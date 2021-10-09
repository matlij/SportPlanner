using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SportPlannerApi.DataLayer.DataAccess;
using SportPlannerApi.DataLayer.Profiles;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SportPlannerApi", Version = "v1" });
            });

            services.AddDbContext<SportPlannerContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("dbConnectionString")));

            services.AddTransient<IRepository<Address>, Repository<Address>>();
            services.AddTransient<IRepository<Event>, Repository<Event>>();
            services.AddTransient<IRepository<User>, Repository<User>>();

            services.AddAutoMapper(typeof(SportPlannerProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SportPlannerApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
