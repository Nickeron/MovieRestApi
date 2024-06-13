using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Movies.Api;
using Movies.Api.Mapping;
using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var configurationManager = builder.Configuration;

// Add services to the container.
builder
    .Services.AddAuthentication(configureOptions =>
    {
        configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(bearerOptions =>
    {
        bearerOptions.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configurationManager["Jwt:Key"]!)
            ),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = configurationManager["Jwt:Issuer"],
            ValidAudience = configurationManager["Jwt:Audience"],
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });
builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy(
        AuthConstants.AdminUserPolicyName,
        policyBuilder => policyBuilder.RequireClaim(AuthConstants.AdminUserClaimName, "true")
    )
    .AddPolicy(
        AuthConstants.TrustedUserPolicyName,
        policyBuilder =>
            policyBuilder.RequireAssertion(context =>
                context.User.HasClaim(claim =>
                    claim is { Type: AuthConstants.TrustedUserClaimName, Value: "true" }
                )
                || context.User.HasClaim(claim =>
                    claim is { Type: AuthConstants.AdminUserClaimName, Value: "true" }
                )
            )
    );
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddDatabase(configurationManager["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
