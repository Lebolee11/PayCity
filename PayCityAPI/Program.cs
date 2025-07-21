// payCityUtilitiesApp.Api/Program.cs
using Microsoft.EntityFrameworkCore;
using payCityUtilitiesApp.Api.Data;
using payCityUtilitiesApp.Api.Repositories.Interfaces;
using payCityUtilitiesApp.Api.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using payCityUtilitiesApp.Api.Services.Interfaces;
using payCityUtilitiesApp.Api.Services.Implementations; // For Swagger security
/*using payCityUtilitiesApp.Api.Services.Interfaces; // Will be added later
using payCityUtilitiesApp.Api.Services.Implementations; // Will be added later*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // <--- CHANGED THIS

// Register Repositories for Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Add other repositories here as you create them:
// builder.Services.AddScoped<IFineRepository, FineRepository>();
// builder.Services.AddScoped<IMeterRepository, MeterRepository>();
// builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();


// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


// Configure Swagger/OpenAPI for API Documentation and Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "payCity Utilities API", Version = "v1" });

    // Add JWT Authorization to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add CORS if your frontend is on a different domain
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin() // Or specify specific origins like .WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "payCity Utilities API v1");
    });
}

// Since we created the project with --no-https, we'll add it back manually here.
// However, if running on localhost only, you might defer this to later.
// For production, you MUST use HTTPS.
// app.UseHttpsRedirection(); // Uncomment for HTTPS

app.UseRouting(); // Important for CORS and endpoint routing

app.UseCors(); // Apply CORS policy

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();