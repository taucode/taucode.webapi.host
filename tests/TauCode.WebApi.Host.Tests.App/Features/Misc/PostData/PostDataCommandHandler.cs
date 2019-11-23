using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.PostData
{
    public class PostDataCommandHandler : ICommandHandler<PostDataCommand>
    {
        public void Execute(PostDataCommand command)
        {
            var greeting = $"Hello, {command.UserName}! Your birthday is {command.Birthday:yyyy-MM-dd}.";
            command.SetResult(greeting);
        }

        public Task ExecuteAsync(PostDataCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            this.Execute(command);
            return Task.CompletedTask;
        }
    }
}
