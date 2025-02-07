using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Contexts;
using TaskManager.Service.Helpers;
using TaskManager.Web.Extensions;
using TaskManager.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Service Extensions
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
if(jwtOptions is not null) builder.Services.AddSingleton(jwtOptions);

builder.Services.AddApplicationServiceExtension();

// Database Register
builder.Services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json","OpenAPI V1");
    });
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
