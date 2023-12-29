namespace SendEmail.Nullables;

public interface IMessageHandler<T>
{
    Task Handle(T message);
}