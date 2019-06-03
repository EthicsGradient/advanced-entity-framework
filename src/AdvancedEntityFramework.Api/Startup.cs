using AdvancedEntityFramework.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackifyMiddleware;

namespace AdvancedEntityFramework.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["SqlServer:ConnectionString"];
            var poolSize = Configuration.GetValue("SqlServer:PoolSize", 128);

            services.AddDbContextPool<SchoolDbContext>(
                o => o.UseSqlServer(connectionString, x => x.EnableRetryOnFailure())
                    .ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .EnableSensitiveDataLogging(WebHostEnvironment.IsDevelopment())
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), poolSize);

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestTracerMiddleware>();

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
