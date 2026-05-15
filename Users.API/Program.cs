using System.Text;
using CORE.APP.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users.APP.Domain;

var builder = WebApplication.CreateBuilder(args);

// builder.AddServiceDefaults();

// Add CORS – allows the standalone HTML frontend to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton<ITokenAuthService, TokenAuthService>();
builder.Configuration["SecurityKey"] = "users_microservices_security_key_2026=";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        // Define rules for validating JWT.
        config.TokenValidationParameters = new TokenValidationParameters
        {
            // Use the builder configuration's security key to create a new symmetric security key for verifying the JWT's signature.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKey"] ?? string.Empty)),

            ValidIssuer = builder.Configuration["Issuer"], // get Issuer section's value from appsettings.json
            ValidAudience = builder.Configuration["Audience"], // get Audience section's value from appsettings.json

            // These flags ensure the validation of the JWT.
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

// Add services to the container. IoC (Inversion of Control) Container
// For DbContext Injection
var connectionString = builder.Configuration.GetConnectionString(nameof(UsersDb)); // değişecek
builder.Services.AddDbContext<DbContext, UsersDb>(options => options.UseSqlite(connectionString)); // değişecek

// For Mediator Injection
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    // Define the basic information for your API.
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });

    // Add the JWT Bearer scheme to the Swagger UI so JWT can be tested in requests.
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter your JWT as: "Bearer jwt"
                      Example: "Bearer a1b2c3"
                      """
    });

    // Add the security requirement globally so all endpoints are secured unless specified otherwise.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
var frontendPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "frontend"));

// app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

if (Directory.Exists(frontendPath))
{
    var frontendFiles = new PhysicalFileProvider(frontendPath);

    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = frontendFiles
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = frontendFiles,
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
            ctx.Context.Response.Headers.Pragma = "no-cache";
            ctx.Context.Response.Headers.Expires = "0";
        }
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
