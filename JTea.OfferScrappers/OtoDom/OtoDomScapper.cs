using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace JTea.OfferScrappers.OtoDom
{
    public class OtoDomScapper : BaseScrapper
    {
        public OtoDomScapper(string baseUrl, string offerUrl)
            : base(baseUrl, offerUrl)
        {
        }

        protected override List<string> GetOfferAdditionalUrls(HtmlDocument document)
        {
            HtmlNode paginationNode = document.DocumentNode
                .Descendants("div")
                ?.FirstOrDefault(x => x.GetAttributeValue("data-cy", null) == "search-list-pagination");

            if (paginationNode == null) { return new List<string>(); }

            HtmlNode lastSubPageNode = paginationNode.Descendants("ul")
                ?.FirstOrDefault()
                ?.Descendants("li")
                ?.LastOrDefault(x => string.IsNullOrEmpty(x.GetAttributeValue("aria-label", null)));

            CheckNodeExists(lastSubPageNode, nameof(lastSubPageNode));

            int lastSubPage = int.Parse(lastSubPageNode.GetDirectInnerText());

            return CreateSubPagesUrls(lastSubPage);
        }

        protected override string GetOffersCountText(HtmlDocument document)
        {
            HtmlNode searchHeadingNode = document.DocumentNode
                .Descendants("h1")
                .FirstOrDefault(x => x.GetAttributeValue("data-cy", null) == "search-listing.heading");

            CheckNodeExists(searchHeadingNode, nameof(searchHeadingNode));

            HtmlNode totalCountNode = searchHeadingNode.ParentNode
                ?.Element("div")
                ?.Element("div");

            CheckNodeExists(totalCountNode, nameof(totalCountNode));

            return totalCountNode.GetDirectInnerText();
        }

        protected override List<Offer> GetOffersFromDocument(HtmlDocument document)
        {
            HtmlNode offersContainerNode = document.DocumentNode
                .Descendants("div")
                ?.FirstOrDefault(x => x.GetAttributeValue("data-cy", null) == "search.listing.organic");

            CheckNodeExists(offersContainerNode, nameof(offersContainerNode));

            List<HtmlNode> offerNodes = offersContainerNode
                ?.Elements("ul")
                ?.SelectMany(x => x.Elements("li"))
                ?.ToList();

            if (offerNodes == null || offerNodes.Count == 0) { return new List<Offer>(); }

            var offers = new List<Offer>();

            foreach (HtmlNode offerNode in offerNodes)
            {
                Offer offer = GetOfferFromNode(offerNode);
                if (offer == null) { continue; }

                offers.Add(offer);
            }

            return offers;
        }

        private OtoDomOffer GetOfferFromNode(HtmlNode offerNode)
        {
            var result = new OtoDomOffer();

            List<HtmlNode> sections = offerNode
                .Element("article")
                ?.Elements("section")
                ?.ToList();

            HtmlNode mainSection = sections
                ?.FirstOrDefault();

            if (mainSection == null) { return null; }

            result.IsPartOfInvestment = sections?.Count > 1;

            HtmlNode dataNode = mainSection
                .Elements("div")
                ?.LastOrDefault();

            HtmlNode linkNode = dataNode
                ?.Element("a");

            CheckNodeExists(linkNode, nameof(linkNode));

            result.TargetHref = CombinePaths(BaseUrl, linkNode.GetAttributeValue("href", null));

            result.Title = PrepareValue(linkNode.Element("p").GetDirectInnerText());

            SetImageHref(result, mainSection);

            SetPrice(result, dataNode);

            SetLocation(result, dataNode);

            SetSpecification(result, dataNode);

            return result;
        }

        private void SetImageHref(OtoDomOffer offer, HtmlNode mainSection)
        {
            offer.ImageHref = mainSection
                ?.Descendants("img")
                ?.FirstOrDefault()
                ?.GetAttributeValue("src", null);
        }

        private void SetLocation(OtoDomOffer result, HtmlNode dataNode)
        {
            HtmlNode locationNode = dataNode
                ?.Descendants("p")
                ?.FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "advert-card-address");

            CheckNodeExists(locationNode, nameof(locationNode));

            result.Location = PrepareValue(locationNode.GetDirectInnerText());
        }

        private void SetPrice(OtoDomOffer result, HtmlNode dataNode)
        {
            HtmlNode priceNode = dataNode
                ?.Elements("div")
                ?.Where(x => x.GetAttributeValue("data-testid", null) == "listing-item-header")
                ?.FirstOrDefault()
                ?.Element("span");

            CheckNodeExists(priceNode, nameof(priceNode));

            result.Price = PrepareValue(priceNode.GetDirectInnerText());
        }

        private void SetSpecification(OtoDomOffer result, HtmlNode dataNode)
        {
            List<HtmlNode> specificationNodes = dataNode
                ?.Descendants("dd")
                ?.ToList();

            if (specificationNodes == null) { return; }

            result.Specification = string.Join(", ", specificationNodes.Select(x => PrepareValue(x.GetDirectInnerText()?.Trim())));
        }
    }
}