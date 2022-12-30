using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Moq;
using NUnit.Framework;

namespace SendEmail.Mocks.Tests;

public class EmailMessageHandlerTests
{
    [Test]
    public async Task DelegatesToAws()
    {
        var aws = new Mock<IAmazonSimpleEmailService>();
        var handler = new EmailMessageHandler(aws.Object);
        var request = new SendTemplatedEmailRequest { Source = Guid.NewGuid().ToString() };

        await handler.Handle(request);

        aws.Verify(s=>s.SendTemplatedEmailAsync(request, It.IsAny<CancellationToken>()), Times.Once());
    }
}