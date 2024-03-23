namespace JTea.OfferScrappers
{
    public class Offer
    {
        public virtual string Description { get; }

        public string Price { get; set; }

        public string TargetHref { get; set; }

        public string Title { get; set; }
    }
}