using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using podfy_user_application.IoC;
using podfy_user_application.Model;
using podfy_user_application.Service;
using podfy_user_application.Utils;
using podfy_user_application.Validators;
using System.Security.Claims;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace podfy_user_application;

public class Function : HttpHelpers
{
    public IUserService userService { get; set; }

    public Function()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        userService = serviceProvider.GetService<IUserService>();
    }

    public async Task<APIGatewayProxyResponse> SingInFunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var model = JsonSerializer.Deserialize<UserSingInRequest>(request.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var validator = new UserSingInRequestValidator().Validate(model);

            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            var result = await userService.UserSingInAsync(model);

            return result is null ? Unauthorized("Email or password incorrect") : Ok(result);

        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Aconteceu um error => {ex.Message}");
            return InternalServerError(ex.Message);
        }
    }

    public async Task<APIGatewayProxyResponse> SingUpFunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var model = JsonSerializer.Deserialize<UserSingUpRequest>(request.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var validator = new UserSingUpRequestValidator().Validate(model);

            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            return Ok(await userService.UserSingUpAsync(model));
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"An error occurred => {ex.Message}");
            return InternalServerError(ex.Message);
        }
    }

    public async Task<APIGatewayCustomAuthorizerResponse> ValidateTokenHandlerAsync(APIGatewayCustomAuthorizerRequest request, ILambdaContext context)
    {
        var authToken = request.Headers["Authorization"].Split(' ')[1];

        var isValid = authToken.ValidateToken();
        var effect = isValid == null ? "Deny" : "Allow";
        var principalId = isValid == null ? "401" : isValid?.FindFirst(ClaimTypes.Name)?.Value;
        return new APIGatewayCustomAuthorizerResponse()
        {
            PrincipalID = principalId,
            PolicyDocument = new APIGatewayCustomAuthorizerPolicy()
            {
                Statement = new List<APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement>
                {
                    new APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement()
                    {
                        Effect = effect,
                        Resource = new HashSet<string> { "*" },
                        Action = new HashSet<string> { "execute-api:Invoke" }
                    }
                }
            }
        };
    }
}


