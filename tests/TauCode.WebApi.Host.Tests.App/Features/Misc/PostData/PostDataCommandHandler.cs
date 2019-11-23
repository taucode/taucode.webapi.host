using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.PostData
{
    public class PostDataCommandHandler : ICommandHandler<PostDataCommand>
    {
        public void Execute(PostDataCommand command)
        {
            throw new System.NotImplementedException();
        }

        public Task ExecuteAsync(PostDataCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}
