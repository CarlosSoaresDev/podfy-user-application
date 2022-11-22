using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Model;

[ExcludeFromCodeCoverage]
public class UserSingInRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
