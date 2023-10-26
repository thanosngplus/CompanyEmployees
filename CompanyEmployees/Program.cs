using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Debug("init main");

try
{

    var builder = WebApplication.CreateBuilder(args);


    // custom extensions
    builder.Services.ConfigureCors();
    builder.Services.ConfigureIISIntegration();
    builder.Services.ConfigureLoggerService();
    builder.Services.ConfigureRepositoryManager();
    builder.Services.ConfigureServiceManager();
    builder.Services.ConfigureSqlContext(builder.Configuration);
    builder.Services.AddControllers()
        .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddControllers(); // ignores View or Pages controllers, they're not required

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage(); // useful for debugging
    else
        app.UseHsts();  // header that forces HTTPS

    app.UseHttpsRedirection();


    app.UseStaticFiles(); // default wwwroot

    // forwards proxy headers to current request, helpful for deployment
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });

    app.UseCors("CorsPolicy");

    app.UseAuthorization();

    // maps controllers to route HTTP requests to the appropriate controller actions
    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
