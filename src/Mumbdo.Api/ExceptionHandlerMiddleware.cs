using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mumbdo.Application.Exceptions;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using ApplicationException = Mumbdo.Application.Exceptions.ApplicationException;

namespace Mumbdo.Api
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }
        
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ApplicationException e)
            {
                _logger.LogError($"Application exception thrown, message: {e.Message}", e);
                context.Response.StatusCode = (int) e.StatusCode;
                await context.Response.WriteAsJsonAsync(new MumbdoErrorDto(e.ErrorCode, e.Message));
            }
            catch (DomainException e)
            {
                _logger.LogError($"Domain exception thrown, message: {e.Message}", e);
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new MumbdoErrorDto(e.ErrorCode, e.Message));
            }
            catch (MumbdoNotAuthorisedException e)
            {
                _logger.LogError("Not authorised exception thrown", e);
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            }
            catch (Exception e)
            {
                _logger.LogError("Unhandled error", e);
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(e.ToString());
            }
            
        }
    }
}