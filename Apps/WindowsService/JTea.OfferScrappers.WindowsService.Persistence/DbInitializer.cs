using JTea.OfferScrappers.WindowsService.Persistence.Migrations;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.DataAccess.SQLiteNet.Migrations;
using System.Reflection;

namespace JTea.OfferScrappers.WindowsService.Persistence
{
    public class DbInitializer : BaseInitializer
    {
        protected override Assembly EntitiesAssembly => GetType().Assembly;

        protected override List<BaseMigration> Migrations => [new Migration001_Init()];
    }
}