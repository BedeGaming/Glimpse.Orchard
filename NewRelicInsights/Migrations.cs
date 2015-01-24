using Glimpse.Orchard.NewRelicInsights.Models;
using Orchard.Data.Migration;

namespace Glimpse.Orchard.NewRelicInsights
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(NewRelicInsightsSettingsPartRecord).Name, table => table
                    .ContentPartRecord()
                    .Column<string>("InsertKey")
                    .Column<long>("AccountId")
                    .Column<long>("AppId")
                    .Column<int>("BufferSize")
                );

            return 1;
        }
    }
}