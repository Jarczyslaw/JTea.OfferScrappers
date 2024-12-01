using JTea.OfferScrappers.Logic.Abstraction.Models;
using JTea.OfferScrappers.Logic.Abstraction.Services;
using Microsoft.Extensions.Configuration;

namespace JTea.OfferScrappers.WebHost.Settings
{
    public class GlobalSettingsProvider : IGlobalSettingsProvider
    {
        public IConfigurationSection QuartzSection => GetRoot().GetSection("Quartz");

        public GlobalSettings Settings
            => GetRoot()
                .GetSection(nameof(GlobalSettings))
                .Get<GlobalSettings>();

        private IConfigurationRoot GetRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}