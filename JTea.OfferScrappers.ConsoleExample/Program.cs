using JTea.OfferScrappers.Olx;

namespace JTea.OfferScrappers.ConsoleExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var baseUrl = "https://www.olx.pl/";
                var offerUrl = "oferty/q-elitebook-830/";

                Console.WriteLine($"Scrapping offers from {BaseScrapper.CombinePaths(baseUrl, offerUrl)}");

                var scrapper = new OlxScapper(baseUrl, offerUrl);
                List<Offer> result = scrapper.Scrap(new ScrapperConfiguration()).Result;

                Console.WriteLine($"Scrapped {result.Count} offers");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}