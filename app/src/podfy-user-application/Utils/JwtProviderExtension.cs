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

                var secretKey = "asdv234234^&%&^%&^hjsdfb2%%%"; //Environment.GetEnvironmentVariable("PasswordSecretKey");

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
    }
}