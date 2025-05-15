using CreateContact.API.Middlewares;
using CreateContact.Application;
using CreateContact.Infrastructure;
using CreateContact.Infrastructure.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApplicationServices(builder.Configuration)
    .ConfigurePersistenceServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync(); //contact-persistence micro service applies this
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Persistency API");
        c.RoutePrefix = string.Empty; // Redireciona a url / para o Swagger
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMetricServer();

app.UseHttpMetrics();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
