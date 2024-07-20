using JTea.OfferScrappers.WindowsService.Middleware;
using JTea.OfferScrappers.WindowsService.Scheduling;
using JTea.OfferScrappers.WindowsService.Settings;
using JToolbox.Core.Abstraction;
using JToolbox.Misc.Logging;
using JToolbox.Misc.TopshelfUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace JTea.OfferScrappers.WindowsService
{
    public class OfferScrappersWindowsService : LocalService
    {
        private WebApplication _webApplication;
        private readonly GlobalSettingsProvider _globalSettingsProvider = new();
        private readonly ILoggerService _loggerService = new LoggerService();

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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            InitializeServices(builder.Services);

            WebApplication app = builder.Build();

            app.UseCors("AllowAll");

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSwaggerUI();
            app.UseSwagger();

            InitializeLifetimeService(app.Services);

            app.UseAuthorization();
            app.MapControllers();

            app.Start();

            _webApplication = app;
        }

        private void InitializeLifetimeService(IServiceProvider serviceProvider)
        {
            IHostApplicationLifetime hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStopping
                .Register(() => ApplicationStopping(serviceProvider));

            hostApplicationLifetime.ApplicationStarted
                .Register(async () => await ApplicationStarted(serviceProvider));
        }

        private async Task ApplicationStarted(IServiceProvider serviceProvider)
        {
            _loggerService.Info("Host application started");

            await serviceProvider.GetService<ISchedulingService>()
                .Initialize();
            _loggerService.Info("Quartz initialized");
        }

        private void ApplicationStopping(IServiceProvider serviceProvider)
        {
            _loggerService.Info("Host application stopping");

            serviceProvider.GetService<ISchedulingService>()
                .Stop()
                .Wait();
            _loggerService.Info("Quartz stopped");
        }

        private void InitializeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(_loggerService);
            serviceCollection.AddSingleton(_globalSettingsProvider);

            InitializeQuartz(serviceCollection);
        }

        private void InitializeQuartz(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<QuartzOptions>(_globalSettingsProvider.QuartzSection);
            serviceCollection.AddQuartz(x => x.UseDefaultThreadPool(y => y.MaxConcurrency = 2));

            serviceCollection.AddSingleton<ISchedulingService, SchedulingService>();
            serviceCollection.AddSingleton<IJobFactory, JobFactory>();
            serviceCollection.AddScoped<MainJob>();
        }
    }
}