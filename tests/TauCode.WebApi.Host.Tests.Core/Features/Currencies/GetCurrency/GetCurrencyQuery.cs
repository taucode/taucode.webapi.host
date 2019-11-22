using TauCode.Cqrs.Queries;
using TauCode.Domain.Identities;
using TauCode.WebApi.Host.Queries;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency
{
    public class GetCurrencyQuery : Query<GetCurrencyQueryResult>, ICodedEntityQuery
    {
        public CurrencyId Id { get; set; }
        public string Code { get; set; }

        IdBase ICodedEntityQuery.GetId() => Id;
        string ICodedEntityQuery.GetCode() => Code;
        string ICodedEntityQuery.GetCodePropertyName() => nameof(Code);
    }
}
