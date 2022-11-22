using Amazon.SQS;
using Amazon.SQS.Model;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Context;

[ExcludeFromCodeCoverage]
public class SQSContext : ISQSContext
{
    public async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest sendMessageRequest)
    {
        return await GetSQSClient().SendMessageAsync(sendMessageRequest);
    }

    public async Task<GetQueueUrlResponse> GetQueueUrlAsync(string queueName)
    {
        return await GetSQSClient().GetQueueUrlAsync(queueName);
    }

    private AmazonSQSClient GetSQSClient()
    {
        if (Debugger.IsAttached)
            return new AmazonSQSClient(Amazon.RegionEndpoint.USEast1);
        else
           return new AmazonSQSClient(Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), Amazon.RegionEndpoint.USEast1);
    }
}
