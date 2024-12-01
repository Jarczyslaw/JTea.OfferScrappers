using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.Logic.Models.Exceptions;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.Utils;
using JToolbox.Core.Abstraction;
using JToolbox.Core.Helpers.CollectionsMerge;
using JToolbox.Core.Models.Results;
using JToolbox.Core.TimeProvider;
using MapsterMapper;

namespace JTea.OfferScrappers.Logic.Core.Services
{
    public class ProcessingService : BaseService, IProcessingService
    {
        private static readonly SemaphoreSlim _processingSemaphore = new(1, 1);
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly IOfferHeadersRepository _offerHeadersRepository;
        private readonly ITimeProvider _timeProvider;

        public ProcessingService(
            IMapper mapper,
            ITimeProvider timeProvider,
            IOfferHeadersRepository offerHeadersRepository,
            IConfigurationRepository configurationRepository,
            ILoggerService loggerService)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _configurationRepository = configurationRepository;
            _loggerService = loggerService;
            _offerHeadersRepository = offerHeadersRepository;
        }

        public bool IsRunning => _processingSemaphore.CurrentCount == 0;

        public Task<Result> ProcessAllOfferHeaders(bool waitIfCurrentlyProcessing)
        {
            return ExecuteInProcessingContext(() =>
            {
                List<OfferHeaderModel> offerHeaders = _offerHeadersRepository.GetAll(completeData: true)
                    .Where(x => x.IsEnabled)
                    .ToList();

                if (offerHeaders.Count == 0)
                {
                    return Result<List<OfferHeaderModel>>.AsError(new NoOfferHeadersException());
                }

                return new Result<List<OfferHeaderModel>>(offerHeaders);
            }, waitIfCurrentlyProcessing);
        }

        public Task<Result> ProcessOfferHeader(int offerHeaderId, bool waitIfCurrentlyProcessing)
        {
            return ExecuteInProcessingContext(() =>
            {
                OfferHeaderModel offerHeader = _offerHeadersRepository.GetById(offerHeaderId, completeData: true);
                if (offerHeader == null)
                {
                    return Result<List<OfferHeaderModel>>.AsError(new OfferHeaderNotFoundException(offerHeaderId));
                }

                if (!offerHeader.IsEnabled)
                {
                    return Result<List<OfferHeaderModel>>.AsError(new OfferHeaderIsNotEnabled(offerHeaderId));
                }

                return new Result<List<OfferHeaderModel>>([offerHeader]);
            }, waitIfCurrentlyProcessing);
        }

        public async Task<ScrapResultModel> ScrapOffers(ScrapOffersArgumentsModel arguments)
        {
            Scrapper scrapper = ScrapperFactory.Create(arguments.OfferType, arguments.OfferUrl);

            ScrapResult scrapResult = await scrapper.Scrap(new ScrapperConfiguration
            {
                PageSourceProvider = PageSourceProviderFactory.Create(arguments.PageSourceProviderType)
            });

            return _mapper.Map<ScrapResultModel>(scrapResult);
        }

        public Task<ScrapResultModel> ScrapTestOffers(ScrapperType scrapperType, PageSourceProviderType pageSourceProviderType)
        {
            ScrapOffersArgumentsModel arguments = new()
            {
                OfferType = scrapperType,
                PageSourceProviderType = pageSourceProviderType,
                OfferUrl = scrapperType switch
                {
                    ScrapperType.Olx => "oferty/q-elitebook-8470p/",
                    ScrapperType.OtoMoto => "osobowe/honda/civic/od-2023",
                    ScrapperType.OtoDom => "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=DEFAULT&direction=DESC&viewType=listing",
                    _ => throw new ArgumentException(),
                }
            };
            return ScrapOffers(arguments);
        }

        private async Task<Result> ExecuteInProcessingContext(Func<Result<List<OfferHeaderModel>>> offerHeadersSource, bool waitIfCurrentlyProcessing)
        {
            try
            {
                if (waitIfCurrentlyProcessing)
                {
                    await _processingSemaphore.WaitAsync();
                }
                else
                {
                    bool entered = await _processingSemaphore.WaitAsync(TimeSpan.Zero);
                    if (!entered)
                    {
                        return GetProcessingStateResult();
                    }
                }

                return await StartProcessing(offerHeadersSource);
            }
            finally
            {
                _processingSemaphore.Release();
            }
        }

        private void FinalizeProcessingResults(OfferHeaderModel offerHeader, List<OfferModel> newOffers)
        {
            List<OfferModel> oldOffers = offerHeader.Offers;

            CollectionsMergeResult<OfferModel, OfferModel> mergeResult
                = CollectionsMergeHelper.Merge(newOffers, oldOffers, x => x.TargetHref);

            // TODO JTJT obsługa pobrania nowych ofert
        }

        private async Task<Result> StartProcessing(Func<Result<List<OfferHeaderModel>>> offerHeadersSource)
        {
            ConfigurationModel configuration = _configurationRepository.GetConfiguration();

            Result<List<OfferHeaderModel>> offerHeadersResult = offerHeadersSource();
            if (offerHeadersResult.IsError) { return offerHeadersResult; }

            foreach (OfferHeaderModel offerHeader in offerHeadersResult.Value)
            {
                try
                {
                    offerHeader.ClearProcessingData();

                    offerHeader.LastCheckDateStart = _timeProvider.Now;

                    Scrapper scrapper = ScrapperFactory.Create(offerHeader.Type, offerHeader.OfferUrl);
                    ScrapResult scrapResult = await scrapper.Scrap(new ScrapperConfiguration
                    {
                        PageSourceProvider = new SeleniumPageSourceProvider.PageSourceProvider(true),
                        DelayBetweenSubPagesChecks = TimeSpan.FromSeconds(configuration.DelayBetweenSubPagesChecksSeconds)
                    });

                    offerHeader.LastCheckDateEnd = _timeProvider.Now;
                    offerHeader.IsLastCheckValid = true;
                    offerHeader.LastCheckOffersCount = scrapResult.Offers.Count;

                    List<OfferModel> newOffers = _mapper.Map<List<OfferModel>>(scrapResult.Offers);

                    FinalizeProcessingResults(offerHeader, newOffers);
                }
                catch (Exception ex)
                {
                    offerHeader.IsLastCheckValid = false;
                    // TODO JTJT dorobić obsługę błędów

                    // TODO JTJT ogarnąć ogólnie obsługę błędów
                }
            }

            return new Result();
        }
    }
}