using JTea.OfferScrappers.Olx;
using JTea.OfferScrappers.OtoDom;
using Newtonsoft.Json;

namespace JTea.OfferScrappers.ConsoleExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //ScrapFromOlx();
                ScrapFromOtoDom();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        private static string SaveOffers(List<Offer> offers, string fileName)
        {
            string serializedOffers = JsonConvert.SerializeObject(offers, Formatting.Indented);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{fileName}.json");
            File.WriteAllText(filePath, serializedOffers);

            return filePath;
        }

        private static void Scrap(BaseScrapper scrapper, string fileName)
        {
            Console.WriteLine($"Scrapping offers with {scrapper.GetType().Name} from {scrapper.FullOfferUrl}");

            List<Offer> result = scrapper.Scrap(new ScrapperConfiguration()
            {
                DelayBetweenSubPagesChecks = TimeSpan.FromSeconds(1),
                PageSourceProvider = new SeleniumPageSourceProvider.PageSourceProvider(true),
                CheckSubpages = false,
                PageSourceLogPath = "C://test"
            }).Result;

            Console.WriteLine($"Scrapped offers count text: {scrapper.OffersCountText}");
            Console.WriteLine($"Scrapped {result.Count} offers");

            string filePath = SaveOffers(result, fileName);
            Console.WriteLine($"Offers serialized to: {filePath}");
        }

        private static void ScrapFromOlx()
        {
            string baseUrl = "https://www.olx.pl/";
            string offerUrl = "oferty/q-elitebook-830/";

            BaseScrapper scrapper = new OlxScapper(baseUrl, offerUrl);

            Scrap(scrapper, "olx_offers");
        }

        private static void ScrapFromOtoDom()
        {
            string baseUrl = "https://www.otodom.pl/";
            string offerUrl = "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=LATEST&direction=DESC";

            BaseScrapper scrapper = new OtoDomScapper(baseUrl, offerUrl);

            Scrap(scrapper, "otodom_offers");
        }
    }
}