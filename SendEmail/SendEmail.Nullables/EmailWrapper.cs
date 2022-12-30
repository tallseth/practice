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
        return new AwsImpl();
    }
    
    public static EmailWrapper CreateNull(SendTemplatedEmailResponse? configuredResponse=null, Exception? configuredException=null)
    {
        return new NullImpl(configuredResponse, configuredException);
    }

    private class AwsImpl : EmailWrapper
    {
        private readonly IAmazonSimpleEmailService _emailService = new AmazonSimpleEmailServiceClient();
        private readonly OutputListener<SendTemplatedEmailRequest> _outputListener = new();
        
        public override Task<SendTemplatedEmailResponse> SendTemplatedEmail(SendTemplatedEmailRequest request)
        {
            _outputListener.Observe(request);
            return _emailService.SendTemplatedEmailAsync(request);
        }

        public override OutputTracker<SendTemplatedEmailRequest> TrackRequests()
        {
            return _outputListener.CreateTracker();
        }
    }
    
    private class NullImpl : EmailWrapper
    {
        private readonly OutputListener<SendTemplatedEmailRequest> _outputListener = new();
        private readonly Exception? _configuredException;
        private readonly SendTemplatedEmailResponse _configuredResponse;

        public NullImpl(SendTemplatedEmailResponse? configuredResponse, Exception? configuredException)
        {
            _configuredException = configuredException;
            _configuredResponse = configuredResponse ?? new SendTemplatedEmailResponse();
        }

        public override Task<SendTemplatedEmailResponse> SendTemplatedEmail(SendTemplatedEmailRequest request)
        {
            _outputListener.Observe(request);
            if (_configuredException != null)
            {
                throw _configuredException;
            }
            
            return Task.FromResult(_configuredResponse);
        }

        public override OutputTracker<SendTemplatedEmailRequest> TrackRequests()
        {
            return _outputListener.CreateTracker();
        }
    }
}