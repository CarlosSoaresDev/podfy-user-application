using Microsoft.IdentityModel.Tokens;
using podfy_user_application.Entity;
using podfy_user_application.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace podfy_user_application.Utils
{
    public static class JwtProviderExtension
    {
        private static string secretKey = Environment.GetEnvironmentVariable("USER_SECRET_KEY");

        public static UserTokenResponse GenerateJwtToken(this User user)
        {
            try
            {
                var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("createdAt", user.CreatedAt.ToString("dd/MM/yyyy")),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new UserTokenResponse
                {
                    AccessToken = tokenHandler.WriteToken(token),
                    ExpiresIn = TimeSpan.FromDays(1),
                    TokenType = "bearer"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ClaimsPrincipal ValidateToken(this string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
            try
            {
                return tokenHandler.ValidateToken(authToken, validationParams, out SecurityToken securityToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}