using Microsoft.Extensions.DependencyInjection;
using podfy_user_application.Context;
using podfy_user_application.Repository;
using podfy_user_application.Service;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.IoC;

[ExcludeFromCodeCoverage]
internal static class ConfigureServices
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDynamoContext, DynamoContext>();
        serviceCollection.AddTransient<ISQSContext, SQSContext>();
        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IUserService,UserService>();
        serviceCollection.AddTransient<ISQSQueueService, SQSQueueService>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}

