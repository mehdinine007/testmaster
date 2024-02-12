using Azure;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OldCarService.Host.Infrastructure.Middlewares;

public class HttpResponseDelegate
{
    private readonly RequestDelegate _next;

    private readonly IHttpContextAccessor _contextAccessor;

    public HttpResponseDelegate(RequestDelegate next, IHttpContextAccessor contextAccessor)
    {
        _next = next;
        _contextAccessor = contextAccessor;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        //context.Request.EnableBuffering();
        //var response = _contextAccessor.HttpContext.Response;

        //uncomment this line to re-read context.Request.Body stream
        //context.Request.EnableBuffering();


        //context.Response.Headers.TryGetValue("content-length", out var d);
        //var stream = response.Body;
        try
        {

        //using var reader = new StreamReader(context.Response.Body);
        //var s = await reader.ReadToEndAsync();
        }
        catch(Exception x)
        {

        }
        await _next(context);

    }

}

//public class ResponseMaintainerMiddleware
//{
//    private readonly RequestDelegate _next;

//    public ResponseMaintainerMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Request.EnableRewind();
//        await _next.Invoke(context);
//    }
//}