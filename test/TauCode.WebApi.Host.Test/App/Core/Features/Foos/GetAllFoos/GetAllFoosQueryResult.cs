using System.Collections.Generic;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetAllFoos
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
