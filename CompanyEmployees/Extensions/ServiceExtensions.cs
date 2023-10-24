namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                // #TODO NOT recommended for production
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()    // WithOrigins("https://example.com")
                        .AllowAnyMethod()       // WithMethods("POST", "GET")
                        .AllowAnyHeader());     // WithHeaders("accept", "content-type")
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });
    }
}
