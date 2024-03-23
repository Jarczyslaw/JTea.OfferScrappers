using System;

namespace JTea.OfferScrappers.OtoDom
{
    public class OtoDomOffer : Offer
    {
        public string Condition { get; set; }

        public override string Description => throw new NotImplementedException();

        public string LocationAndDate { get; set; }

        public string ToNegotiate { get; set; }
    }
}