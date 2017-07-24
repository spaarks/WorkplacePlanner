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
using WorkplacePlanner.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                    .AddEntityFrameworkStores<DataContext, int>()
                    .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IAccountService, AccountService>();

            services.Configure<CorsSettings>(Configuration.GetSection("CorsSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            DataContext context, 
            IOptions<CorsSettings> corsSettings, 
            IOptions<AppSettings> appSettings, 
            UserManager<ApplicationUser> userManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Accept All HTTP Request Methods from all origins
            app.UseCors(builder => builder.WithOrigins(corsSettings.Value.AllowedOrigins)
                                        .AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseMiddleware(typeof(ErrorLoggingMiddleware));

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = appSettings.Value.AuthTokenIssuerUrl,
                    ValidAudience = appSettings.Value.AuthTokenIssuerUrl,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.AuthTokenKey)),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();

            //This will populate some test data automatically that will help in early stages of development. Remove this once the product is stable.
            DbInitializer.Initialize(context, userManager);
        }
    }
}
