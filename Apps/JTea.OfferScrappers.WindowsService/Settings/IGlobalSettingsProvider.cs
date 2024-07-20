using Microsoft.Extensions.Configuration;

namespace JTea.OfferScrappers.WindowsService.Settings
{
    public interface IGlobalSettingsProvider
    {
        IConfigurationSection QuartzSection { get; }
        GlobalSettings Settings { get; }
    }
}