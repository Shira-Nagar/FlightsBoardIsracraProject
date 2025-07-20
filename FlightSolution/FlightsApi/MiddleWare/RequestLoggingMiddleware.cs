
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FlightsApi.MiddleWare
{
    

    /// <summary>
    /// Middleware for logging incoming HTTP requests.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">Logger for logging requests.</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to log the incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");
            await _next(context);
        }
    }

}
