using FluentMigrator;
using Newtonsoft.Json;
using System.Collections.Generic;
using TauCode.Utils.Extensions;

namespace TauCode.WebApi.Host.Tests.DbMigrations
{
    [Migration(0)]
    public class M0_Baseline : AutoReversingMigration
    {
        public override void Up()
        {
            this.Create.Table("currency")
                .WithColumn("id")
                    .AsGuid()
                    .NotNullable()
                    .PrimaryKey("PK_currency")
                .WithColumn("code")
                    .AsAnsiString()
                    .NotNullable()
                    .Unique("UX_currency_code")
                .WithColumn("name")
                    .AsString()
                    .NotNullable()
                .WithColumn("is_available_to_users")
                    .AsBoolean()
                    .NotNullable();

            this.InsertCurrencies();
        }

        private void InsertCurrencies()
        {
            var json = this.GetType().Assembly.GetResourceText("currencies.json", true);
            var currencies = JsonConvert.DeserializeObject<List<CurrencyDto>>(json);

            foreach (var currency in currencies)
            {
                var row = new
                {
                    id = currency.Id,
                    code = currency.Code,
                    name = currency.Name,
                    is_available_to_users = currency.InitialIsAvailableToUsers,
                };

                this.Insert.IntoTable("currency").Row(row);
            }
        }
    }
}
