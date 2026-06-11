namespace API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseApi(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            return app;
        }
    }
}