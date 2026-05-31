using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Naseejk.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Database Connection ---
builder.Services.AddDbContext<NaseejkDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseOpenIddict();
});

// --- 2. Identity Configuration ---
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<NaseejkDbContext>()
    .AddDefaultTokenProviders();

// --- 3. OpenIddict Configuration ---
builder.Services.AddOpenIddict()

    // the Client Application will be registered in the database, so we need to use the Entity Framework Core to persist it inside my microservice database.
    .AddCore(options =>
    {
        
        options.UseEntityFrameworkCore()
               .UseDbContext<NaseejkDbContext>();
    })

    // define the URL that the mobile or the Website to send username and password to get the access token .
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token");
        
        // allow to sign in with password and email .
        options.AllowPasswordFlow()     
               .AllowRefreshTokenFlow();

        // allow the client application to exchange the access token with another access token that has different scopes or different audiences, this is useful when we want to use the same access token to access multiple microservices with different scopes or audiences.
        options.AllowCustomFlow("urn:ietf:params:oauth:grant-type:token-exchange");

        // The access token is a JWT token, so we need to use the JWT format to generate the access token.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // validate the User on my Business Logic Layer .
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough(); 
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();