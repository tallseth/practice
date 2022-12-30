using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

namespace SendEmail.Nullables;

public class Function
{
    private readonly EmailWrapper _emailWrapper;

    public Function() : this(EmailWrapper.Create())
    {
        
    }

    private Function(EmailWrapper emailWrapper)
    {
        _emailWrapper = emailWrapper;
    }

    public static Function CreateNull()
    {
        return new Function(EmailWrapper.CreateNull());
    }

    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        return await Task.FromResult(new SQSBatchResponse());
    }
}