using FluentValidation;
using FluentValidation.AspNetCore;
using JTea.OfferScrappers.WindowsService.Abstraction.Services;
using JTea.OfferScrappers.WindowsService.Core.Services;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Repositories;
using JTea.OfferScrappers.WindowsService.Requests;
using JTea.OfferScrappers.WindowsService.Scheduling;
using JTea.OfferScrappers.WindowsService.Settings;
using JTea.OfferScrappers.WindowsService.Validators;
using JToolbox.Core.Abstraction;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.Misc.Logging;
using JToolbox.Misc.TopshelfUtils;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Spi;

namespace JTea.OfferScrappers.WindowsService
{
    public class OfferScrappersWindowsService : LocalService
    {
        private readonly GlobalSettingsProvider _globalSettingsProvider = new();
        private readonly ILoggerService _loggerService = new LoggerService();
        private WebApplication _webApplication;

        public override string ServiceName => "JTea.OfferScrappers";

        public override bool Start(bool launchedInConsole)
        {
            InitializeWebApplication(launchedInConsole);

            _loggerService.Info($"{ServiceName} started");

            return base.Start(launchedInConsole);
        }

        public override bool Stop()
        {
            _webApplication?.StopAsync()
                .Wait();

            _loggerService.Info($"{ServiceName} stopped");

            return base.Stop();
        }

        private async Task ApplicationStarted(IServiceProvider serviceProvider)
        {
            LogInfo("Host application started");
            LogInfo($"Swagger is available at: {_globalSettingsProvider.Settings.ApiAddress}/swagger/");

            IGlobalSettingsProvider globalSettingsProvider = serviceProvider.GetService<IGlobalSettingsProvider>();

            IDataAccessService dataAccessService = serviceProvider.GetService<IDataAccessService>();

            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, globalSettingsProvider.Settings.DbFileName);
            await dataAccessService.Init(
                databasePath,
                password: null,
                skipInitialization: false);

            LogInfo("Database initialized");

            ConfigurationModel configuration = serviceProvider.GetService<IConfigurationRepository>()
                .GetConfiguration();

            ISchedulingService schedulingService = serviceProvider.GetService<ISchedulingService>();

            await schedulingService.Initialize();
            LogInfo($"Quartz initialized with cron expression: {configuration.CronExpression}");
        }

        private void ApplicationStopping(IServiceProvider serviceProvider)
        {
            LogInfo("Host application stopping");

            serviceProvider.GetService<ISchedulingService>()
                .Stop()
                .Wait();
            LogInfo("Quartz stopped");
        }

        private void InitializeCoreServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IConfigurationService, ConfigurationService>();
            serviceCollection.AddScoped<IOfferHeadersService, OfferHeadersService>();
        }

        private void InitializeDatabase(IServiceCollection serviceCollection)
        {
            DbInitializer initializer = new();
            DataAccessService service = new(initializer)
            {
                CacheConnection = false,
                UseMigrationLockFile = false
            };

            serviceCollection.AddSingleton<IDataAccessService>(service);
            serviceCollection.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
            serviceCollection.AddSingleton<IOfferHeadersRepository, OfferHeadersRepository>();
            serviceCollection.AddSingleton<IOffersRepository, OffersRepository>();
            serviceCollection.AddSingleton<IOfferHistoriesRepository, OfferHistoriesRepository>();
        }

        private void InitializeLifetimeService(IServiceProvider serviceProvider)
        {
            IHostApplicationLifetime hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStopping
                .Register(() => ApplicationStopping(serviceProvider));

            hostApplicationLifetime.ApplicationStarted
                .Register(async () => await ApplicationStarted(serviceProvider));
        }

        private void InitializeQuartz(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<QuartzOptions>(_globalSettingsProvider.QuartzSection);
            serviceCollection.AddQuartz(x => x.UseDefaultThreadPool(y => y.MaxConcurrency = 2));

            serviceCollection.AddSingleton<ISchedulingService, SchedulingService>();
            serviceCollection.AddSingleton<IJobFactory, JobFactory>();
            serviceCollection.AddScoped<MainJob>();
        }

        private void InitializeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(_loggerService);
            serviceCollection.AddSingleton<IGlobalSettingsProvider>(_globalSettingsProvider);
            serviceCollection.AddSingleton<IMapper>(new Mapper());

            InitializeQuartz(serviceCollection);
            InitializeDatabase(serviceCollection);
            InitializeCoreServices(serviceCollection);
        }

        private void InitializeWebApplication(bool launchedInConsole)
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
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGenNewtonsoftSupport();

            builder.Services.AddFluentValidationAutoValidation();

            RegisterValidators(builder.Services);

            InitializeServices(builder.Services);

            WebApplication app = builder.Build();

            // WTF this line is needed to make global exception handling working
            app.UseExceptionHandler(_ => { });
            app.UseCors("AllowAll");

            app.UseSwaggerUI();
            app.UseSwagger();

            InitializeLifetimeService(app.Services);

            app.UseAuthorization();
            app.MapControllers();

            app.Start();

            _webApplication = app;
        }

        private void RegisterValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateConfigurationRequest>, UpdateConfigurationRequestValidator>();
            services.AddScoped<IValidator<CreateOfferHeaderRequest>, CreateOfferHeaderRequestValidator>();
            services.AddScoped<IValidator<UpdateOfferHeaderRequest>, UpdateOfferHeaderRequestValidator>();
        }

        private void LogInfo(string message)
        {
            _loggerService.Info(message);
            Console.WriteLine(message);
        }
    }
}