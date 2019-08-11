using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class CreatedOkResult : OkObjectResult
    {
        public CreatedOkResult(string id, string url)
            : base(new CreateResultDto
            {
                InstanceId = id,
                Url = url,
            })
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.CreateResultPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }

    public class CreatedOkResult<T> : OkObjectResult
    {
        public CreatedOkResult(string id, string url, T instance)
            : base(new CreateResultDto<T>
            {
                InstanceId = id,
                Url = url,
                Content = instance,
            })
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.CreateResultPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}
