using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WorkplacePlanner.Data;
using WorkPlacePlanner.Domain.Services;
using WorkplacePlanner.Services;
using WorkplacePlanner.Utills.ErrorHandling;
using WorkplacePlanner.Utills.ConfigSettings;
using Microsoft.Extensions.Options;

namespace WorkplacePlanner.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMembershipService, MembershipService>();

            services.Configure<CorsSettings>(Configuration.GetSection("CorsSettings"));

            //services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext context, IOptions<CorsSettings> corsSettings)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var corsSett = corsSettings.Value;

            //Accept All HTTP Request Methods from all origins
            app.UseCors(builder => builder.WithOrigins(corsSett.AllowedOrigins)
                                        .AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseMiddleware(typeof(ErrorLoggingMiddleware));
            app.UseMvc();

            //This will populate some test data automatically that will help in early stages of development. Remove this once the product is stable.
            DbInitializer.Initialize(context);
        }
    }
}
