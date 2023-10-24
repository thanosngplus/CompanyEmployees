using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

// custom extensions
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

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
