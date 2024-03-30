using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Exceptions;
using APICatalogo.Responses;

namespace APICatalogo.Middlewares
{
    public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = ex.Message
            }.ToString());
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(new ErrorResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = ex.Message
            }.ToString());
        }
    }
}
}