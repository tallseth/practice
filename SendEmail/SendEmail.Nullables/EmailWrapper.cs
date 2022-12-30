using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using SendEmail.Nullables.OutputTracking;

namespace SendEmail.Nullables;

public class EmailWrapper
{
    private Func<SendTemplatedEmailRequest, CancellationToken, Task<SendTemplatedEmailResponse>> _function;
    private readonly OutputListener<SendTemplatedEmailRequest> _outputListener = new();

    private EmailWrapper(Func<SendTemplatedEmailRequest, CancellationToken, Task<SendTemplatedEmailResponse>> function)
    {
        _function = function;
    }

    public Task<SendTemplatedEmailResponse> SendTemplatedEmail(SendTemplatedEmailRequest request)
    {
        _outputListener.Observe(request);
        return _function(request, CancellationToken.None);
    }

    public OutputTracker<SendTemplatedEmailRequest> TrackRequests()
    {
        return _outputListener.CreateTracker();
    }
 
    public static EmailWrapper Create()
    {
        var aws = new AmazonSimpleEmailServiceClient();
        return new EmailWrapper(aws.SendTemplatedEmailAsync);
    }
    
    public static EmailWrapper CreateNull(SendTemplatedEmailResponse? configuredResponse=null, Exception? configuredException=null)
    {
        return new EmailWrapper((_, _) =>
        {
            if (configuredException != null)
                throw configuredException;

            return Task.FromResult(configuredResponse ?? new SendTemplatedEmailResponse());
        });
    }
}