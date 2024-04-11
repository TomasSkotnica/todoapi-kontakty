using System.Net;

namespace todoapi.Controllers
{
    public class RequestSetOptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestSetOptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Test with https://localhost:5001/Privacy/?option=Hello
        public async Task Invoke(HttpContext httpContext)
        {
            var option = httpContext.Request.Query["option"];

            if (!string.IsNullOrWhiteSpace(option))
            {
                httpContext.Items["option"] = WebUtility.HtmlEncode(option);
            }

            await _next(httpContext);
        }
    }
}
