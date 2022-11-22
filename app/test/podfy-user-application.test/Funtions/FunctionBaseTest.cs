using Amazon.Lambda.Core;
using Moq;
using podfy_user_application.Service;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.test.Funtions
{
    [ExcludeFromCodeCoverage]
    public class FunctionBaseTest
    {
        protected readonly Mock<IUserService> _userService;
        protected readonly Mock<ILambdaContext> _context;
        protected readonly Mock<ILambdaLogger> _logger;
        protected readonly Function _function;

        public FunctionBaseTest()
        {
            _userService = new Mock<IUserService>();
            _context = new Mock<ILambdaContext>();
            _logger = new Mock<ILambdaLogger>();
            _function = new Function();

            _function.userService = _userService.Object;
            _context.Setup(x => x.Logger).Returns(_logger.Object);
        }
    }
}
