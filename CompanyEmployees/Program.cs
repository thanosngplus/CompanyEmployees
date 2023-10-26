using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var initLogger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
initLogger.Debug("init main");

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

var appLogger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(appLogger);

if (app.Environment.IsProduction())
    app.UseHsts();

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

LogManager.Shutdown();
