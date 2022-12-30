namespace SendEmail.Nullables.OutputTracking;

public class OutputTracker<T>
{
    public IList<T> Data { get; } = new List<T>();
    
    public void Observe(T output)
    {
        Data.Add(output);
    }
}