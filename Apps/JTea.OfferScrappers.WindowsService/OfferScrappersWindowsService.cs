using JTea.OfferScrappers.WebHost;
using JToolbox.Core.Abstraction;
using JToolbox.Misc.Logging;
using JToolbox.Misc.TopshelfUtils;

namespace JTea.OfferScrappers.WindowsService
{
    public class OfferScrappersWindowsService : LocalService
    {
        private readonly OfferScrappersHost _host = new();
        private readonly ILoggerService _loggerService = new LoggerService();

        public override string ServiceName => "JTea.OfferScrappers";

        public override bool Start(bool launchedInConsole)
        {
            _host.Start();

            _loggerService.Info($"{ServiceName} started");

            return base.Start(launchedInConsole);
        }

        public override bool Stop()
        {
            _host.Stop();

            _loggerService.Info($"{ServiceName} stopped");

            return base.Stop();
        }
    }
}