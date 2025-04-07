namespace TaskManagementSystem.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //catch (CustomException ex)
            //{
            //    context.Response.StatusCode = ex.StatusCode;
            //    _logger.LogWarning(ex, ex.Message);
            //    await context.Response.WriteAsJsonAsync(new { statusCode = ex.StatusCode, message = ex.Message });
            //}
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                _logger.LogError(ex, "Unhandled exception");
                await context.Response.WriteAsJsonAsync(new { statusCode = 500, message = "An unexpected error occurred." });
            }
        }
    }

}
