using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace SendEmail.Mocks;

public class EmailMessageHandler : IMessageHandler<SendTemplatedEmailRequest>
{
    private readonly IAmazonSimpleEmailService _emailService;

    public EmailMessageHandler(IAmazonSimpleEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task Handle(SendTemplatedEmailRequest message)
    {
        return _emailService.SendTemplatedEmailAsync(message);
    }
}