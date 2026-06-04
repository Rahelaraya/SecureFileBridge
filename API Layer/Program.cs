using API_Layer.Installers;
using API_Layer.Logging;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Infrastructure_Layer.Configuration;
using Serilog;

SerilogConfiguration.Configure();

var builder = WebApplication.CreateBuilder(args);

// Key Vault configuration - loads RabbitMQ--UserName and RabbitMQ--Password automatically
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUrl = new Uri(builder.Configuration["KeyVault:Url"]!);
    builder.Configuration.AddAzureKeyVault(
        keyVaultUrl,
        new DefaultAzureCredential());
}

builder.Host.UseSerilog();

builder.Services.InstallServicesInAssembly(
    builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


