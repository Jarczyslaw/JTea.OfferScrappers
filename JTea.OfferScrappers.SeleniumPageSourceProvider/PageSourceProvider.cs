using JTea.OfferScrappers.Abstraction;
using OpenQA.Selenium.Firefox;
using System;
using System.Threading.Tasks;

namespace JTea.OfferScrappers.SeleniumPageSourceProvider
{
    public class PageSourceProvider : IPageSourceProvider
    {
        private readonly bool _scrollStepByStep;

        public PageSourceProvider(bool scrollStepByStep = false)
        {
            _scrollStepByStep = scrollStepByStep;
        }

        public async Task<string> GetPageSource(string url)
        {
            var options = new FirefoxOptions();
            options.AddArguments("-headless");

            using (var driver = new FirefoxDriver(options))
            {
                driver.Navigate().GoToUrl(url);

                await ScrollStepByStep(driver);

                return (string)driver.ExecuteScript($"return document.documentElement.outerHTML");
            }
        }

        private async Task ScrollStepByStep(FirefoxDriver driver)
        {
            if (!_scrollStepByStep) { return; }

            TimeSpan delay = TimeSpan.FromSeconds(20);
            const int steps = 20;
            TimeSpan stepDelay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds / steps);

            int height = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight;"));
            int stepHeight = height / steps;

            for (int i = 1; i <= steps; i++)
            {
                driver.ExecuteScript($"window.scrollBy(0, {stepHeight});");

                await Task.Delay(stepDelay);
            }
        }
    }
}