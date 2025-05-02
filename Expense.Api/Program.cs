
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Expense.Api.Middleware;
using Expense.Application.Mapping;
using Expense.Application.Services.Implementations;
using Expense.Application.Services.Interfaces;
using Expense.Application.Validators;
using Expense.Common.Configurations;
using Expense.Domain.Interfaces;
using Expense.Domain.Interfaces.Repositories;
using Expense.Infrastructure.Context;
using Expense.Infrastructure.Repositories;
using Expense.Infrastructure.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL 
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// UoW ve Repo
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
builder.Services.AddScoped<IExpenseClaimService, ExpenseClaimService>();
builder.Services.AddScoped<IEftSimulationLogService, EftSimulationLogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAppSession, AppSession>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IFileService, FileService>();


// JwtConfig  
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtConfig>>().Value);

var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>();

// JWT Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ExpenseManagement API",
        Version = "v1"
    });

    options.UseInlineDefinitionsForEnums();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like this: Bearer {token}"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Middleware 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense.Api v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();    

app.UseMiddleware<ExceptionMiddleware>();


app.MapControllers();

app.Run();
