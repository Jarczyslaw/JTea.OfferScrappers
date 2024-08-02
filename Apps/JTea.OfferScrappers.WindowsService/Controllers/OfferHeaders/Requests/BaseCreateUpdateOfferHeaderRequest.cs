namespace JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Requests
{
    public abstract class BaseCreateUpdateOfferHeaderRequest
    {
        public bool Enabled { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }
    }
}