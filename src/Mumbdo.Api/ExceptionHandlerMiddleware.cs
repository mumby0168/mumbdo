using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using ApplicationException = Mumbdo.Application.Exceptions.ApplicationException;

namespace Mumbdo.Api
{
    public class ExceptionHandlerMiddleware : IMiddleware
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
                await context.Response.WriteAsJsonAsync(new MumbdoErrorDto(e.ErrorCode, e.Message));
            }
            catch (DomainException e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new MumbdoErrorDto(e.ErrorCode, e.Message));
            }
            
        }
    }
}