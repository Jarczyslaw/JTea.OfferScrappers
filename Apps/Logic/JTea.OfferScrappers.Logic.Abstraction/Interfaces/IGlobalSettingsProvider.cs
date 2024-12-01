using JTea.OfferScrappers.Logic.Abstraction.Models;
using Microsoft.Extensions.Configuration;

namespace JTea.OfferScrappers.Logic.Abstraction.Services
{
    public interface IGlobalSettingsProvider
    {
        IConfigurationSection QuartzSection { get; }

        GlobalSettings Settings { get; }
    }
}