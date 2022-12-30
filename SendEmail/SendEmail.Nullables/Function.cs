using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Nullables;

public class Function
{
    private readonly SqsAdapter<SendTemplatedEmailRequest> _sqsAdapter;

    public Function() : this(EmailWrapper.Create())
    {
        
    }

    public Function(EmailWrapper emailWrapper)
    {
        _sqsAdapter = new SqsAdapter<SendTemplatedEmailRequest>(EmailMessageHandler.Create(emailWrapper));
    }
    
    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent input, ILambdaContext _)
    {
        return await _sqsAdapter.Adapt(input);
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
        private readonly EmailWrapper _emailWrapper;

        private EmailMessageHandler(EmailWrapper emailWrapper)
        {
            _emailWrapper = emailWrapper;
        }

        public static EmailMessageHandler Create()
        {
            return Create(EmailWrapper.Create());
        }
        
        public static EmailMessageHandler Create(EmailWrapper wrapper)
        {
            return new EmailMessageHandler(wrapper);
        }

        public Task Handle(SendTemplatedEmailRequest message)
        {
            return _emailWrapper.SendTemplatedEmail(message);
        }
    }    
}