using System.Threading.Tasks;

namespace JTea.OfferScrappers.Abstraction
{
    public interface IPageSourceProvider
    {
        Task<string> GetPageSource(string url);
    }
}