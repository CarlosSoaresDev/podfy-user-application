using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Model;

[ExcludeFromCodeCoverage]
public class UserTokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public TimeSpan ExpiresIn { get; set; }
}

