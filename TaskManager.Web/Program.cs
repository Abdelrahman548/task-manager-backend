using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Contexts;
using TaskManager.Service.Helpers;
using TaskManager.Web.Extensions;
using TaskManager.Web.Middlewares;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

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

// Authentication
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),

             ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token"
    });
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "API_KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter your API Key"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });
});

// Change Model Binding Validation Response
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = new BaseResult<string>
        {
            IsSuccess = false,
            Message = "Validation failed",
            Errors = errors,
            StatusCode = MyStatusCode.BadRequest
        };

        return new BadRequestObjectResult(response);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
