using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using SendEmail.Nullables.OutputTracking;

namespace SendEmail.Nullables;

public abstract class EmailWrapper
{
    public abstract Task<SendTemplatedEmailResponse> SendTemplatedEmail(SendTemplatedEmailRequest request);
    public abstract OutputTracker<SendTemplatedEmailRequest> TrackRequests();
 
    public static EmailWrapper Create()
    {
        var aws = new AmazonSimpleEmailServiceClient();
        return new FunctionImpl(aws.SendTemplatedEmailAsync);
    }
    
    public static EmailWrapper CreateNull(SendTemplatedEmailResponse? configuredResponse=null, Exception? configuredException=null)
    {
        return new FunctionImpl((_, _) =>
        {
            if (configuredException != null)
                throw configuredException;

            return Task.FromResult(configuredResponse ?? new SendTemplatedEmailResponse());
        });
    }

    private class FunctionImpl : EmailWrapper
    {
        private Func<SendTemplatedEmailRequest, CancellationToken, Task<SendTemplatedEmailResponse>> _function;
        private readonly OutputListener<SendTemplatedEmailRequest> _outputListener = new();

        public FunctionImpl(Func<SendTemplatedEmailRequest, CancellationToken, Task<SendTemplatedEmailResponse>> function)
        {
            _function = function;
        }

        public override Task<SendTemplatedEmailResponse> SendTemplatedEmail(SendTemplatedEmailRequest request)
        {
            _outputListener.Observe(request);
            return _function(request, CancellationToken.None);
        }

        public override OutputTracker<SendTemplatedEmailRequest> TrackRequests()
        {
            return _outputListener.CreateTracker();
        }
    }
}