using CreateContact.API.Middlewares;
using CreateContact.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Persistency API");
        c.RoutePrefix = string.Empty; // Redireciona a url / para o Swagger
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
