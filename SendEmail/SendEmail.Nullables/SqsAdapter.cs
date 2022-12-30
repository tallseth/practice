using System.Text.Json;
using Amazon.Lambda.SQSEvents;

namespace SendEmail.Nullables;

public class SqsAdapter<T>
{
    private readonly IMessageHandler<T> _messageHandler;

    public SqsAdapter(IMessageHandler<T> messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task<SQSBatchResponse> Adapt(SQSEvent sqsEvent)
    {
        var response = new SQSBatchResponse();
    
        foreach(var message in sqsEvent.Records)
        {
            var value = JsonSerializer.Deserialize<T>(message.Body);
            try
            {
                await _messageHandler.Handle(value);
            }
            catch
            {
                response.BatchItemFailures.Add(new SQSBatchResponse.BatchItemFailure{ ItemIdentifier = message.MessageId });
            }
        }

        return response;
    }
}
