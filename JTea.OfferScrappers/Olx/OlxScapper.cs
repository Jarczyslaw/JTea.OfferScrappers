using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace JTea.OfferScrappers.Olx
{
    public class OlxScapper : Scrapper
    {
        public OlxScapper(string offerUrl)
            : base("https://www.olx.pl/", offerUrl)
        {
        }

        public OlxScapper(string baseUrl, string offerUrl)
            : base(baseUrl, offerUrl)
        {
        }

        protected override List<string> GetOfferAdditionalUrls(HtmlDocument document)
        {
            HtmlNode paginationListNode = document.DocumentNode
                .Descendants("ul")
                .FirstOrDefault(x => x.HasClass("pagination-list"));

            if (paginationListNode == null) { return new List<string>(); }

            HtmlNode lastSubPageNode = paginationListNode
                .Descendants("a")
                .LastOrDefault(x => x.ParentNode.Name == "li");

            CheckNodeExists(lastSubPageNode, nameof(lastSubPageNode));

            int lastSubPage = int.Parse(lastSubPageNode.GetDirectInnerText());

            return CreateSubPagesUrls(lastSubPage);
        }

        protected override string GetOffersCountText(HtmlDocument document)
        {
            HtmlNode totalCountNode = document.DocumentNode
                .Descendants("span")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "total-count");

            CheckNodeExists(totalCountNode, nameof(totalCountNode));

            return totalCountNode.GetDirectInnerText();
        }

        protected override List<Offer> GetOffersFromDocument(HtmlDocument document)
        {
            var offers = new List<Offer>();

            List<HtmlNode> offerNodes = document.DocumentNode
                .Descendants("div")
                .Where(x => x.GetAttributeValue("data-testid", null) == "l-card")
                .ToList();

            if (offerNodes == null || offerNodes.Count == 0) { return new List<Offer>(); }

            foreach (HtmlNode offerNode in offerNodes)
            {
                OlxOffer offer = CreateOfferFromOfferNode(offerNode);
                offers.Add(offer);
            }

            return offers;
        }

        private OlxOffer CreateOfferFromOfferNode(HtmlNode offerNode)
        {
            var offer = new OlxOffer();

            HtmlNode offerDataNode = offerNode
                .Element("div")
                ?.Element("div")
                ?.Elements("div")
                ?.LastOrDefault()
                ?.Element("div");

            CheckNodeExists(offerDataNode, nameof(offerDataNode));

            HtmlNode linkNode = offerDataNode
                .Element("a");

            CheckNodeExists(linkNode, nameof(linkNode));

            offer.TargetHref = CombinePaths(BaseUrl, linkNode.GetAttributeValue("href", null));

            SetPriceAndToNegotiate(offer, offerDataNode);

            HtmlNode headerNode = linkNode
                .Descendants("h6")
                .FirstOrDefault();

            CheckNodeExists(headerNode, nameof(headerNode));

            offer.Title = PrepareText(headerNode.InnerText);

            SetCondition(offer, offerNode);

            HtmlNode locationAndDateNode = offerNode
                .Descendants("p")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "location-date");

            CheckNodeExists(locationAndDateNode, nameof(locationAndDateNode));

            offer.LocationAndDate = PrepareText(locationAndDateNode.GetDirectInnerText());

            SetImage(offer, offerNode);

            return offer;
        }

        private void SetCondition(OlxOffer offer, HtmlNode linkNode)
        {
            IEnumerable<HtmlNode> spanNodes = linkNode
                .Descendants("span");

            List<string> conditions = new List<string>
            {
                "Nowe",
                "Używane",
                "Uszkodzone"
            };

            foreach (string condition in conditions)
            {
                if (spanNodes.Any(x => x.GetAttributeValue("title", null) == condition))
                {
                    offer.Condition = PrepareText(condition);
                    return;
                }
            }
        }

        private void SetImage(OlxOffer offer, HtmlNode offerNode)
        {
            HtmlNode imageNode = offerNode
                .Element("div")
                ?.Element("div")
                ?.Elements("div")
                ?.First()
                ?.Element("a")
                ?.Element("div")
                ?.Element("div")
                ?.Element("img");

            offer.ImageHref = imageNode?.GetAttributeValue("src", null);
        }

        private void SetPriceAndToNegotiate(OlxOffer offer, HtmlNode offerDataNode)
        {
            HtmlNode priceNode = offerDataNode
                .Elements("p")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "ad-price");

            CheckNodeExists(priceNode, nameof(priceNode));

            offer.Price = PrepareText(priceNode.GetDirectInnerText());

            HtmlNode spanChild = priceNode
                .Elements("span")
                .FirstOrDefault(x => x.FirstChild?.Name == "#text");

            if (spanChild == null) { return; }

            offer.ToNegotiate = PrepareText(spanChild.GetDirectInnerText());
        }
    }
}