﻿using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
  class Send
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory()
      {
        HostName = "localhost",
        UserName = "zero99lotto",
        Password = "Test123!",
        VirtualHost = "vhost",      
        ClientProvidedName = "app:t3 component:event-consumer"
      };
      using (var connection = factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
          channel.QueueDeclare(queue: "hello",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

          string message = "Hello World!";
          var body = Encoding.UTF8.GetBytes(message);

          channel.BasicPublish(exchange: "",
                               routingKey: "hello",
                               basicProperties: null,
                               body: body);
          Console.WriteLine(" [x] Sent {0}", message);
        }
        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
      }
    }
  }
}
