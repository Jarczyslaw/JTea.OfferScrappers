using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace JTea.OfferScrappers.OtoMoto
{
    public class OtoMotoScrapper : Scrapper
    {
        public OtoMotoScrapper(string offerUrl)
            : base("https://www.otomoto.pl/", offerUrl)
        {
        }

        public OtoMotoScrapper(string baseUrl, string offerUrl)
            : base(baseUrl, offerUrl)
        {
        }

        public override ScrapperType Type => ScrapperType.OtoMoto;

        protected override List<string> GetOfferAdditionalUrls(HtmlDocument document)
        {
            HtmlNode paginationNode = document.DocumentNode
                .Descendants("ul")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "pagination-list");

            if (paginationNode == null) { return new List<string>(); }

            HtmlNode lastSubPageNode = paginationNode
                .Descendants("li")
                .LastOrDefault(x => x.GetAttributeValue("data-testid", null) == "pagination-list-item")
                ?.Descendants("span")
                ?.FirstOrDefault();

            CheckNodeExists(lastSubPageNode, nameof(lastSubPageNode));

            int lastSubPage = int.Parse(lastSubPageNode.GetDirectInnerText());

            return CreateSubPagesUrls(lastSubPage);
        }

        protected override string GetOffersCountText(HtmlDocument document)
        {
            HtmlNode totalCountNode = document.DocumentNode
                .Descendants("div")
                .FirstOrDefault(x => x.GetAttributeValue("aria-label", null) == "Results header")
                ?.Element("div")
                ?.Element("p");

            if (totalCountNode == null) { return null; }

            return PrepareText(totalCountNode.InnerText);
        }

        protected override List<Offer> GetOffersFromDocument(HtmlDocument document)
        {
            HtmlNode searchResultsNode = document.DocumentNode
                .Descendants("div")
                .FirstOrDefault(x => x.GetAttributeValue("data-testid", null) == "search-results");

            if (searchResultsNode == null) { return new List<Offer>(); }

            List<HtmlNode> offerNodes = searchResultsNode
                .Elements("div")
                .Where(x => x.GetAttributeValue("role", null) == null
                    && x.GetAttributeValue("id", null) == null
                    && x.Element("article") != null)
                .ToList();

            var offers = new List<Offer>();

            foreach (HtmlNode offerNode in offerNodes)
            {
                Offer offer = GetOfferFromNode(offerNode);
                if (offer == null) { continue; }

                offers.Add(offer);
            }

            return offers;
        }

        private Offer GetOfferFromNode(HtmlNode offerNode)
        {
            var result = new OtoMotoOffer();

            HtmlNode sectionNode = offerNode
                .Element("article")
                ?.Element("section");

            CheckNodeExists(sectionNode, nameof(sectionNode));

            List<HtmlNode> divNodes = sectionNode
                .Elements("div")
                .ToList();

            HtmlNode linkDivNode = divNodes.ElementAtOrDefault(1);

            CheckNodeExists(linkDivNode, nameof(linkDivNode));

            SetTargetHrefAndTitle(result, linkDivNode);

            SetSpecification(result, linkDivNode);

            SetFeaturesLocationAndDate(result, divNodes);

            SetImageHref(result, divNodes);

            SetPrice(result, divNodes);

            return result;
        }

        private void SetFeaturesLocationAndDate(OtoMotoOffer offer, List<HtmlNode> divNodes)
        {
            HtmlNode featureDivNode = divNodes.ElementAtOrDefault(2);

            CheckNodeExists(featureDivNode, nameof(featureDivNode));

            List<HtmlNode> dlNodes = featureDivNode
                .Elements("dl")
                .ToList();

            HtmlNode featureNode = dlNodes.ElementAtOrDefault(0);

            if (featureNode == null) { return; }

            offer.Features = string.Join(", ",
                featureNode.Elements("dd").Select(x => PrepareText(x.GetDirectInnerText())));

            HtmlNode locationAndDateNode = dlNodes.ElementAtOrDefault(1);

            if (featureNode == null) { return; }

            offer.LocationAndDate = string.Join(", ",
                locationAndDateNode.Descendants("p").Select(x => PrepareText(x.GetDirectInnerText())));
        }

        private void SetImageHref(OtoMotoOffer offer, List<HtmlNode> divNodes)
        {
            HtmlNode imageDivNode = divNodes.ElementAtOrDefault(0);

            CheckNodeExists(imageDivNode, nameof(imageDivNode));

            offer.ImageHref = imageDivNode
                .Descendants("img")
                .FirstOrDefault()
                ?.GetAttributeValue("src", null);
        }

        private void SetPrice(OtoMotoOffer offer, List<HtmlNode> divNodes)
        {
            HtmlNode priceDivNode = divNodes.ElementAtOrDefault(3);

            CheckNodeExists(priceDivNode, nameof(priceDivNode));

            HtmlNode priceNode = priceDivNode
                .Descendants("h3")
                ?.FirstOrDefault();

            CheckNodeExists(priceNode, nameof(priceNode));

            HtmlNode currencyNode = priceDivNode
                .Descendants("p")
                ?.FirstOrDefault();

            CheckNodeExists(currencyNode, nameof(currencyNode));

            offer.Price = PrepareText(priceNode.GetDirectInnerText() + " " + currencyNode.GetDirectInnerText());
        }

        private void SetSpecification(OtoMotoOffer offer, HtmlNode linkDivNode)
        {
            HtmlNode specificationNode = linkDivNode
                .Element("p");

            CheckNodeExists(specificationNode, nameof(specificationNode));

            offer.Specification = PrepareText(specificationNode.GetDirectInnerText());
        }

        private void SetTargetHrefAndTitle(OtoMotoOffer offer, HtmlNode linkDivNode)
        {
            HtmlNode linkNode = linkDivNode
                .Descendants("a")
                .FirstOrDefault();

            CheckNodeExists(linkNode, nameof(linkNode));

            offer.TargetHref = linkNode.GetAttributeValue("href", null);
            offer.Title = PrepareText(linkNode.GetDirectInnerText());
        }
    }
}