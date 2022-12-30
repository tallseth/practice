using NUnit.Framework;
using SendEmail.Nullables.OutputTracking;

namespace SendEmail.Nullables.Tests.OutputTracking;

public class OutputListenerTests
{
    [Test]
    public void CreatedTrackerGetsData()
    {
        var listener = new OutputListener<object>();
        var myObject = new object();

        var tracker = listener.CreateTracker();
        listener.Observe(myObject);
        
        Assert.That(tracker.Data.Single(), Is.SameAs(myObject));
    }
}