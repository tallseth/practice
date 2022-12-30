using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Mocks;

public class Function
{
    private readonly IAmazonSimpleEmailService _emailService;

    public Function() : this(new AmazonSimpleEmailServiceClient()) { }
    
    public Function(IAmazonSimpleEmailService emailService)
    {
        _emailService = emailService;
        
    }
    
    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent input, ILambdaContext _)
    {
        var response = new SQSBatchResponse();
        
        foreach(var message in input.Records)
        {
            var request = JsonSerializer.Deserialize<SendTemplatedEmailRequest>(message.Body);
            try
            {
                await _emailService.SendTemplatedEmailAsync(request);
            }
            catch
            {
                response.BatchItemFailures.Add(new SQSBatchResponse.BatchItemFailure{ ItemIdentifier = message.MessageId });
            }
        }

        return response;
    }
}

