using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Domain.Exceptions;

namespace Infrastructure.Configuration.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class AppExceptionFilterAttribute(ILogger<AppExceptionFilterAttribute> logger) : ExceptionFilterAttribute
    {
        private readonly ILogger<AppExceptionFilterAttribute> _logger = logger;

        public override void OnException(ExceptionContext context)
        {
            if (context != null)
            {
                context.HttpContext.Response.StatusCode = context.Exception switch
                {
                    NotFoundException => ((int)HttpStatusCode.NotFound),
                    ValidationException => ((int)HttpStatusCode.BadRequest),
                    _ => ((int)HttpStatusCode.InternalServerError)
                };

                _logger.LogError(context.Exception, context.Exception.Message);

                var msg = new
                {
                    context.Exception.Message
                };

                context.Result = new ObjectResult(msg);
            }
        }
    }
}
