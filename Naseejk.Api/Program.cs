using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Naseejk.Api.Filters;
using Naseejk.core.Common;
using Naseejk.core.Entities;
using Naseejk.core.Interfaces;
using Naseejk.core.Repositories;
using Naseejk.Infrastructure.Data;
using Naseejk.Infrastructure.Repositories;
using Naseejk.Infrastructure.Services;
using Naseejk.Infrastructure.Services.Admins;
using Naseejk.Infrastructure.Services.Admins.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("NaseejkPolicy", policy => {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "https://naseejk-app.vercel.app",
                "https://y-abood.vercel.app",
                "https://y-pink-one-86.vercel.app"
                )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Naseejk_";
});



builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers(options => {
    options.Filters.Add<AuthCookieResultFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddSingleton<ISnowflakeIdGenerator>(new SnowflakeIdGenerator(1));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IExternalAuthProvider, GoogleAuthProvider>();
builder.Services.AddScoped<IExternalAuthProvider, MicrosoftAuthProvider>();
builder.Services.AddScoped<IMicrosoftAuthService, MicrosoftAuthService>();
builder.Services.AddScoped<IMicrosoftProvisioningService, MicrosoftProvisioningService>();
builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.AddScoped<IBasicSeeder, BasicTenantSeeder>();
builder.Services.AddScoped<INaseejkAdmin, NaseejkAdmin>();
builder.Services.AddScoped<NaseejkDataSeeder>();
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<NaseejkDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddDbContextPool<NaseejkDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var tenantSeeder = services.GetRequiredService<IBasicSeeder>();

        await tenantSeeder.SeedAsync();

        var dataSeeder = services.GetRequiredService<NaseejkDataSeeder>();
        await dataSeeder.SeedAllAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error occurred while seeding basic tenant.");
    }
}
app.UseRouting();
app.UseCors("NaseejkPolicy");

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();