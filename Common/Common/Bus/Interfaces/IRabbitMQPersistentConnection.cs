using RabbitMQ.Client;
using System;

namespace Zero99Lotto.SRC.Common.Bus.Interfaces
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
