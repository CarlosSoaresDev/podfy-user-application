using Amazon.SQS.Model;
using podfy_user_application.Context;
using Amazon.Lambda.Core;

namespace podfy_user_application.Service;

public class SQSQueueService : ISQSQueueService
{
    private readonly ISQSContext _sQSContext;
    private readonly ILambdaContext _context;

    public SQSQueueService(ISQSContext sQSContext)
    {
        _sQSContext = sQSContext ?? throw new ArgumentNullException(nameof(sQSContext));
    }

    /// <inheritdoc/>
    public async Task SendMessageAsync(object item)
    {
        var sqsName = Environment.GetEnvironmentVariable("SQS_NAME");

        var getUrl = await _sQSContext.GetQueueUrlAsync(sqsName);

        var queueUrl = getUrl.QueueUrl;
        var messageBody = JsonSerializer.Serialize(item);
        var messageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messageBody
        };

        var messageResponse = await _sQSContext.SendMessageAsync(messageRequest);

        if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
            _context.Logger.Log("[Error]" + "Messager not sended to sqs");
    }
}

