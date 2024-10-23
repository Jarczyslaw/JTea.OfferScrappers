namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class UpdateOfferHeaderModel
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }
    }
}