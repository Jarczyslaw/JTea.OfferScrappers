using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JTea.OfferScrappers.Olx
{
    public class OlxScapper : BaseScrapper
    {
        public OlxScapper(string baseUrl, string offerLink)
            : base(baseUrl, offerLink)
        {
        }

        public override async Task<List<Offer>> Scrap(ScrapperConfiguration configuration)
        {
            configuration = configuration ?? new ScrapperConfiguration();

            var result = new List<OlxOffer>();

            HtmlDocument document = GetDocument(FullOfferLink);

            OffersCountText = GetOffersCountText(document);

            result = AppendOffersFromDocument(result, document);

            List<OlxOffer> limitedOffers = CheckOffersLimit(result, configuration);
            if (limitedOffers != null) { return CreateScrapperResult(limitedOffers); }

            List<string> offerAdditionalLinks = GetOfferAdditionalLinks(document);

            foreach (string offerAdditionalLink in offerAdditionalLinks)
            {
                await Task.Delay(configuration.DelayBetweenSubPagesChecks);

                document = GetDocument(offerAdditionalLink);

                result = AppendOffersFromDocument(result, document);

                limitedOffers = CheckOffersLimit(result, configuration);
                if (limitedOffers != null) { return CreateScrapperResult(limitedOffers); }
            }

            return CreateScrapperResult(result);
        }

        private List<OlxOffer> AppendOffersFromDocument(List<OlxOffer> result, HtmlDocument document)
        {
            List<OlxOffer> offers = GetOffersFromDocument(document);
            result.AddRange(offers);

            return RemoveDuplicates(result);
        }

        private List<OlxOffer> CheckOffersLimit(List<OlxOffer> offers, ScrapperConfiguration configuration)
        {
            if (configuration.OffersLimits == null) { return null; }

            if (offers.Count > configuration.OffersLimits.Value)
            {
                return offers.Take(configuration.OffersLimits.Value)
                    .ToList();
            }

            return null;
        }

        private OlxOffer CreateOfferFromOfferNode(HtmlNode offerNode)
        {
            var offer = new OlxOffer();

            HtmlNode offerDataNode = offerNode.Element("div")
                ?.Element("div")
                ?.Elements("div").Last()
                ?.Element("div");

            if (offerDataNode == null) { throw new ScrapperParsingException("Can not find offer data node in offer node"); }

            HtmlNode linkNode = offerDataNode
                .Element("a");

            if (linkNode == null) { throw new ScrapperParsingException("Can not find link node in offer data node"); }

            offer.TargetHref = CombinePaths(BaseUrl, linkNode.GetAttributeValue("href", null));

            SetPriceAndToNegotiate(offer, offerDataNode);

            HtmlNode headerNode = linkNode.Descendants("h6")
                .FirstOrDefault();

            if (headerNode == null) { throw new ScrapperParsingException("Can not find header node in link node"); }

            offer.Title = headerNode.InnerText;

            SetCondition(offer, offerNode);

            HtmlNode locationAndDateNode = offerNode.Descendants("p")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "location-date");

            if (locationAndDateNode == null) { throw new ScrapperParsingException("Can not find location and date node in offer node"); }

            offer.LocationAndDate = locationAndDateNode.GetDirectInnerText();

            return offer;
        }

        private List<Offer> CreateScrapperResult(List<OlxOffer> offers)
            => offers.Cast<Offer>().ToList();

        private List<string> GetOfferAdditionalLinks(HtmlDocument document)
        {
            HtmlNode paginationListNode = document.DocumentNode.Descendants("ul")
               .FirstOrDefault(x => x.HasClass("pagination-list"));

            if (paginationListNode == null) { return null; }

            string lastSubPageInnerText = paginationListNode.Descendants("a")
                .Last(x => x.ParentNode.Name == "li")
                .GetDirectInnerText();

            int lastSubPage = int.Parse(lastSubPageInnerText);

            var subPagesLinks = new List<string>();
            for (int i = 2; i <= lastSubPage; i++)
            {
                subPagesLinks.Add(CombinePaths(FullOfferLink, $"?page={i}"));
            }

            return subPagesLinks;
        }

        private string GetOffersCountText(HtmlDocument document)
        {
            return document.DocumentNode.Descendants("span")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "total-count")
                ?.GetDirectInnerText();
        }

        private List<OlxOffer> GetOffersFromDocument(HtmlDocument document)
        {
            var offers = new List<OlxOffer>();

            List<HtmlNode> offerNodes = document.DocumentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("data-testid", null) == "l-card")
                .ToList();

            foreach (HtmlNode offerNode in offerNodes)
            {
                OlxOffer offer = CreateOfferFromOfferNode(offerNode);
                offers.Add(offer);
            }

            return offers;
        }

        private List<OlxOffer> RemoveDuplicates(List<OlxOffer> offers)
            => offers.GroupBy(x => x.TargetHref)
                .Select(x => x.First())
                .ToList();

        private void SetCondition(OlxOffer offer, HtmlNode linkNode)
        {
            IEnumerable<HtmlNode> spanNodes = linkNode.Descendants("span");

            string newCondition = "Nowe";
            if (spanNodes.Any(x => x.GetAttributeValue("title", null) == newCondition))
            {
                offer.Condition = newCondition;
                return;
            }

            string usedCondition = "Używane";
            if (spanNodes.Any(x => x.GetAttributeValue("title", null) == usedCondition))
            {
                offer.Condition = usedCondition;
            }
        }

        private void SetPriceAndToNegotiate(OlxOffer offer, HtmlNode offerDataNode)
        {
            HtmlNode priceNode = offerDataNode.Elements("p")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "ad-price");

            if (priceNode == null) { throw new ScrapperParsingException("Can not find price node in offer data node"); }

            offer.Price = priceNode.GetDirectInnerText();

            HtmlNode spanChild = priceNode.Elements("span")
                .FirstOrDefault(x => x.FirstChild?.Name == "#text");

            if (spanChild == null) { return; }

            offer.ToNegotiate = spanChild.GetDirectInnerText();
        }
    }
}