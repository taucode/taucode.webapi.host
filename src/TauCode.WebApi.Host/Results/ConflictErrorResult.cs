using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class ConflictErrorResult : ConflictObjectResult
    {
        public ConflictErrorResult(Exception exception, string code = null)
            : base(exception.ToErrorDto(code))
        {
        }

        public ErrorDto Error => (ErrorDto)this.Value;

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}
