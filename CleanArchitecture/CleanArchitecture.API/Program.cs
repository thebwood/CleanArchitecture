using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Infrastructure;
using FluentValidation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(
    Assembly.Load("CleanArchitecture.Application"));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Clean Architecture Address API",
        Description = "A Clean Architecture implementation for Address management with CRUD operations, FluentValidation, and comprehensive middleware"
    });

    // Add XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add CORS - Configure for multiple clients
builder.Services.AddCors(options =>
{
    // React App Policy
    options.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Correlation-ID");
    });

    // Blazor App Policy
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7220", "http://localhost:5220")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Correlation-ID");
    });

    // Combined Policy for Development (allows both React and Blazor)
    options.AddPolicy("DevelopmentClients", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",           // React
                "https://localhost:7220",          // Blazor HTTPS
                "http://localhost:5220")           // Blazor HTTP
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Correlation-ID");
    });

    // Keep AllowAll for testing only
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Add health checks
builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Exception handling (must be first)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Correlation ID for tracking
app.UseMiddleware<CorrelationIdMiddleware>();

// Request logging
app.UseMiddleware<RequestLoggingMiddleware>();

// Security headers
app.UseMiddleware<SecurityHeadersMiddleware>();

// Response compression
app.UseResponseCompression();

// CORS - Use the DevelopmentClients policy (allows both React and Blazor)
app.UseCors("DevelopmentClients");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Address API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
