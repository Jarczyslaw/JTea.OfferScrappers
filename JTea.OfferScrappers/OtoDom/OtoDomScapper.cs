using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace JTea.OfferScrappers.OtoDom
{
    public class OtoDomScapper : Scrapper
    {
        public OtoDomScapper(string offerUrl)
            : base("https://www.otodom.pl/", offerUrl)
        {
        }

        public OtoDomScapper(string baseUrl, string offerUrl)
            : base(baseUrl, offerUrl)
        {
        }

        public override ScrapperType Type => ScrapperType.OtoDom;

        protected override List<string> GetOfferAdditionalUrls(HtmlDocument document)
        {
            HtmlNode paginationNode = document.DocumentNode
                .Descendants("div")
                .FirstOrDefault(x => x.GetAttributeValue("data-cy", null) == "search-list-pagination");

            if (paginationNode == null) { return new List<string>(); }

            HtmlNode lastSubPageNode = paginationNode
                .Descendants("ul")
                .FirstOrDefault()
                ?.Descendants("li")
                ?.LastOrDefault(x => string.IsNullOrEmpty(x.GetAttributeValue("aria-label", null)));

            if (lastSubPageNode == null) { return new List<string>(); }

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
                .Element("div")
                ?.Element("span");

            CheckNodeExists(totalCountNode, nameof(totalCountNode));

            return totalCountNode.GetDirectInnerText();
        }

        protected override List<Offer> GetOffersFromDocument(HtmlDocument document)
        {
            HtmlNode offersContainerNode = document.DocumentNode
                .Descendants("div")
                .FirstOrDefault(x => x.GetAttributeValue("data-cy", null) == "search.listing.organic");

            if (offersContainerNode == null) { return new List<Offer>(); }

            List<HtmlNode> offerNodes = offersContainerNode
                .Elements("ul")
                .SelectMany(x => x.Elements("li"))
                .ToList();

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
                .LastOrDefault();

            HtmlNode linkNode = dataNode?.Element("a");

            CheckNodeExists(linkNode, nameof(linkNode));

            result.TargetHref = CombinePaths(BaseUrl, linkNode.GetAttributeValue("href", null));

            HtmlNode titleNode = linkNode.Element("p");

            CheckNodeExists(titleNode, nameof(titleNode));

            result.Title = PrepareText(titleNode.GetDirectInnerText());

            SetImageHref(result, mainSection);

            SetPrice(result, dataNode);

            SetLocation(result, dataNode);

            SetSpecification(result, dataNode);

            return result;
        }

        private void SetImageHref(OtoDomOffer offer, HtmlNode mainSection)
        {
            offer.ImageHref = mainSection
                .Descendants("img")
                .FirstOrDefault()
                ?.GetAttributeValue("src", null);
        }

        private void SetLocation(OtoDomOffer result, HtmlNode dataNode)
        {
            HtmlNode locationNode = dataNode
                .Elements("div")
                .ToList()
                .ElementAtOrDefault(1)
                ?.Element("p");

            CheckNodeExists(locationNode, nameof(locationNode));

            result.Location = PrepareText(locationNode.GetDirectInnerText());
        }

        private void SetPrice(OtoDomOffer result, HtmlNode dataNode)
        {
            HtmlNode priceNode = dataNode
                .Element("div")
                ?.Element("span");

            CheckNodeExists(priceNode, nameof(priceNode));

            result.Price = PrepareText(priceNode.GetDirectInnerText());
        }

        private void SetSpecification(OtoDomOffer result, HtmlNode dataNode)
        {
            List<HtmlNode> specificationNodes = dataNode
                .Descendants("dd")
                .ToList();

            if (specificationNodes == null) { return; }

            result.Specification = string.Join(", ", specificationNodes.Select(x => PrepareText(x.GetDirectInnerText()?.Trim())));
        }
    }
}