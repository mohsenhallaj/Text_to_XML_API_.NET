using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TextToXmlApiNet.Middleware;
using TextToXmlApiNet.Services;
using TextToXmlApiNet.Services.AesImpl;
using TextToXmlApiNet.Services.Rsa;
using TextToXmlApiNet.Data;
using Serilog;
using Hangfire;
using Hangfire.Redis.StackExchange;

var builder = WebApplication.CreateBuilder(args);


// Serilog setup BEFORE builder.Build()

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: builder.Configuration["Logging:File:Path"] ?? "Logs/app.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Use AppContext.BaseDirectory for test compatibility
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Add controllers and XML formatting
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

// EF Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=Data/XmlStorage.db"));

// Skip Hangfire/Redis setup if in test mode
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddHangfire(config =>
    {
        config.UseRedisStorage("localhost:6379");
    });
    builder.Services.AddHangfireServer();
    builder.Services.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
}

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TextToXmlApiNet",
        Version = "1.0",
        Description = @"This REST API allows users to:
- Convert structured text into validated XML  
- Encrypt and decrypt XML using AES or RSA  
- Validate XML against an XSD schema  
- Authenticated access using API keys  
- Store the generated XML files in SQLite  
- Process XML in the background using Redis + Hangfire

All routes require an API key in the Authorization header."
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter your API key. Example: super-secret-123",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// App Services
builder.Services.AddScoped<IAesEncryptionService, AesEncryptionService>();
builder.Services.AddSingleton<IRsaEncryptionService, RsaEncryptionService>();
builder.Services.AddScoped<FieldValidationService>();
builder.Services.AddScoped<XmlValidationService>();
builder.Services.AddScoped<XmlBackgroundService>();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextToXmlApiNet v1");
    c.RoutePrefix = "swagger";
});

// Optional: Auto-open Swagger UI
try
{
    Process.Start(new ProcessStartInfo
    {
        FileName = "http://localhost:5211/swagger",
        UseShellExecute = true
    });
}
catch (Exception ex)
{
    Console.WriteLine("⚠ Failed to auto-launch Swagger UI: " + ex.Message);
}

// Only show Hangfire Dashboard if not Testing
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHangfireDashboard("/jobs");
}

app.UseAuthorization();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseMiddleware<ApiKeyMiddleware>();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

try
{
    Log.Information("Starting application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Needed for integration test project
public partial class Program { }
