using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class DeletedOkResult : OkObjectResult
    {
        public DeletedOkResult(string id) 
            : base(new DeleteResultDto
            {
                InstanceId = id,
            })
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.DeleteResultPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}
