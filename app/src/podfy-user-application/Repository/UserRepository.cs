using Amazon.DynamoDBv2.DataModel;
using podfy_user_application.Context;
using podfy_user_application.Entity;

namespace podfy_user_application.Repository;

public class UserRepository: IUserRepository
{
    private readonly IDynamoContext _dynamoContext;

    public UserRepository(IDynamoContext dynamoContext)
    {
        _dynamoContext = dynamoContext;
    }

    public async Task<User> FindUserAsync(IEnumerable<ScanCondition> scanConditions) =>
        (await _dynamoContext.Context.ScanAsync<User>(scanConditions, null).GetRemainingAsync()).FirstOrDefault();

    public async Task SaveUserAsync(User user) =>
        await _dynamoContext.Context.SaveAsync(user);
}

