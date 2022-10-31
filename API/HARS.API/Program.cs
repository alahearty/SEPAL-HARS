using Admin.Infrastructure;
using HARS.API.Utilities;
using HARS.Shared.Infrastructure.Bootstrapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var applicationConfiguration = builder.Configuration.ExtractCoreApplicationSettings(new LoggerFactory());
applicationConfiguration.WebRootPath = builder.Environment.WebRootPath;

var bootstrapper = new Bootstrapper(applicationConfiguration);
bootstrapper.RegisterModule<AdminModule>();

builder.Services.AddScoped<IApplication>((_) => bootstrapper.Application);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

Task.WaitAll(bootstrapper.AddApplicationServiceAsync(app.Services));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

