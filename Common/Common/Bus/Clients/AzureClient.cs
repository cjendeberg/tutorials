using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Messages;

namespace Zero99Lotto.SRC.Common.Bus.Clients
{
    public class AzureClient : BusClientBase<AzureClient>
    {
        private readonly IAzureServiceBusPersistentConnection _persistentConnection;
        private readonly SubscriptionClient _subscriptionClient;

        public AzureClient(IAzureServiceBusPersistentConnection persistentConnection, ILifetimeScope lifeTimeScope, ISubscriptionManager subscriptionManager, 
            ILogger<AzureClient> logger, string subscriptionClientName, string exchangeName) 
            : base(lifeTimeScope, subscriptionManager, logger, exchangeName)
        {
            _persistentConnection = persistentConnection;
            _subscriptionClient = new SubscriptionClient(_persistentConnection.ServiceBusConnectionStringBuilder,
              subscriptionClientName);

            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        protected override async Task SendOrPublishAsync(IMessage message)
        {
            var messageName = message.GetType().Name;
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var payload = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = messageName,
            };

            var topicClient = _persistentConnection.CreateModel();

            await topicClient.SendAsync(payload);
                //.GetAwaiter()
                //.GetResult();
        }

        protected override void DoInternalSubscription<TMessage>()
        {
            var hasSubscription = subsManager.HasSubscription<TMessage>();
            if (!hasSubscription)
            {
                var messageName = subsManager.GetMessageName<TMessage>();
                try
                {
                    _subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = messageName },
                        Name = messageName
                    }).GetAwaiter().GetResult();
                }
                catch (ServiceBusException)
                {
                    logger.LogInformation($"The messaging entity {messageName} already exists.");
                }
            }
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = message.Label;//$"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                    var messageData = Encoding.UTF8.GetString(message.Body);
                    await ProcessMessage(eventName, messageData);

                    // Complete the message so that it is not received again.
                    await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                },
               new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private void RemoveDefaultRule()
        {
            try
            {
                _subscriptionClient
                 .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                logger.LogInformation($"The messaging entity { RuleDescription.DefaultRuleName } Could not be found.");
            }
        }
    }
}
