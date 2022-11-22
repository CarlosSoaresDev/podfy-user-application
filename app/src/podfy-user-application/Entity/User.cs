using Amazon.DynamoDBv2.DataModel;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Entity;

[DynamoDBTable("podfy-user")]
[ExcludeFromCodeCoverage]
public class User
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
}

