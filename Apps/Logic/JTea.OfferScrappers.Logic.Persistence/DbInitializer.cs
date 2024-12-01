using JTea.OfferScrappers.Logic.Persistence.Migrations;
using JToolbox.Core.TimeProvider;
using JToolbox.DataAccess.L2DB.Migrations;
using JToolbox.DataAccess.L2DB.SQLite;
using LinqToDB;

namespace JTea.OfferScrappers.Logic.Persistence
{
    public class DbInitializer : BaseSQLiteInitializer
    {
        public DbInitializer(ITimeProvider timeProvider)
            : base(timeProvider)
        {
        }

        public override DataOptions GetDataOptions => new DataOptions().UseSQLiteOfficial();

        protected override List<BaseMigration> Migrations => [new Migration001_Init()];
    }
}