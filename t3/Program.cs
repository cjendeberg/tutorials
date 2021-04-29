using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace t3
{
  class Program
  {
    static void Main(string[] args)
    {
    //"EventBusConnection": "localhost",
    //"EventBusUserName": "zero99lotto",
    //"EventBusPassword": "Test123!",
    //"SubscriptionClientName": "Draw",
    //"EventBusRetryCount": 5,
    //"AzureServiceBusEnabled": false,
    //"ExchangeName": "MessageBus",
    //"VirtualHost": "vhost"
   

      ConnectionFactory factory = new ConnectionFactory();
      factory.UserName = "zero99lotto";
      factory.Password = "Test123!";
      factory.VirtualHost = "vhost";
      factory.HostName = "localhost";
      factory.ClientProvidedName = "app:t3 component:event-consumer";

      IConnection conn = factory.CreateConnection();

      IModel channel = conn.CreateModel();


      channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
      channel.QueueDeclare(queueName, false, false, false, null);
      channel.QueueBind(queueName, exchangeName, routingKey, null);

      channel.Close();
      conn.Close();

    }
  }
}
