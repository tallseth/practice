using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using NUnit.Framework;

namespace SendEmail.Nullables.Tests;

public class FunctionTests
{
    [Test]
    public async Task EmptyBatchDoesNotFail()
    {
        var function = Function.CreateNull();
        var input = new SQSEvent { Records = new List<SQSEvent.SQSMessage>() };
        var context = new TestLambdaContext();

        var result = await function.FunctionHandler(input, context);
        
        Assert.That(result.BatchItemFailures, Is.Empty);
    }
}