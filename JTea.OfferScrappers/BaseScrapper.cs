using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JTea.OfferScrappers
{
    public abstract class BaseScrapper
    {
        protected BaseScrapper(string baseUrl, string offerLink)
        {
            BaseUrl = baseUrl;
            OfferLink = offerLink;
        }

        public string BaseUrl { get; }

        public string FullOfferLink => CombinePaths(BaseUrl, OfferLink);

        public string OfferLink { get; }

        public string OffersCountText { get; set; }

        public static string CombinePaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0) { return null; }

            return string.Join("/", paths.Select(x => x.Trim('/')));
        }

        public abstract Task<List<Offer>> Scrap(ScrapperConfiguration configuration);

        protected HtmlDocument GetDocument(string path)
        {
            var web = new HtmlWeb();
            return web.Load(path);
        }
    }
}