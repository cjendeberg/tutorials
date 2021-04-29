using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Messages;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus.Clients
{
    public abstract class BusClientBase<TClient> : IBusClient
    {
        protected readonly ILogger<TClient> logger;
        protected readonly ISubscriptionManager subsManager;
        protected readonly ILifetimeScope lifetimeScope;

        protected readonly string AutofacScopeName;
        protected readonly string BrokerName;

        public BusClientBase(ILifetimeScope lifeTimeScope, ISubscriptionManager subscriptionManager, ILogger<TClient> logger,
            string exchangeName)
        {
            this.lifetimeScope = lifeTimeScope;
            this.subsManager = subscriptionManager;
            this.logger = logger;
            if (string.IsNullOrWhiteSpace(exchangeName))
                exchangeName = "MessageBus";

            AutofacScopeName = exchangeName;
            BrokerName = exchangeName;
        }

        public virtual async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
            => await SendOrPublishAsync(@event);

        public virtual async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
            => await SendOrPublishAsync(command);

        public virtual IBusSubscriber SubscribeCommand<TCommand>() where TCommand : ICommand
        {
            DoInternalSubscription<TCommand>();
            subsManager.SubscribeCommandAsync<TCommand>();
            return this;
        }

        public virtual IBusSubscriber SubscribeEvent<TEvent>() where TEvent : IEvent
        {
            DoInternalSubscription<TEvent>();
            subsManager.SubscribeEventAsync<TEvent>();
            return this;
        }

        protected abstract Task SendOrPublishAsync(IMessage message);
        protected abstract void DoInternalSubscription<TMessage>() where TMessage : IMessage;

        public virtual void Dispose()
        {
            subsManager.Clear();
        }

        protected virtual async Task<bool> ProcessMessage(string messageName, string payload)
        {
            var messageType = subsManager.GetRegisteredMessageType(messageName);
            if (messageType != null)
            {
                using (var scope = lifetimeScope.BeginLifetimeScope(AutofacScopeName))
                {
                    Type handlerType = null;
                    if (subsManager.BelongsToCommands(messageName))
                        handlerType = typeof(ICommandHandler<>);

                    if (subsManager.BelongsToEvents(messageName))
                        handlerType = typeof(IEventHandler<>);

                    if (handlerType != null)
                    {
                        var specificType = handlerType.MakeGenericType(messageType);

                        var handler = scope.Resolve(specificType);
                        var message = JsonConvert.DeserializeObject(payload, messageType);

                        try
                        {
                            await (Task)specificType.GetMethod("HandleAsync").Invoke(handler, new object[] { message });
                        }
                        catch (Exception ex)
                        {
                            // Log and silently drop the message
                            logger.LogError(ex, $"{BrokerName}: {Assembly.GetEntryAssembly().GetName().Name} HandleAsync() exception handling message {messageName}:{payload}");
                        }
                    }

                }
                return true;
            }
            return false;
        }
    }
}
