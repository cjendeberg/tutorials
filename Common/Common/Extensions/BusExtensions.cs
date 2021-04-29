using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using Zero99Lotto.SRC.Common.Bus;
using Zero99Lotto.SRC.Common.Bus.Clients.Connections;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using RabbitMQ.Client;
using Zero99Lotto.SRC.Common.Bus.Clients;
using Zero99Lotto.SRC.Common.Bus.Clients.SubscriptionManagers;
using Microsoft.Azure.ServiceBus;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class BusExtensions
    {
        public static IBusSubscriber UseBus(this IApplicationBuilder app, IContainer container)
            => new BusSubscriber(container.Resolve<IBusClient>());

        public static IBusSubscriber UseBus(this IContainer container)
            => new BusSubscriber(container.Resolve<IBusClient>());

        public static void AddBus(this ContainerBuilder builder, EventBusSettings settings)
        {
            builder.RegisterType<InMemorySubscriptionManager>()
              .As<ISubscriptionManager>().SingleInstance();

            if (settings.AzureServiceBusEnabled)
                AddAzureBus(builder, settings);
            else
                AddRabbitMQBus(builder, settings);


            builder.RegisterType<BusPublisher>()
                .As<IBusPublisher>().SingleInstance();

            builder.RegisterType<BusSubscriber>()
               .As<IBusSubscriber>().SingleInstance();
        }

        private static void AddAzureBus(ContainerBuilder builder, EventBusSettings settings)
        {
            builder.Register<IAzureServiceBusPersistentConnection>(ctx =>
            {
                var logger = ctx.Resolve<ILogger<DefaultAzureServiceBusPersistentConnection>>();
                var serviceBusConnection = new ServiceBusConnectionStringBuilder(settings.EventBusConnection);

                return new DefaultAzureServiceBusPersistentConnection(serviceBusConnection, logger);
            }).SingleInstance();

            builder.Register<IBusClient>(ctx =>
            {
                var persisterConnection = ctx.Resolve<IAzureServiceBusPersistentConnection>();
                var lifeTimeScope = ctx.Resolve<ILifetimeScope>();
                var logger = ctx.Resolve<ILogger<AzureClient>>();
                var subscriptionManager = ctx.Resolve<ISubscriptionManager>();

                return new AzureClient(persisterConnection, lifeTimeScope, subscriptionManager, logger,
                    settings.SubscriptionClientName, settings.ExchangeName);
            }).SingleInstance();
        }

        private static void AddRabbitMQBus(ContainerBuilder builder, EventBusSettings settings)
        {
            builder.Register<IRabbitMQPersistentConnection>(ctx =>
            {
                var logger = ctx.Resolve<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    VirtualHost= settings.VirtualHost,
                    HostName = settings.EventBusConnection,
                    UserName = settings.EventBusUserName,
                    Password = settings.EventBusPassword,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };
                var retryCount = settings.EventBusRetryCount > 0 ? settings.EventBusRetryCount : 5;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            }).SingleInstance();

            builder.Register<IBusClient>(ctx =>
            {
                var logger = ctx.Resolve<ILogger<RabbitMQClient>>();
                var lifeTimeScope = ctx.Resolve<ILifetimeScope>();
                var subscriptionManager = ctx.Resolve<ISubscriptionManager>();
                var persistentConnection = ctx.Resolve<IRabbitMQPersistentConnection>();
                var retryCount = settings.EventBusRetryCount > 0 ? settings.EventBusRetryCount : 5;

                return new RabbitMQClient(lifeTimeScope, persistentConnection, retryCount,
                    settings.SubscriptionClientName, subscriptionManager, logger, settings.ExchangeName, settings.LogMessages);
            });
        }
    }
}
