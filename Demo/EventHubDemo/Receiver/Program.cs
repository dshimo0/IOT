using EventHubDemo.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubDemo.Receiver
{
    class Program
    {
        static string eventHubName;        
        static int numberOfPartitions;

        static void Main(string[] args)
        {
            ParseArgs(args);
            var connectionString = EventHubManager.GetServiceBusConnectionString();
            var namespaceManager = EventHubManager.GetNamespaceManager(connectionString);
            EventHubManager.CreateEventHubIfNotExists(eventHubName, numberOfPartitions, namespaceManager);
            
            var group = namespaceManager.CreateConsumerGroupIfNotExists(eventHubName, "TestConsumerGroup");

            Receiver r = new Receiver(eventHubName, connectionString);
            r.MessageProcessingWithPartitionDistribution(group);

            Console.WriteLine("Press enter key to stop worker.");
            Console.ReadLine();
        }

        static void ParseArgs(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Incorrect number of arguments. Expected 2 args <eventhubname> <NumberOfPartitions>", args.ToString());
            }
            else
            {
                eventHubName = args[0];
                Console.WriteLine("ehnanme: " + eventHubName);

                numberOfPartitions = Int32.Parse(args[1]);
                Console.WriteLine("numberOfPartitions: " + numberOfPartitions);
            }
        }
    }
}
