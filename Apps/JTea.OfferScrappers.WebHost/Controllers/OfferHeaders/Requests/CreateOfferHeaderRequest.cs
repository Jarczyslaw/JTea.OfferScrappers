namespace JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests
{
    public class CreateOfferHeaderRequest : BaseCreateUpdateOfferHeaderRequest
    {
        public ScrapperType Type { get; set; }
    }
}