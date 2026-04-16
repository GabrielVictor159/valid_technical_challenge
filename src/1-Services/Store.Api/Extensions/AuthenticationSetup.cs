using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Store.Api.Extensions;

public static class AuthenticationSetup
{
    public static void AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Keycloak:Authority"];
            options.Audience = configuration["Keycloak:Audience"];
            options.RequireHttpsMetadata = configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Keycloak:Authority"]
            };
        });

        services.AddAuthorization();
    }
}