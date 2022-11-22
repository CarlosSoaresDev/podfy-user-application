using Amazon.Lambda.APIGatewayEvents;
using FluentValidation.Results;
using Moq;
using podfy_user_application.Model;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace podfy_user_application.test.Funtions
{
    [ExcludeFromCodeCoverage]
    public class FunctionSingInHandlerTest: FunctionBaseTest
    {
        [Fact(DisplayName = "When sing-in is success, return an object response with status Ok")]
        public async Task Success()
        {
            // Arrange
            var request = new APIGatewayProxyRequest
            {
                Body = "{\"email\":\"carlos@gmail.com\", \"password\":\"123456\"}"
            };

            var userToken = new UserTokenResponse()
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOlsiY2FybG9zIl0sImp0aSI6ImVhNThhNDJiLTgxYzEtNDU2Yy05OTIwLTMwOWYxMTM4MTY2YyIsImlkIjoiMGI2M2I5OWYtOTk3Ni00YTBjLWIzNWQtNzA4ZTU1NTFlZjA3IiwibmFtZSI6ImNhcmxvcyIsImNyZWF0ZWRBdCI6IjIyLzExLzIwMjIiLCJuYmYiOjE2NjkwODYwNjEsImV4cCI6MTY2OTE3MjQ2MSwiaWF0IjoxNjY5MDg2MDYxfQ.7T6PswAr-9P5H2nz4RfM_UZajZBthvZAqhxuS5I-HY4",
                ExpiresIn = TimeSpan.FromDays(1),
                TokenType = "Bearer"
            };

            _userService.Setup(x => x.UserSingInAsync(It.IsAny<UserSingInRequest>())).ReturnsAsync(userToken);

            // Action
            var result = await _function.SingInFunctionHandlerAsync(request, _context.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<APIGatewayProxyResponse>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(userToken.AccessToken, JsonSerializer.Deserialize<UserTokenResponse>(result.Body).AccessToken);
        }

        [Fact(DisplayName = "When data input in sing-in is not valid, return an error messages with status BadRequest")]
        public async Task ErrorValidator()
        {
            // Arrange
            var request = new APIGatewayProxyRequest
            {
                Body = "{\"email\":\"\", \"password\":\"123456\"}"
            };

            // Action
            var result = await _function.SingInFunctionHandlerAsync(request, _context.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<APIGatewayProxyResponse>(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Email field is requied", JsonSerializer.Deserialize<List<ValidationFailure>>(result.Body).First(x => x.PropertyName == "Email").ErrorMessage);
        }

        [Fact(DisplayName = "When user not founded in sing-in, return menssage response with status Unauthorized")]
        public async Task ErrorUserNotFounded()
        {
            // Arrange
            var request = new APIGatewayProxyRequest
            {
                Body = "{\"email\":\"carlos@gmail.com\", \"password\":\"123456\"}"
            };

            UserTokenResponse? userToken = null;

            _userService.Setup(x => x.UserSingInAsync(It.IsAny<UserSingInRequest>())).ReturnsAsync(userToken);

            // Action
            var result = await _function.SingInFunctionHandlerAsync(request, _context.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<APIGatewayProxyResponse>(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Email or password incorrect", JsonSerializer.Deserialize<string>(result.Body));
        }

        [Fact(DisplayName = "When an exception happens in sing-in json parse, return menssage response with status InternalServerError")]
        public async Task ErrorServiceException()
        {
            // Arrange
            var messageError = "'p' is an invalid start of a property name. Expected a '\"'. Path: $ | LineNumber: 0 | BytePositionInLine: 29.";

            var request = new APIGatewayProxyRequest
            {
                Body = "{\"email\":\"carlos@gmail.com\", password:\"123456\"}"
            };

            // Action
            var result = await _function.SingInFunctionHandlerAsync(request, _context.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<APIGatewayProxyResponse>(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(messageError, JsonSerializer.Deserialize<string>(result.Body));
        }

        [Fact(DisplayName = "When an exception happens in sing-in service, return menssage response with status InternalServerError")]
        public async Task ErrorJsonParseException()
        {
            // Arrange
            var messageError = "Um error aconteceu no serviço";

            var request = new APIGatewayProxyRequest
            {
                Body = "{\"email\":\"carlos@gmail.com\", \"password\":\"123456\"}"
            };

            _userService.Setup(x => x.UserSingInAsync(It.IsAny<UserSingInRequest>())).Throws(() => new Exception("Um error aconteceu no serviço"));

            // Action
            var result = await  _function.SingInFunctionHandlerAsync(request, _context.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<APIGatewayProxyResponse>(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(messageError, JsonSerializer.Deserialize<string>(result.Body));
        }
    }
}
