using JTea.OfferScrappers.Olx;
using JTea.OfferScrappers.OtoDom;
using JTea.OfferScrappers.OtoMoto;
using Newtonsoft.Json;

namespace JTea.OfferScrappers.ConsoleExample
{
    internal static class Program
    {
        private static async Task Main(string[] _)
        {
            try
            {
                await ScrapFromOlx();
                //ScrapFromOtoDom();
                //ScrapFromOtoMoto();
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

        private static async Task Scrap(BaseScrapper scrapper, string fileName)
        {
            WriteLine($"Scrapping offers with {scrapper.GetType().Name} from {scrapper.FullOfferUrl}", ConsoleColor.Cyan);

            List<Offer> result = await scrapper.Scrap(new ScrapperConfiguration()
            {
                DelayBetweenSubPagesChecks = TimeSpan.FromSeconds(1),
                PageSourceProvider = new SeleniumPageSourceProvider.PageSourceProvider(true),
                CheckSubpages = true,
                PageSourceLogPath = "C://test"
            });

            WriteLine($"Scrapped offers count text: {scrapper.OffersCountText}", ConsoleColor.Green);
            WriteLine($"Scrapped offers: {result.Count}", ConsoleColor.Green);

            int invalidOffersCount = result.Count(x => !x.IsValid);
            WriteLine($"Invalid offers count: {invalidOffersCount}",
                invalidOffersCount == 0 ? ConsoleColor.Green : ConsoleColor.Red);

            string filePath = SaveOffers(result, fileName);
            Console.WriteLine($"Offers serialized to: {filePath}");
        }

        private static Task ScrapFromOlx()
        {
            const string offerUrl = "oferty/q-elitebook-830/";

            BaseScrapper scrapper = new OlxScapper(offerUrl);

            return Scrap(scrapper, "olx_offers");
        }

        private static Task ScrapFromOtoDom()
        {
            const string offerUrl = "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=LATEST&direction=DESC";

            BaseScrapper scrapper = new OtoDomScapper(offerUrl);

            return Scrap(scrapper, "otodom_offers");
        }

        private static Task ScrapFromOtoMoto()
        {
            const string offerUrl = "osobowe/honda/civic/od-2023";

            BaseScrapper scrapper = new OtoMotoScrapper(offerUrl);

            return Scrap(scrapper, "otomoto_offers");
        }

        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}