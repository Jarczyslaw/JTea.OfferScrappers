using JTea.OfferScrappers.Olx;
using JTea.OfferScrappers.OtoDom;
using JTea.OfferScrappers.OtoMoto;

namespace JTea.OfferScrappers
{
    public static class ScrapperFactory
    {
        public static Scrapper Create(ScrapperType type, string offerUrl)
        {
            switch (type)
            {
                case ScrapperType.Olx:
                    return new OlxScapper(offerUrl);

                case ScrapperType.OtoDom:
                    return new OtoDomScapper(offerUrl);

                case ScrapperType.OtoMoto:
                    return new OtoMotoScrapper(offerUrl);

                default:
                    return null;
            }
        }
    }
}