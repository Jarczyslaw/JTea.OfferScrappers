using JTea.OfferScrappers.WindowsService.Abstraction.Models;
using Microsoft.Extensions.Configuration;

namespace JTea.OfferScrappers.WindowsService.Abstraction.Services
{
    public interface IGlobalSettingsProvider
    {
        IConfigurationSection QuartzSection { get; }

        GlobalSettings Settings { get; }
    }
}