using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Messages;

namespace Zero99Lotto.SRC.Common.Bus.Clients
{
    public class RabbitMQClient : BusClientBase<RabbitMQClient>
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly int _retryCount;
        private readonly string _queueName;
        private readonly bool _logMessages;
        private IModel _consumerChannel;

        public RabbitMQClient(ILifetimeScope lifeTimeScope, IRabbitMQPersistentConnection persistentConnection,
            int retryCount, string queueName, ISubscriptionManager subscriptionManager, ILogger<RabbitMQClient> logger,
            string exchangeName = null, bool logMessages = false)
            : base(lifeTimeScope, subscriptionManager, logger, exchangeName)
        {
            _retryCount = retryCount;
            _persistentConnection = persistentConnection;
            _queueName = queueName;
            _logMessages = logMessages;
            _consumerChannel = CreateConsumerChannel();
        }

        protected void Connect()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
        }

        protected override async Task SendOrPublishAsync(IMessage message)
        {
            Connect();

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
              .Or<SocketException>()
              .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
              {
                  logger.LogWarning(ex.ToString());
              });

            using (var channel = _persistentConnection.CreateModel())
            {
                var messageName = message.GetType().Name;
                var payload = JsonConvert.SerializeObject(message);
                if (_logMessages)
                {
                    // _queueName
                    logger.LogInformation($"{BrokerName}: {_queueName} sending message {messageName}:{payload}");
                }

                var body = Encoding.UTF8.GetBytes(payload);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: BrokerName,
                                     routingKey: messageName,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });
                await Task.CompletedTask;
            }
        }

        protected override void DoInternalSubscription<TMessage>() 
        {
            var hasSubscription = subsManager.HasSubscription<TMessage>();
            if (!hasSubscription)
            {
                Connect();

                using (var channel = _persistentConnection.CreateModel())
                {
                    var messageName = subsManager.GetMessageName<TMessage>();

                    channel.QueueBind(queue: _queueName,
                                      exchange: BrokerName,
                                      routingKey: messageName);
                }
            }
        }

        public override void Dispose()
        {
            _consumerChannel?.Dispose();
            base.Dispose();
        }

        private IModel CreateConsumerChannel()
        {
            Connect();

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BrokerName,
                                 type: "direct",
                                 durable: true);

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var messageName = ea.RoutingKey;
                var payload = Encoding.UTF8.GetString(ea.Body);
                if (_logMessages)
                {
                    // _queueName
                    logger.LogInformation($"{BrokerName}: {_queueName} received message {messageName}:{payload}");
                }

                if (await ProcessMessage(messageName, payload))
                {
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                else
                    channel.BasicNack(ea.DeliveryTag, false, true);

            };

            channel.BasicConsume(queue: _queueName,
                               autoAck: false,
                               consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }
    }
}
