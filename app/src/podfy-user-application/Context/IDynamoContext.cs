using Amazon.DynamoDBv2.DataModel;

namespace podfy_user_application.Context;

public interface IDynamoContext
{
    DynamoDBContext Context { get; }
}

