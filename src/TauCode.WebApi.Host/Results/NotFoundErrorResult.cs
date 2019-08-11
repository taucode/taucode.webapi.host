using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class NotFoundErrorResult : NotFoundObjectResult
    {
        public NotFoundErrorResult(Exception exception)
            : base(new ErrorDto
            {
                Code = exception.GetType().FullName,
                Message = exception.Message,
            })
        {
            // todo: check if exception is not null
        }

        public ErrorDto Error => (ErrorDto)this.Value;

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}
