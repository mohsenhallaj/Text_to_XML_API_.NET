# A high-performance REST API built with ASP.NET Core, designed to convert structured text into XSD-validated XML.Includes AES/RSA encryption, SQLite-based XML storage, background job processing via Hangfire + Redis, Swagger UI with API key authentication, and CI/CD pipeline integration.
## Objective
1. Convert structured plain text to XML
2. AES & RSA encryption/decryption of XML
3. XML schema validation via XSD
4. Background XML processing using Hangfire + Redis
5. Swagger UI endpoint
6. API Key authentication middleware
7. SQLite-based XML file storage
8. CI/CD deployment to Azure App Service (via GitHub)

# Tech Stack
ASP.NET Core 9
SQLite (EF Core)
Serilog (logging)
Swagger / OpenAPI
Hangfire + Redis (background processing)
Custom middleware (API key)
GitHub Actions (CI/CD)

# API Endpoints
POST   /api/convert             → Convert text to XML (sync)  
POST   /api/encrypt             → Encrypt XML (AES or RSA)  
POST   /api/decrypt             → Decrypt XML (AES or RSA)  
POST   /api/validate            → Validate XML against XSD  
GET    /health                  → Health check 

Note: All routes require an API key in the X-API-KEY header

# Requirements
.NET 9 SDK
SQLite
Redis (running on localhost:6379)
Visual Studio or VS Code
Azure account (for deployment)

# CI/CD & Deployment
1. Connected to GitHub repository with Actions
2. Auto-deployment to Azure App Service
3. Azure URL (live Swagger UI): https://dotnet-xml-api-mohsenhallaj-a4f4cacmdxe4hud8.spaincentral-01.azurewebsites.net

# Testing & Logs
Local debug: dotnet run
Logging: Serilog output to console + file (Logs/app.log)
Swagger UI opens automatically on localhost
   
