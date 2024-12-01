using FluentValidation;
using JTea.OfferScrappers.Logic.Abstraction.Services;
using JTea.OfferScrappers.Logic.Core.Services;
using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Persistence;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.Logic.Persistence.Repositories;
using JTea.OfferScrappers.WebHost.Controllers.Configuration.Requests;
using JTea.OfferScrappers.WebHost.Controllers.Configuration.Validators;
using JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests;
using JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Validators;
using JTea.OfferScrappers.WebHost.Scheduling;
using JTea.OfferScrappers.WebHost.Settings;
using JToolbox.Core.Abstraction;
using JToolbox.Core.TimeProvider;
using JToolbox.DataAccess.L2DB;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace JTea.OfferScrappers.WebHost
{
    public static class ApplicationServices
    {
        public static void AddApplicationServices(
            this IServiceCollection services,
            ILoggerService loggerService,
            GlobalSettingsProvider globalSettingsProvider)
        {
            services.AddSingleton(loggerService);
            services.AddSingleton<IGlobalSettingsProvider>(globalSettingsProvider);
            services.AddSingleton<IMapper>(new Mapper());

            ITimeProvider timeProvider = new LocalTimeProvider();
            services.AddSingleton(timeProvider);

            InitializeQuartz(services, globalSettingsProvider);
            InitializeDatabase(services, timeProvider);
            InitializeCoreServices(services);
            RegisterValidators(services);
        }

        private static void InitializeCoreServices(IServiceCollection services)
        {
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IOfferHeadersService, OfferHeadersService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddSingleton<IProcessingService, ProcessingService>();
        }

        private static void InitializeDatabase(IServiceCollection services, ITimeProvider timeProvider)
        {
            DataAccessService dataAccessService = DataAccessFactory.Create(timeProvider);

            services.AddSingleton<IDataAccessService>(dataAccessService);
            services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
            services.AddSingleton<IOfferHeadersRepository, OfferHeadersRepository>();
            services.AddSingleton<IOffersRepository, OffersRepository>();
            services.AddSingleton<IOfferHistoriesRepository, OfferHistoriesRepository>();
        }

        private static void InitializeQuartz(IServiceCollection services, GlobalSettingsProvider globalSettingsProvider)
        {
            services.Configure<QuartzOptions>(globalSettingsProvider.QuartzSection);
            services.AddQuartz(x => x.UseDefaultThreadPool(y => y.MaxConcurrency = 2));

            services.AddSingleton<ISchedulingService, SchedulingService>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddScoped<MainJob>();
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateConfigurationRequest>, UpdateConfigurationRequestValidator>();
            services.AddScoped<IValidator<CreateOfferHeaderRequest>, CreateOfferHeaderRequestValidator>();
            services.AddScoped<IValidator<UpdateOfferHeaderRequest>, UpdateOfferHeaderRequestValidator>();
        }
    }
}