using JTea.OfferScrappers.Abstraction;

namespace JTea.OfferScrappers.Utils
{
    public static class PageSourceProviderFactory
    {
        public static IPageSourceProvider Create(PageSourceProviderType type)
        {
            switch (type)
            {
                case PageSourceProviderType.Selenium:
                    return new SeleniumPageSourceProvider.PageSourceProvider(true);

                case PageSourceProviderType.HtmlAgilityPack:
                    return new PageSourceProvider();

                default:
                    return null;
            }
        }
    }
}