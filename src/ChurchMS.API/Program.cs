using System.Text;
using ChurchMS.API.Middleware;
using ChurchMS.Application;
using ChurchMS.Domain.Entities;
using ChurchMS.Infrastructure;
using ChurchMS.Persistence;
using ChurchMS.Persistence.Seed;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Layer DI registrations
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

// ASP.NET Identity
builder.Services.AddIdentity<AppUser, AppIdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization Policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.RequireSuperAdmin, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin))
    .AddPolicy(AuthorizationPolicies.RequireCentralAdmin, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.CentralAdmin))
    .AddPolicy(AuthorizationPolicies.RequireChurchAdmin, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.CentralAdmin, AppConstants.Roles.ChurchAdmin))
    .AddPolicy(AuthorizationPolicies.RequireITManager, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.ITManager))
    .AddPolicy(AuthorizationPolicies.RequireSecretary, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.Secretary))
    .AddPolicy(AuthorizationPolicies.RequireTreasurer, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.Treasurer))
    .AddPolicy(AuthorizationPolicies.RequireDepartmentHead, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.DepartmentHead))
    .AddPolicy(AuthorizationPolicies.RequireDepartmentTreasurer, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.Treasurer, AppConstants.Roles.DepartmentTreasurer))
    .AddPolicy(AuthorizationPolicies.RequireTeacher, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.Teacher))
    .AddPolicy(AuthorizationPolicies.RequireEvangelismLeader, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.EvangelismLeader))
    .AddPolicy(AuthorizationPolicies.RequireMultimediaManager, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.MultimediaManager))
    .AddPolicy(AuthorizationPolicies.RequireLogisticsManager, p =>
        p.RequireRole(AppConstants.Roles.SuperAdmin, AppConstants.Roles.ChurchAdmin, AppConstants.Roles.LogisticsManager));

// Controllers
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChurchMS API",
        Version = "v1",
        Description = "Church Management Solution API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    options.AddSecurityRequirement(doc =>
    {
        var scheme = new OpenApiSecuritySchemeReference("Bearer", doc);
        return new OpenApiSecurityRequirement
        {
            { scheme, new List<string>() }
        };
    });
});

// CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware pipeline
app.UseExceptionHandling();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChurchMS API v1"));
}

app.UseHttpsRedirection();
app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Apply migrations and seed data in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await SuperAdminSeedData.SeedSuperAdminAsync(app.Services);

await app.RunAsync();
