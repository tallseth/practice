using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Nullables;

public class Function
{
    private readonly SqsAdapter<SendTemplatedEmailRequest> _sqsAdapter;

    public Function() : this(new SqsAdapter<SendTemplatedEmailRequest>(EmailMessageHandler.Create()))
    {
        
    }

    public Function(EmailWrapper emailWrapper)
    {
        _sqsAdapter = new SqsAdapter<SendTemplatedEmailRequest>(EmailMessageHandler.Create(emailWrapper));
    }

    private Function(SqsAdapter<SendTemplatedEmailRequest> adapter)
    {
        _sqsAdapter = adapter;
    }

    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent input, ILambdaContext _)
    {
        return await _sqsAdapter.Adapt(input);
    }
}