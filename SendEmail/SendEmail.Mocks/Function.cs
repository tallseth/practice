using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Mocks;

public class Function
{
    private SqsAdapter<SendTemplatedEmailRequest> _adapter;
    public Function() : this(new AmazonSimpleEmailServiceClient()) { }
    
    public Function(IAmazonSimpleEmailService emailService)
    {
        _adapter = new SqsAdapter<SendTemplatedEmailRequest>(new EmailMessageHandler(emailService));
    }
    
    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent input, ILambdaContext _)
    {
        return await _adapter.Adapt(input);
    }
}

public interface ISqsAdapter
{
    Task<SQSBatchResponse> Adapt(SQSEvent sqsEvent);
}

public class SqsAdapter<T> : ISqsAdapter
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

public interface IMessageHandler<T>
{
    Task Handle(T message);
}

public class EmailMessageHandler : IMessageHandler<SendTemplatedEmailRequest>
{
    private readonly IAmazonSimpleEmailService _emailService;

    public EmailMessageHandler(IAmazonSimpleEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task Handle(SendTemplatedEmailRequest message)
    {
        return _emailService.SendTemplatedEmailAsync(message);
    }
}

