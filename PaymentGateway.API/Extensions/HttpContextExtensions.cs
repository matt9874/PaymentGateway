using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetErrorInformation(this HttpContext context)
        {
            string requestBody = context.GetRequestBody().Result;
            
            return $@"
Error.Unhandled exception. TraceId: { context.TraceIdentifier}. 
Remote IP address: {context.Connection.RemoteIpAddress.ToString()}
Local IP address: {context.Connection.LocalIpAddress.ToString()}
Request:
    Scheme: {context.Request.Scheme}
    Host: {context.Request.Host}
    Path: {context.Request.Path}
    Method: {context.Request.Method}
    Query string: {context.Request.QueryString} 
    Body: {requestBody}
Response:
    Status code: {context.Response.StatusCode}
";
        }

        public static async Task<string> GetRequestBody(this HttpContext context)
        {
            HttpRequest request = context.Request;
            string bodyText;
            long streamPosition = request.Body.Position;
            request.Body.Seek(0, SeekOrigin.Begin);

            using (StreamReader reader
                  = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyText = await reader.ReadToEndAsync();
            }

            request.Body.Position = streamPosition;

            return bodyText;
        }
    }
}
