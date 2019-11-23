using System;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.PostData
{
    public class PostDataCommand : Command<string>
    {
        public string UserName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
