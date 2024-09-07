namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class UpdateOfferHeader
    {
        public bool Enabled { get; set; }

        public int Id { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }
    }
}