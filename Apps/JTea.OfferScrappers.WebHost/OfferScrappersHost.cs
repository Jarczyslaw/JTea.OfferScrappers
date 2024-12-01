using FluentValidation.AspNetCore;
using JTea.OfferScrappers.Logic.Abstraction.Services;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.WebHost.Controllers;
using JTea.OfferScrappers.WebHost.ErrorHandling;
using JTea.OfferScrappers.WebHost.Settings;
using JToolbox.Core.Abstraction;
using JToolbox.DataAccess.L2DB;
using JToolbox.Misc.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JTea.OfferScrappers.WebHost
{
    public class OfferScrappersHost
    {
        private readonly GlobalSettingsProvider _globalSettingsProvider = new();
        private readonly ILoggerService _loggerService = new LoggerService();
        private WebApplication _webApplication;

        public void Start()
        {
            InitializeWebApplication();

            _loggerService.Info($"{nameof(OfferScrappersHost)} started");
        }

        public void Stop()
        {
            _webApplication?.StopAsync()
                .Wait();

            _loggerService.Info($"{nameof(OfferScrappersHost)} stopped");
        }

        private async Task ApplicationStarted(IServiceProvider serviceProvider)
        {
            LogInfo("Host application started");
            LogInfo($"Swagger is available at: {_globalSettingsProvider.Settings.ApiAddress}/swagger/");

            await InitializeDatabase(serviceProvider);
            LogInfo("Database initialized");

            ConfigurationModel configuration = await InitializeQuartz(serviceProvider);
            LogInfo($"Quartz initialized with cron expression: {configuration.CronExpression}");
        }

        private static async Task<ConfigurationModel> InitializeQuartz(IServiceProvider serviceProvider)
        {
            ConfigurationModel configuration = serviceProvider.GetService<IConfigurationRepository>()
                .GetConfiguration();

            ISchedulingService schedulingService = serviceProvider.GetService<ISchedulingService>();

            await schedulingService.Initialize();
            await schedulingService.StartWithCron(configuration.CronExpression);
            return configuration;
        }

        private static async Task InitializeDatabase(IServiceProvider serviceProvider)
        {
            IGlobalSettingsProvider globalSettingsProvider = serviceProvider.GetService<IGlobalSettingsProvider>();

            IDataAccessService dataAccessService = serviceProvider.GetService<IDataAccessService>();

            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, globalSettingsProvider.Settings.DbFileName);
            dataAccessService.ConnectionString = $"Data Source={databasePath};Version=3;";
            await dataAccessService.Init();
        }

        private void ApplicationStopping(IServiceProvider serviceProvider)
        {
            LogInfo("Host application stopping");

            serviceProvider.GetService<ISchedulingService>()
                .Stop()
                .Wait();
            LogInfo("Quartz stopped");
        }

        private void InitializeLifetimeService(IServiceProvider serviceProvider)
        {
            IHostApplicationLifetime hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStopping
                .Register(() => ApplicationStopping(serviceProvider));

            hostApplicationLifetime.ApplicationStarted
                .Register(async () => await ApplicationStarted(serviceProvider));
        }

        private void InitializeWebApplication()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

#if DEBUG
            builder.Environment.EnvironmentName = Environments.Development;
#endif

            builder.WebHost.UseDefaultServiceProvider(x =>
            {
                x.ValidateScopes =
                    x.ValidateOnBuild = true;
            });

            builder.WebHost.UseUrls(_globalSettingsProvider.Settings.ApiAddress);

            // Needed to override default model validation handling
            builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(BaseController).Assembly)
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGenNewtonsoftSupport();
            builder.Services.AddHttpClient("swaggerClient", client => client.Timeout = TimeSpan.FromMinutes(15));

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddApplicationServices(_loggerService, _globalSettingsProvider);

            WebApplication app = builder.Build();

            // WTF this line is needed to make global exception handling working
            app.UseExceptionHandler(_ => { });
            app.UseCors("AllowAll");

            app.UseSwaggerUI(x => x.DisplayRequestDuration());
            app.UseSwagger();

            InitializeLifetimeService(app.Services);

            app.UseAuthorization();
            app.MapControllers();

            app.Start();

            _webApplication = app;
        }

        private void LogInfo(string message)
        {
            _loggerService.Info(message);
            Console.WriteLine(message);
        }
    }
}