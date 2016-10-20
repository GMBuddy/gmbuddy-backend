using System.Threading.Tasks;
using GMBuddy.Identity.Data;
using GMBuddy.Identity.Models;
using GMBuddy.Identity.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GMBuddy.Identity
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<IdentityContext>();

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;

                    // Force ASP.NET to not redirect when returning Unauthorized()
                    options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.Configure<JwtOptions>(options =>
            {
                options.SigningCredentials = new SigningCredentials(AuthorizationConstants.SecurityKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddTransient<JwtService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");
            });
        }
    }
}
