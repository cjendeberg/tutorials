using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Zero99Lotto.SRC.Common.Bus.Interfaces;

namespace Zero99Lotto.SRC.Common.Bus.Clients.Connections
{
    public class DefaultAzureServiceBusPersistentConnection : IAzureServiceBusPersistentConnection
    {
        private readonly ILogger<DefaultAzureServiceBusPersistentConnection> _logger;
        private ITopicClient _topicClient;
        bool _disposed;
        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; private set; }

        public DefaultAzureServiceBusPersistentConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            ILogger<DefaultAzureServiceBusPersistentConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ServiceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return _topicClient;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
        }
    }
}
