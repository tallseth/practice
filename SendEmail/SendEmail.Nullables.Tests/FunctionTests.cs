using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using Amazon.SimpleEmail.Model;
using NUnit.Framework;

namespace SendEmail.Nullables.Tests;

public class FunctionTests
{
    [Test]
    public async Task EmptyBatchDoesNotFail()
    {
        var function = new Function(EmailWrapper.CreateNull());
        var input = new SQSEvent { Records = new List<SQSEvent.SQSMessage>() };
        var context = new TestLambdaContext();

        var result = await function.FunctionHandler(input, context);
        
        Assert.That(result.BatchItemFailures, Is.Empty);
    }

    [Test]
    public async Task MultipleRequestsAllGetSent()
    {
        var emailWrapper = EmailWrapper.CreateNull();
        var tracker = emailWrapper.TrackRequests();
        var function = new Function(emailWrapper);
        var requests = new[] { CreateRequest(), CreateRequest(), CreateRequest() };
        var input = new SQSEvent { Records = CreateFromRequests(requests) };

        var result = await function.FunctionHandler(input, new TestLambdaContext());

        for (int i = 0; i < requests.Length; i++)
        {
            Assert.That(tracker.Data[i].Source, Is.EqualTo(requests[i].Source));
        }
        Assert.That(result.BatchItemFailures, Is.Empty);
    }
    
    [Test]
    public async Task AllRequestGetSendEvenIfTheyFail()
    {
        var emailWrapper = EmailWrapper.CreateNull(configuredException:new ArithmeticException());
        var tracker = emailWrapper.TrackRequests();
        var function = new Function(emailWrapper);
        var requests = new[] { CreateRequest(), CreateRequest(), CreateRequest() };
        var input = new SQSEvent { Records = CreateFromRequests(requests) };

        await function.FunctionHandler(input, new TestLambdaContext());

        for (int i = 0; i < requests.Length; i++)
        {
            Assert.That(tracker.Data[i].Source, Is.EqualTo(requests[i].Source));
        }
    }
    
    [Test]
    public async Task FailuresAreTracked()
    {
        var emailWrapper = EmailWrapper.CreateNull(configuredException:new ArithmeticException());
        var function = new Function(emailWrapper);
        var requests = new[] { CreateRequest(), CreateRequest(), CreateRequest() };
        var input = new SQSEvent { Records = CreateFromRequests(requests) };

        var result = await function.FunctionHandler(input, new TestLambdaContext());

        Assert.That(result.BatchItemFailures.Select(f=>f.ItemIdentifier).ToArray(), Is.EquivalentTo(input.Records.Select(r=>r.MessageId).ToArray()));
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