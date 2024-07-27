using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.SQLiteNet.Migrations;
using SQLite;

namespace JTea.OfferScrappers.WindowsService.Persistence.Migrations
{
    internal class Migration001_Init : BaseMigration
    {
        public override void Up(SQLiteConnection db, bool newDatabase)
        {
            if (newDatabase)
            {
                db.Insert(new ConfigurationEntity
                {
                    CronExpression = "0 0 21 ? * * *"
                });
            }
        }
    }
}