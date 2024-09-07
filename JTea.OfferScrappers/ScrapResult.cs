using System.Collections.Generic;
using System.Linq;

namespace JTea.OfferScrappers
{
    public class ScrapResult
    {
        public ScrapResult(string offersCountText, List<Offer> offers)
        {
            OffersCountText = offersCountText;
            Offers = offers;
        }

        public List<Offer> Offers { get; set; } = new List<Offer>();

        public int OffersCount => Offers.Count;

        public string OffersCountText { get; set; }

        public int ValidOffers => Offers.Count(x => x.IsValid);
    }
}