using Amazon.SQS.Model;

namespace podfy_user_application.Context;

public interface ISQSContext
{
    Task<SendMessageResponse> SendMessageAsync(SendMessageRequest sendMessageRequest);

    Task<GetQueueUrlResponse> GetQueueUrlAsync(string queueName);
}
