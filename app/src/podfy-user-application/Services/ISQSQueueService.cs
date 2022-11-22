namespace podfy_user_application.Service;

public interface ISQSQueueService
{
    Task SendMessageAsync(object item);
}

