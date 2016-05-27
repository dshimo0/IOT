using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EventHubDemo.Receiver
{
    class Receiver
    {
        
        #region Fields
        string eventHubName;        
        string eventHubConnectionString;
        EventProcessorHost eventProcessorHost;
        #endregion

        public Receiver(string eventHubName, string eventHubConnectionString)
        {
            this.eventHubConnectionString = eventHubConnectionString;
            this.eventHubName = eventHubName;
        }

        public void MessageProcessingWithPartitionDistribution(ConsumerGroupDescription group)
        {
            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, this.eventHubName);

            // Get the default Consumer Group
            //defaultConsumerGroup = eventHubClient.GetDefaultConsumerGroup();
            
            string blobConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"]; // Required for checkpoint/state
            
            if(null == group)
            {
                //Use default consumer group
                EventHubConsumerGroup defaultConsumerGroup = eventHubClient.GetDefaultConsumerGroup();
                eventProcessorHost = new EventProcessorHost("singleworker", eventHubClient.Path, defaultConsumerGroup.GroupName, this.eventHubConnectionString, blobConnectionString);
            }
            else
            {
                //Use custom consumer group
                eventProcessorHost = new EventProcessorHost("singleworker", eventHubClient.Path, group.Name, this.eventHubConnectionString, blobConnectionString);
            }

            //Only use events from the time the sender is started.
            EventProcessorOptions options = new EventProcessorOptions();
            options.InitialOffsetProvider = (partitionId) => DateTime.UtcNow;

            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();
        }

        public void UnregisterEventProcessor()
        {
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
