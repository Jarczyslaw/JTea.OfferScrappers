using Microsoft.Extensions.Configuration;

namespace JTea.OfferScrappers.WindowsService.Settings
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