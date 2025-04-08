using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.Redis;
using TextToXmlApiNet.Middleware;
using TextToXmlApiNet.Services;
using TextToXmlApiNet.Services.AesImpl;
using TextToXmlApiNet.Services.Rsa;
using TextToXmlApiNet.Data;
using Hangfire.Redis.StackExchange;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Serilog setup BEFORE builder.Build()
// -----------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: builder.Configuration["Logging:File:Path"] ?? "Logs/app.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Tell .NET to use Serilog

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add controllers and XML formatting
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

// Register EF Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=Data/XmlStorage.db"));

// Hangfire with Redis for async job processing
builder.Services.AddHangfire(config =>
    config.UseRedisStorage("localhost:6379"));
builder.Services.AddHangfireServer();

// Swagger/OpenAPI support with XML docs and API key auth
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

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

// Application services
builder.Services.AddScoped<IAesEncryptionService, AesEncryptionService>();
builder.Services.AddSingleton<IRsaEncryptionService, RsaEncryptionService>();
builder.Services.AddScoped<FieldValidationService>();
builder.Services.AddScoped<XmlValidationService>();
builder.Services.AddScoped<XmlBackgroundService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    Process.Start(new ProcessStartInfo
    {
        FileName = "http://localhost:5211/swagger",
        UseShellExecute = true
    });
}

// Hangfire Dashboard
app.UseHangfireDashboard("/jobs");

// API key middleware
app.UseMiddleware<ApiKeyMiddleware>();

// Routing
app.UseAuthorization();
app.MapControllers();

// Run
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
