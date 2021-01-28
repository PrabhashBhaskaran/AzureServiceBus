using System;
using System.Collections.Generic;
using System.Linq;
using Prabhash.Platform.AzureServiceBus.Library.Core;
using Prabhash.Platform.AzureServiceBus.Library.Implementation;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
namespace Test
{
    class Program
    {
        static string Jsonvalue;
        static string queuename = ConfigurationManager.AppSettings["queue"];
        static string connection = ConfigurationManager.AppSettings["connection"];
        static int _batchsize = Convert.ToInt32(ConfigurationManager.AppSettings["batchSize"]);
        static int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);

        static void Main(string[] args)
        {
            Jsonvalue = GetQueueMessage();
            while (true)
            {
                Console.WriteLine("\n1.Send Messages to queue\n2.Receive Messages from queue\n3.Receive Messages with commit\n4.Clear\n5.Quit");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        SendMessages(queuename, connection);
                        break;
                    case ConsoleKey.D2:
                        ReadMessages(queuename, connection);
                        break;
                    case ConsoleKey.D3:
                        ReadMessagesAndCommit(queuename, connection);
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        break;
                    case ConsoleKey.D5:
                        return;
                }
            }
        }

        private static void SendMessages(string queue, string connectionString)
        {
            IServiceBusManager manager = new ServiceBusManager(connectionString);
            List<BrokeredMessage> mesages = GenerateSampleMessages();
            if (manager.QueueExists(queue).Result)
            {
                manager.GetDecoratedQueue(queue).Result.EnqueueAsync(mesages);
            }
            else
            {
                Console.WriteLine("Invalid Queue name : " + queue);
            }
        }

        private static void ReadMessages(string queue, string connectionString)
        {
            IServiceBusManager manager = new ServiceBusManager(connectionString);
            if (manager.QueueExists(queue).Result)
            {
                List<BrokeredMessage> mesages = manager.GetDecoratedQueue(queue).Result.DequeueAsync(200, TimeSpan.FromSeconds(10)).Result.ToList();
                if (mesages.Count == 0) { Console.WriteLine("\nQueue Empty!"); return; }
                foreach (var m in mesages)
                {
                    string msg = m.GetBody<string>();
                    Console.WriteLine(msg);
                }
            }
            else
            {
                Console.WriteLine("Invalid Queue name : " + queue);
            }
        }

        private static void ReadMessagesAndCommit(string queue, string connectionString)
        {
            IServiceBusManager manager = new ServiceBusManager(connectionString);
            if (manager.QueueExists(queue).Result)
            {
                IServiceBusQueueDecorator decorator = manager.GetDecoratedQueue(queue).Result;
                List<BrokeredMessage> mesages = decorator.DequeueAsync(200, TimeSpan.FromSeconds(10)).Result.ToList();
                if (mesages.Count == 0) { Console.WriteLine("\nQueue Empty!"); return; }
                foreach (var m in mesages)
                {
                    string msg = m.GetBody<string>();
                    Console.WriteLine(msg);
                }
                decorator.CompleteAsync(mesages);
            }
            else
            {
                Console.WriteLine("Invalid Queue name : " + queue);
            }
        }
        private static List<BrokeredMessage> GenerateSampleMessages()
        {
            List<BrokeredMessage> mesages = new List<BrokeredMessage>();

            for (int i = 0; i < 200; i++)
            {
                mesages.Add(new BrokeredMessage(Jsonvalue + i));
            }

            return mesages;
        }

        private static string GetQueueMessage()
        {
            return System.IO.File.ReadAllText("RespondMessage.json");
        }

    }
}


