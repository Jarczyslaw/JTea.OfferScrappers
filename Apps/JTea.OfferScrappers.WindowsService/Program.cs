namespace JTea.OfferScrappers.WindowsService
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            OfferScrappersWindowsService service = new();
            service.Run();
        }
    }
}