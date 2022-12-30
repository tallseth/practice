using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Nullables;

public class Function
{
    private readonly EmailWrapper _emailWrapper;

    public Function() : this(EmailWrapper.Create())
    {
        
    }

    public Function(EmailWrapper emailWrapper)
    {
        _emailWrapper = emailWrapper;
    }
    
    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent input, ILambdaContext _)
    {
        var batchResponse = new SQSBatchResponse();

        foreach (var record in input.Records)
        {
            var request = JsonSerializer.Deserialize<SendTemplatedEmailRequest>(record.Body);
            try
            {
                await _emailWrapper.SendTemplatedEmail(request);
            }
            catch
            {
                batchResponse.BatchItemFailures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = record.MessageId });
            }
        }
        
        return batchResponse;
    }
}