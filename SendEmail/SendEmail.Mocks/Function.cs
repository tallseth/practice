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