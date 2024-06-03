using HtmlAgilityPack;
using JTea.OfferScrappers.Abstraction;
using System.Threading.Tasks;

namespace JTea.OfferScrappers
{
    public class PageSourceProvider : IPageSourceProvider
    {
        public async Task<string> GetPageSource(string url)
        {
            var web = new HtmlWeb();
            HtmlDocument document = await web.LoadFromWebAsync(url);

            return document.DocumentNode.OuterHtml;
        }
    }
}