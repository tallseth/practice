using Amazon.SimpleEmail.Model;

namespace SendEmail.Nullables;


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
