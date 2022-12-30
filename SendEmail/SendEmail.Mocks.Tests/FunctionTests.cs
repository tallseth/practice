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
    public async Task EmptyBatchDoesNotFail()
    {
        var input = new SQSEvent { Records = new List<SQSEvent.SQSMessage>() };

        var result = await _function.FunctionHandler(input, new TestLambdaContext());
        
        Assert.That(result.BatchItemFailures, Is.Empty);
    }

    [Test]
    public async Task BatchWithMultipleMessagesForwardsEachToEmailService()
    {
        var requests = new[] { CreateRequest(), CreateRequest(), CreateRequest() };
        var input = new SQSEvent { Records = CreateFromRequests(requests) };

        var result = await _function.FunctionHandler(input, new TestLambdaContext());
        
        Assert.That(result.BatchItemFailures, Is.Empty);
        foreach (var expectedRequest in requests)
        {
            _emailService.Verify(e=>e.SendTemplatedEmailAsync(It.Is<SendTemplatedEmailRequest>(r=>r.Source == expectedRequest.Source), It.IsAny<CancellationToken>()));
        }
    }

    [Test]
    public async Task BatchWithSomeExceptionsOnlyFailsThoseRequests()
    {
        var requests = new[] { CreateRequest(), CreateRequest(), CreateRequest() };
        var input = new SQSEvent { Records = CreateFromRequests(requests) };
        _emailService.Setup(e =>
            e.SendTemplatedEmailAsync(It.Is<SendTemplatedEmailRequest>(r => r.Source == requests[1].Source),
                It.IsAny<CancellationToken>())).Throws<Exception>();

        var result = await _function.FunctionHandler(input, new TestLambdaContext());
        
        Assert.That(result.BatchItemFailures.Single().ItemIdentifier, Is.EqualTo(input.Records[1].MessageId));
        _emailService.Verify(e=>e.SendTemplatedEmailAsync(It.IsAny<SendTemplatedEmailRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(requests.Length));
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