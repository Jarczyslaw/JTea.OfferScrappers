using JTea.OfferScrappers.Olx;
using Newtonsoft.Json;

namespace JTea.OfferScrappers.ConsoleExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                string baseUrl = "https://www.olx.pl/";
                string offerUrl = "oferty/q-elitebook-830/";

                Console.WriteLine($"Scrapping offers from {BaseScrapper.CombinePaths(baseUrl, offerUrl)}");

                var scrapper = new OlxScapper(baseUrl, offerUrl);
                List<Offer> result = scrapper.Scrap(new ScrapperConfiguration()).Result;

                Console.WriteLine($"Scrapped offers count text: {scrapper.OffersCountText}");
                Console.WriteLine($"Scrapped {result.Count} offers");

                string serializedOffers = JsonConvert.SerializeObject(result, Formatting.Indented);
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "offers.json");
                File.WriteAllText(filePath, serializedOffers);

                Console.WriteLine($"Offers serialized to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}