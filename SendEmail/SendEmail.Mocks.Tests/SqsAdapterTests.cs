using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using Moq;
using NUnit.Framework;

namespace SendEmail.Mocks.Tests;

public class SqsAdapterTests
{
    private Mock<IMessageHandler<TestObject>> _handler;
    private SqsAdapter<TestObject> _adapter;

    [SetUp]
    public void Setup()
    {
        _handler = new Mock<IMessageHandler<TestObject>>();
        _adapter = new SqsAdapter<TestObject>(_handler.Object);
    }

    [Test]
    public async Task EmptyBatchDoesNotFail()
    {
        var input = new SQSEvent { Records = new List<SQSEvent.SQSMessage>() };

        var result = await _adapter.Adapt(input);
        
        Assert.That(result.BatchItemFailures, Is.Empty);
    }

    [Test]
    public async Task BatchWithMultipleMessagesForwardsEachToEmailService()
    {
        var testObjects = new[] { CreateTestObject(), CreateTestObject(), CreateTestObject() };
        var input = new SQSEvent { Records = CreateFromTestObjects(testObjects) };

        var result = await _adapter.Adapt(input);
        
        Assert.That(result.BatchItemFailures, Is.Empty);
        foreach (var obj in testObjects)
        {
            _handler.Verify(e=>e.Handle(It.Is<TestObject>(r=>r.Id == obj.Id)));
        }
    }

    [Test]
    public async Task BatchWithSomeExceptionsOnlyFailsThoseRequests()
    {
        var objects = new[] { CreateTestObject(), CreateTestObject(), CreateTestObject() };
        var input = new SQSEvent { Records = CreateFromTestObjects(objects) };
        _handler.Setup(e => e.Handle(It.Is<TestObject>(r => r.Id == objects[1].Id)))
            .Throws<Exception>();
        
        var result = await _adapter.Adapt(input);
        
        Assert.That(result.BatchItemFailures.Single().ItemIdentifier, Is.EqualTo(input.Records[1].MessageId));
        _handler.Verify(e=>e.Handle(It.IsAny<TestObject>()), Times.Exactly(objects.Length));
    }    

    private TestObject CreateTestObject()
    {
        return new TestObject();
    }
    
    private List<SQSEvent.SQSMessage> CreateFromTestObjects(TestObject[] requests)
    {
        return requests
            .Select(r => new SQSEvent.SQSMessage
            {
                Body = JsonSerializer.Serialize(r), 
                MessageId = Guid.NewGuid().ToString()
            })
            .ToList();
    }

    public class TestObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}