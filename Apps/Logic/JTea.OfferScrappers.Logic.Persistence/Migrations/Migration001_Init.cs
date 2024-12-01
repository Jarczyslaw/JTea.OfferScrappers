using JTea.OfferScrappers.Logic.Persistence.Entities;
using JToolbox.DataAccess.L2DB.Migrations;
using LinqToDB;
using LinqToDB.Data;

namespace JTea.OfferScrappers.Logic.Persistence.Migrations
{
    internal class Migration001_Init : BaseMigration
    {
        public override void Up(DataConnection db, bool newDatabase)
        {
            db.CreateTable<ConfigurationEntity>(tableOptions: TableOptions.CheckExistence);
            db.CreateTable<OfferHeaderEntity>(tableOptions: TableOptions.CheckExistence);
            db.CreateTable<OfferEntity>(tableOptions: TableOptions.CheckExistence);
            db.CreateTable<OfferHistoryEntity>(tableOptions: TableOptions.CheckExistence);

            if (newDatabase)
            {
                db.Insert(new ConfigurationEntity
                {
                    CronExpression = "0 0 21 ? * * *",
                    DelayBetweenOffersChecksSeconds = 5,
                    DelayBetweenSubPagesChecksSeconds = 1,
                });
            }
        }
    }
}