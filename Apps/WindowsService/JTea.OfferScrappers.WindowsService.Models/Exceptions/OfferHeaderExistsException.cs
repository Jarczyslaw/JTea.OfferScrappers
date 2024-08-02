namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class OfferHeaderExistsException : DefinedException
    {
        public OfferHeaderExistsException(ScrapperType scrapperType, string offerUrl)
            : base($"Scrapper with type {scrapperType} for {offerUrl} url already exists")
        {
        }

        public OfferHeaderExistsException(string title)
            : base($"Scrapper with title {title} already exists")
        {
        }
    }
}