using Amazon.DynamoDBv2.DataModel;
using podfy_user_application.Entity;

namespace podfy_user_application.Repository;

public interface IUserRepository
{
    Task<User> FindUserAsync(IEnumerable<ScanCondition> scanConditions);

    Task SaveUserAsync(User user);
}
