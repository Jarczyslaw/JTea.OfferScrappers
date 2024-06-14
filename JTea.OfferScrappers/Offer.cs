namespace JTea.OfferScrappers
{
    public class Offer
    {
        public virtual string Description { get; }

        public string ImageHref { get; set; }

        public virtual bool IsValid
            => !string.IsNullOrEmpty(ImageHref)
                && !string.IsNullOrEmpty(Price)
                && !string.IsNullOrEmpty(TargetHref)
                && !string.IsNullOrEmpty(Title);

        public string Price { get; set; }

        public string TargetHref { get; set; }

        public string Title { get; set; }
    }
}