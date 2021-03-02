using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PaymentGateway.API.Middleware
{
    public sealed class EnableBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public EnableBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try { context.Request.EnableBuffering(); } catch { }
            await _next(context);
        }
    }
}
