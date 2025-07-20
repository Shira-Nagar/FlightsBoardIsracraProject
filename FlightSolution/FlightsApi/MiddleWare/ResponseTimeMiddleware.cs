using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
namespace FlightsApi.MiddleWare
{ 
    /// <summary>
    /// Middleware for measuring and logging the response time of HTTP requests.
    /// </summary>
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseTimeMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTimeMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">Logger for logging response times.</param>
        public ResponseTimeMiddleware(RequestDelegate next, ILogger<ResponseTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to measure and log the response time for the current HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation($"Request [{context.Request.Method}] {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

}

