namespace BlogProject.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = 500; // Internal Server Error
                    context.Response.ContentType = "application/json";
                    var errorResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "An unexpected error occurred. Please try again later."
                    };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                })
                ;
            });
        }
    }
}
