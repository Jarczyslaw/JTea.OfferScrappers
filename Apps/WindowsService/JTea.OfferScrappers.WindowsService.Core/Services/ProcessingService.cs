using JTea.OfferScrappers.Utils;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ProcessingService : IProcessingService
    {
        private readonly IMapper _mapper;

        public ProcessingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ProcessingState State { get; private set; }

        public async Task<ScrapResultModel> FetchOffers(FetchOffersArguments arguments)
        {
            Scrapper scrapper = ScrapperFactory.Create(arguments.OfferType, arguments.OfferUrl);

            ScrapResult scrapResult = await scrapper.Scrap(new ScrapperConfiguration
            {
                PageSourceProvider = PageSourceProviderFactory.Create(arguments.PageSourceProviderType)
            });

            return _mapper.Map<ScrapResultModel>(scrapResult);
        }

        public Task<ScrapResultModel> TestFetchOffers(ScrapperType scrapperType, PageSourceProviderType pageSourceProviderType)
        {
            FetchOffersArguments arguments = new()
            {
                OfferType = scrapperType,
                PageSourceProviderType = pageSourceProviderType
            };

            arguments.OfferUrl = scrapperType switch
            {
                ScrapperType.Olx => "oferty/q-elitebook-8470p/",
                ScrapperType.OtoMoto => "osobowe/honda/civic/od-2023",
                ScrapperType.OtoDom => "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=DEFAULT&direction=DESC&viewType=listing",
                _ => throw new ArgumentException(),
            };
            return FetchOffers(arguments);
        }
    }
}