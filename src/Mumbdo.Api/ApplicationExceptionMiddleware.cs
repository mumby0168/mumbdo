using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mumbdo.Application.Exceptions;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Api
{
    public class ApplicationExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ApplicationException e)
            {
                context.Response.StatusCode = (int) e.StatusCode;
                await context.Response.WriteAsJsonAsync(new DomainErrorDto(e.ErrorCode, e.Message));
            }
        }
    }
}