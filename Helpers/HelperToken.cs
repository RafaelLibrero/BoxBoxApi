using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class HelperToken
{
    private readonly SecretClient _secretClient;
    public KeyVaultSecret Issuer { get; set; }
    public KeyVaultSecret Audience { get; set; }
    public KeyVaultSecret SecretKey { get; set; }

    public HelperToken(SecretClient secretClient)
    {
        _secretClient = secretClient;
        Issuer = _secretClient.GetSecret("Issuer");
        Audience = _secretClient.GetSecret("Audience");
        SecretKey = _secretClient.GetSecret("SecretKey");
    }

    public SymmetricSecurityKey GetKeyToken()
    {
        byte[] data = Encoding.UTF8.GetBytes(SecretKey.Value);
        return new SymmetricSecurityKey(data);
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // Aquí sí validar expiración normalmente
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer.Value,
            ValidAudience = Audience.Value,
            IssuerSigningKey = GetKeyToken()
        };
    }

    public Action<JwtBearerOptions> GetJwtBearerOptions()
    {
        return options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer.Value,
                ValidAudience = Audience.Value,
                IssuerSigningKey = GetKeyToken()
            };
        };
    }

    public Action<AuthenticationOptions> GetAuthenticateSchema()
    {
        return options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        };
    }
}