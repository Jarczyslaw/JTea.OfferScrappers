namespace JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Requests
{
    public class CreateOfferHeaderRequest : BaseCreateUpdateOfferHeaderRequest
    {
        public ScrapperType Type { get; set; }
    }
}