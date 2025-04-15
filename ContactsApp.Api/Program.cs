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
// Usuwamy sztywny URL, aby mozna było go zmienić przez argumenty
// builder.WebHost.UseUrls("http://localhost:5000");

// Dodanie CORS - bardzo permisywna konfiguracja z nazwą polityki
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Authorization");
    });
});

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
        
        // Ważne dla CORS - nie wymaga autentykacji dla OPTIONS (preflight request)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            },
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
            }
        };
        
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

// WAŻNE: CORS MUSI być przed jakimkolwiek routingiem i autoryzacją
// Użycie nazwane polityki CORS
app.UseCors("AllowAll");

// Dodaj middleware do logowania żądań
app.Use(async (context, next) =>
{
    // Obsługa preflight OPTIONS bez przechodzenia dalej do middleware'u
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();