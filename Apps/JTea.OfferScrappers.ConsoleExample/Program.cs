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
                await ScrapFromOtoDom();
                await ScrapFromOtoMoto();
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

        private static async Task Scrap(Scrapper scrapper, string fileName)
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

            IEnumerable<Offer> invalidOffers = result.Where(x => !x.IsValid);
            WriteLine($"Invalid offers count: {invalidOffers.Count()}",
                !invalidOffers.Any() ? ConsoleColor.Green : ConsoleColor.Red);

            string filePath = SaveOffers(result, fileName);
            Console.WriteLine($"Offers serialized to: {filePath}");
        }

        private static Task ScrapFromOlx()
        {
            const string offerUrl = "oferty/q-elitebook-8470p/";

            Scrapper scrapper = new OlxScapper(offerUrl);

            return Scrap(scrapper, "olx_offers");
        }

        private static Task ScrapFromOtoDom()
        {
            // no results
            //const string offerUrl = "pl/wyniki/sprzedaz/mieszkanie/wiele-lokalizacji?limit=36&ownerTypeSingleSelect=ALL&areaMin=1000000&locations=%5Bslaskie%2Fkatowice%2Fkatowice%2Fkatowice%2Fbrynow--osiedle-zgrzebnioka%2Cslaskie%2Fkatowice%2Fkatowice%2Fkatowice%2Cslaskie%5D&by=DEFAULT&direction=DESC&viewType=listing";
            const string offerUrl = "pl/wyniki/sprzedaz/mieszkanie/slaskie/katowice/katowice/katowice/brynow--osiedle-zgrzebnioka?limit=36&ownerTypeSingleSelect=ALL&areaMin=80&by=DEFAULT&direction=DESC&viewType=listing";

            Scrapper scrapper = new OtoDomScapper(offerUrl);

            return Scrap(scrapper, "otodom_offers");
        }

        private static Task ScrapFromOtoMoto()
        {
            // no results
            //const string offerUrl = "osobowe/honda/accord/seg-cabrio/od-2024?search%5Bfilter_float_price%3Ato%5D=50000";
            const string offerUrl = "osobowe/honda/civic/od-2023";

            Scrapper scrapper = new OtoMotoScrapper(offerUrl);

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