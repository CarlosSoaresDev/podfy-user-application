using Amazon.Lambda.Core;
using podfy_user_application.Model;

namespace podfy_user_application.Service;

public interface IUserService
{
    Task<UserTokenResponse> UserSingInAsync(UserSingInRequest userLoginRequest);
    Task<UserTokenResponse> UserSingUpAsync(UserSingUpRequest userLoginRequest);
}
