using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Api
{
    public class DomainExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new DomainErrorDto(e.ErrorCode, e.Message));
            }
        }
    }
}