using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace UniversityMgmtSystemServerApi.Middlleware
{
    public class ErrorHandler
    {

        private readonly RequestDelegate _next;
        public ErrorHandler(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context,ex);
            }

        }

        public static Task HandleExceptionAsync(HttpContext context, Exception ex) 
        
        
        {
            var code=HttpStatusCode.InternalServerError;
            var result =JsonConvert.SerializeObject(new {error=ex.Message});
            context.Response.ContentType= "application/json";
            context.Response.StatusCode=(int)code;
            return context.Response.WriteAsync(result);

         }



    }
}
