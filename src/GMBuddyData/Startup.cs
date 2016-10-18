using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GMBuddyData
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddUserSecrets();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime when ASPNETCORE_ENVIRONMENT = Development
        /// </summary>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // Use sqlite when youre fine with losing your database a lot
            services.AddDbContext<Data.DND35.GameContext>((options) => options.UseSqlite(Configuration.GetConnectionString("DevelopmentSQLite-DND35")));

            // Use sqlserver at least while model state is in flux, because what are ALTER statements and why doesnt sqlite support them
            // services.AddDbContext<Data.DND35.GameContext>((options) => options.UseSqlServer(Configuration.GetConnectionString("DevelopmentSQLExpress-DND35")));

            ConfigureCommonServices(services);
        }

        /// <summary>
        /// This method gets called by the runtime when ASPNETCORE_ENVIRONMENT = Production
        /// </summary>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<Data.DND35.GameContext>(options => options.UseSqlServer(Configuration["AzureSqlDND35"]));

            // only *require* authentication in production
            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter(
                    new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));
            });

            ConfigureCommonServices(services);
        }

        /// <summary>
        /// Called by ConfigureProductionServices and ConfigureDevelopmentServices to do common service setup logic
        /// </summary>
        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime when ASPNETCORE_ENVIRONMENT = Development
        /// </summary>
        public void ConfigureDevelopment(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            Data.DND35.GameContext dnd35Context)
        {
            ConfigureCommon(app, env, loggerFactory);

            // seed the database with some initial values in development
            Data.DND35.DataInitializer.Init(dnd35Context);
        }

        /// <summary>
        /// This method gets called by the runtime when ASPNETCORE_ENVIRONMENT = Production
        /// </summary>
        public void ConfigureProduction(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureCommon(app, env, loggerFactory);
        }

        /// <summary>
        /// This method gets called by ConfigureProduction and ConfigureDevelopment to do common setup
        /// </summary>
        private void ConfigureCommon(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                ScopeName = "GMBuddyData",
                RequireHttpsMetadata = false
            });

            app.UseMvc((routes) =>
            {
                routes.MapRoute("default", "{area}/{controller}/{action=Index}");
            });
        }
    }
}
