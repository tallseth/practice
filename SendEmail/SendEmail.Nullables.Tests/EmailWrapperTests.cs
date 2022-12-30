using Amazon.SimpleEmail.Model;
using NUnit.Framework;

namespace SendEmail.Nullables.Tests;

public class EmailWrapperTests
{
    [Test]
    public async Task TracksOutput()
    {
        var wrapper = EmailWrapper.CreateNull();
        var tracker = wrapper.TrackRequests();
        var request = new SendTemplatedEmailRequest();

        await wrapper.SendTemplatedEmail(request);

        Assert.That(tracker.Data.Single(), Is.SameAs(request));
    }
    
    [Test]
    public async Task ReturnsDefaultResponseIfNotSet()
    {
        var wrapper = EmailWrapper.CreateNull();

        var response = await wrapper.SendTemplatedEmail(new SendTemplatedEmailRequest());

        //later I'll probably want more details in a default response, good enough for now
        Assert.That(response, Is.Not.Null); 
    }
    
    [Test]
    public async Task ThrowsConfiguredException()
    {
        var expectedException = new DivideByZeroException();
        var wrapper = EmailWrapper.CreateNull(configuredException:expectedException);

        var actualException = Assert.ThrowsAsync<DivideByZeroException>(async () =>
            await wrapper.SendTemplatedEmail(new SendTemplatedEmailRequest()));
        
        Assert.That(actualException, Is.SameAs(expectedException));
    }
    
    [Test]
    public async Task ReturnsConfiguredResponse()
    {
        var expected = new SendTemplatedEmailResponse();
        var wrapper = EmailWrapper.CreateNull(expected);

        var response = await wrapper.SendTemplatedEmail(new SendTemplatedEmailRequest());

        Assert.That(response, Is.SameAs(expected)); 
    }
}