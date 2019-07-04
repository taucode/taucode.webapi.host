using System.Collections.Generic;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetAllFoos
{
    public class GetAllFoosQueryResult
    {
        public IList<FooDto> Items { get; set; }
        public class FooDto
        {
            public FooId Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
