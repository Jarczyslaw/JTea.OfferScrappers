using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace JTea.OfferScrappers.OtoMoto
{
    public class OtoMotoScrapper : BaseScrapper
    {
        public OtoMotoScrapper(string baseUrl, string offerUrl)
            : base(baseUrl, offerUrl)
        {
        }

        protected override List<string> GetOfferAdditionalUrls(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override string GetOffersCountText(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override List<Offer> GetOffersFromDocument(HtmlDocument document)
        {
            throw new NotImplementedException();
        }
    }
}