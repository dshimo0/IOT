using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using EventHubDemo.Common.Contracts;

namespace EventHubDemo.Receiver
{
    class SimpleEventProcessor : IEventProcessor
    {        
        PartitionContext partitionContext;        

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine(string.Format("SimpleEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
            this.partitionContext = context;

            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
            try
            {
                foreach (EventData eventData in events)
                {                    
                    try
                    { 
                    var newData = this.DeserializeEventData(eventData);

                    Console.WriteLine(string.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}', Enqueued: '{3:MM/dd/yy H:mm:ss}'",
                        this.partitionContext.Lease.PartitionId, newData.DeviceId, newData.Temperature, eventData.EnqueuedTimeUtc));
                        }
                    catch(Exception oops)
                    {
                        Console.WriteLine(oops.Message);
                    }
                }
                
                await context.CheckpointAsync();               
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in processing: " + exp.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine(string.Format("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        MetricEvent DeserializeEventData(EventData eventData)
        {
            return JsonConvert.DeserializeObject<MetricEvent>(Encoding.UTF8.GetString(eventData.GetBytes()));
        }
    }
}
