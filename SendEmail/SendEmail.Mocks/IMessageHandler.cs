﻿namespace SendEmail.Mocks;

public interface IMessageHandler<T>
{
    Task Handle(T message);
}