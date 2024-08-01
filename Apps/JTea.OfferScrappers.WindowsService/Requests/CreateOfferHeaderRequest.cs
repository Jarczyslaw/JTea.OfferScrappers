namespace JTea.OfferScrappers.WindowsService.Requests
{
    public class CreateOfferHeaderRequest : BaseCreateUpdateOfferHeaderRequest
    {
        public ScrapperType Type { get; set; }
    }
}