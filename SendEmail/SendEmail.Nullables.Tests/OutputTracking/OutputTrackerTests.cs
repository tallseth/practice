using NUnit.Framework;
using SendEmail.Nullables.OutputTracking;

namespace SendEmail.Nullables.Tests.OutputTracking;

public class OutputTrackerTests
{
    [Test]
    public void ObservedInfoGetsAddedToData()
    {
        var tracker = new OutputTracker<object>();
        var myObject = new object();
        
        tracker.Observe(myObject);
        
        Assert.That(tracker.Data.Single(), Is.SameAs(myObject));
    }
}