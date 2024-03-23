using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JTea.OfferScrappers.OtoDom
{
    public class OtoDomScapper : BaseScrapper
    {
        public OtoDomScapper(string baseUrl, string offerLink)
            : base(baseUrl, offerLink)
        {
        }

        public override async Task<List<Offer>> Scrap(ScrapperConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}