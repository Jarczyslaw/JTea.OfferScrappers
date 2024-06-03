using JTea.OfferScrappers.Olx;
using JTea.OfferScrappers.OtoDom;
using Newtonsoft.Json;

namespace JTea.OfferScrappers.ConsoleExample
{
    internal static class Program
    {
        private static void Main(string[] _)
        {
            try
            {
                ScrapFromOlx();
                ScrapFromOtoDom();
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString(), ConsoleColor.Red);
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
            WriteLine($"Scrapping offers with {scrapper.GetType().Name} from {scrapper.FullOfferUrl}", ConsoleColor.Blue);

            List<Offer> result = scrapper.Scrap(new ScrapperConfiguration()
            {
                DelayBetweenSubPagesChecks = TimeSpan.FromSeconds(1),
                PageSourceProvider = new SeleniumPageSourceProvider.PageSourceProvider(true),
                CheckSubpages = false,
                PageSourceLogPath = "C://test"
            }).Result;

            WriteLine($"Scrapped offers count text: {scrapper.OffersCountText}", ConsoleColor.Green);
            WriteLine($"Scrapped {result.Count} offers", ConsoleColor.Green);

            string filePath = SaveOffers(result, fileName);
            Console.WriteLine($"Offers serialized to: {filePath}");
        }

        private static void ScrapFromOlx()
        {
            const string baseUrl = "https://www.olx.pl/";
            const string offerUrl = "oferty/q-elitebook-830/";

            BaseScrapper scrapper = new OlxScapper(baseUrl, offerUrl);

            Scrap(scrapper, "olx_offers");
        }

        private static void ScrapFromOtoDom()
        {
            const string baseUrl = "https://www.otodom.pl/";
            const string offerUrl = "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=LATEST&direction=DESC";

            BaseScrapper scrapper = new OtoDomScapper(baseUrl, offerUrl);

            Scrap(scrapper, "otodom_offers");
        }

        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}