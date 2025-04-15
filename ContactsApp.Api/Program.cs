using System.Text;
using ContactsApp.Api.Data;
using ContactsApp.Api.Models;
using ContactsApp.Api.Repositories.Interfaces;
using ContactsApp.Api.Repositories.Impl;
using ContactsApp.Api.Services.Impl;
using ContactsApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthServiceImpl>(); 

builder.Services.AddScoped<IContactService, ContactServiceImpl>();

builder.Services.AddScoped<IContactRepository, ContactRepositoryImpl>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? 
                throw new InvalidOperationException("JWT key is missing"))),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.NameIdentifier
        };
        
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context => 
            {                
                if (context.Principal?.Identity is ClaimsIdentity identity)
                {                    
                    var nameIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                    if (nameIdClaim == null)
                    {
                        var altClaim = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                        if (altClaim != null)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, altClaim.Value));
                        }
                    }
                }
                
                return Task.CompletedTask; 
            },
            OnAuthenticationFailed = context => 
            {
                return Task.CompletedTask;
            },
            OnChallenge = context => 
            {
                return Task.CompletedTask;
            },
            OnMessageReceived = context => 
            {
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Konfiguracja serializacji JSON z obsługą cykli referencji
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ustawienie ReferenceHandler.Preserve zapobiega błędom cykli w grafie obiektów
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        // Ignorowanie wartości null w odpowiedzi JSON
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        // Użycie camelCase dla nazw właściwości
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactsApp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT with Bearer in the format 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Dodaj middleware do logowania żądań
app.Use(async (context, next) =>
{
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();