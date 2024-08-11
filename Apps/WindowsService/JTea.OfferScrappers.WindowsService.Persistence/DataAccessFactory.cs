using JToolbox.Core.TimeProvider;
using JToolbox.DataAccess.L2DB;

namespace JTea.OfferScrappers.WindowsService.Persistence
{
    public static class DataAccessFactory
    {
        public static DataAccessService Create(ITimeProvider timeProvider)
        {
            DbInitializer initializer = new(timeProvider);
            BaseLocker locker = new();
            return new DataAccessService(initializer, locker);
        }
    }
}