using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Moq;
using NUnit.Framework;

namespace SendEmail.Mocks.Tests;

public class FunctionTests
{
    private Mock<IAmazonSimpleEmailService> _emailService = null!;
    private Function _function = null!;

    [SetUp]
    public void Setup()
    {
        _emailService = new Mock<IAmazonSimpleEmailService>();
        _function = new Function(_emailService.Object);
    }

    [Test]
    public async Task GetsMessagesToEmailService()
    {
        var input = new SQSEvent { Records = CreateFromRequests(new[] { CreateRequest() }) };

        var result = await _function.FunctionHandler(input, new TestLambdaContext());
        
        Assert.That(result.BatchItemFailures, Is.Empty);
        _emailService.Verify(e=>e.SendTemplatedEmailAsync(It.IsAny<SendTemplatedEmailRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(input.Records.Count));
    }

    private SendTemplatedEmailRequest CreateRequest()
    {
        return new SendTemplatedEmailRequest
        {
            Source = Guid.NewGuid().ToString()
        };
    }
    
    private List<SQSEvent.SQSMessage> CreateFromRequests(SendTemplatedEmailRequest[] requests)
    {
        return requests
            .Select(r => new SQSEvent.SQSMessage
            {
                Body = JsonSerializer.Serialize(r), 
                MessageId = Guid.NewGuid().ToString()
            })
            .ToList();
    }
}