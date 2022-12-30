namespace SendEmail.Nullables.OutputTracking;

public class OutputListener<T>
{
    //there should be a list here, and tracker removal, etc.  I haven't needed it yet, so not adding it yet
    private OutputTracker<T>? _tracker; 

    public OutputTracker<T> CreateTracker()
    {
        _tracker = new OutputTracker<T>();
        return _tracker;
    }

    public void Observe(T output)
    {
        _tracker?.Observe(output);
    }
}