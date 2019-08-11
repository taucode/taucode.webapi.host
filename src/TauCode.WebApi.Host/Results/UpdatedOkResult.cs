using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class UpdatedOkResult : OkObjectResult
    {
        public UpdatedOkResult(string id, string url)
            : base(new UpdateResultDto
            {
                InstanceId = id,
                Url = url,
            })
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.UpdateResultPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }

    public class UpdatedOkResult<T> : OkObjectResult
    {
        public UpdatedOkResult(string id, string url, T instance)
            : base(new UpdateResultDto<T>
            {
                InstanceId = id,
                Url = url,
                Content = instance,
            })
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.UpdateResultPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}
