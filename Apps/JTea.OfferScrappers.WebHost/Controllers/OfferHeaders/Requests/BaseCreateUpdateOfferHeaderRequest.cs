namespace JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests
{
    public abstract class BaseCreateUpdateOfferHeaderRequest
    {
        public bool IsEnabled { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }
    }
}