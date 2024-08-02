using HtmlAgilityPack;
using JTea.OfferScrappers.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JTea.OfferScrappers
{
    public abstract class Scrapper
    {
        protected Scrapper(string baseUrl, string offerUrl)
        {
            BaseUrl = baseUrl;
            OfferUrl = offerUrl;
        }

        public string BaseUrl { get; }

        public string FullOfferUrl => CombinePaths(BaseUrl, OfferUrl);

        public string OffersCountText { get; set; }

        public string OfferUrl { get; }

        public abstract ScrapperType Type { get; }

        public static string CombinePaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0) { return null; }

            return string.Join("/", paths.Select(x => x.Trim('/')));
        }

        public virtual async Task<List<Offer>> Scrap(ScrapperConfiguration configuration)
        {
            configuration = PrepareConfiguration(configuration);

            var result = new List<Offer>();

            HtmlDocument document = await GetDocument(FullOfferUrl, configuration, 1);

            OffersCountText = GetOffersCountText(document);

            result = AppendOffersFromDocument(result, document);

            List<Offer> limitedOffers = CheckOffersLimit(result, configuration);
            if (limitedOffers != null) { return limitedOffers; }

            if (!configuration.CheckSubpages) { return result; }

            List<string> offerAdditionalUrls = GetOfferAdditionalUrls(document);

            for (int i = 0; i < offerAdditionalUrls.Count; i++)
            {
                await Task.Delay(configuration.DelayBetweenSubPagesChecks);

                string offerAdditionalUrl = offerAdditionalUrls[i];

                document = await GetDocument(offerAdditionalUrl, configuration, i + 2);

                result = AppendOffersFromDocument(result, document);

                limitedOffers = CheckOffersLimit(result, configuration);
                if (limitedOffers != null) { return limitedOffers; }
            }

            return result;
        }

        protected List<Offer> AppendOffersFromDocument(List<Offer> result, HtmlDocument document)
        {
            List<Offer> offers = GetOffersFromDocument(document);
            result.AddRange(offers);

            return RemoveDuplicates(result);
        }

        protected void CheckNodeExists(HtmlNode node, string nodeName)
        {
            if (node == null) { throw new NodeNotExistsException(nodeName); }
        }

        protected List<string> CreateSubPagesUrls(int lastSubPage)
        {
            var subPagesUrls = new List<string>();

            string argument = FullOfferUrl.Contains("?")
                ? "&page="
                : "?page=";

            for (int i = 2; i <= lastSubPage; i++)
            {
                subPagesUrls.Add(FullOfferUrl + $"{argument}{i}");
            }

            return subPagesUrls;
        }

        protected async Task<HtmlDocument> GetDocument(string path, ScrapperConfiguration configuration, int pageIndex)
        {
            HtmlDocument document = new HtmlDocument();
            string pageSource = await configuration.PageSourceProvider.GetPageSource(path);
            document.LoadHtml(pageSource);

            TryLogPageSource(pageSource, configuration, pageIndex);

            return document;
        }

        protected abstract List<string> GetOfferAdditionalUrls(HtmlDocument document);

        protected abstract string GetOffersCountText(HtmlDocument document);

        protected abstract List<Offer> GetOffersFromDocument(HtmlDocument document);

        protected string PrepareText(string value)
        {
            if (string.IsNullOrEmpty(value)) { return value; }

            return value.Replace("&nbsp;", " ")
                .Trim();
        }

        private List<Offer> CheckOffersLimit(List<Offer> offers, ScrapperConfiguration configuration)
        {
            if (configuration.OffersLimit == null) { return null; }

            if (offers.Count > configuration.OffersLimit.Value)
            {
                return offers.Take(configuration.OffersLimit.Value)
                    .ToList();
            }

            return null;
        }

        private ScrapperConfiguration PrepareConfiguration(ScrapperConfiguration configuration)
        {
            configuration = configuration ?? new ScrapperConfiguration();
            configuration.PageSourceProvider = configuration.PageSourceProvider ?? new PageSourceProvider();

            return configuration;
        }

        private List<Offer> RemoveDuplicates(List<Offer> offers)
            => offers.GroupBy(x => x.TargetHref)
                .Select(x => x.First())
                .ToList();

        private void TryLogPageSource(string pageSource, ScrapperConfiguration configuration, int pageIndex)
        {
            if (string.IsNullOrEmpty(configuration.PageSourceLogPath)
                || pageIndex < 1) { return; }

            Directory.CreateDirectory(configuration.PageSourceLogPath);

            string logPath = Path.Combine(configuration.PageSourceLogPath, $"page_{pageIndex}.txt");

            File.WriteAllText(logPath, pageSource);
        }
    }
}