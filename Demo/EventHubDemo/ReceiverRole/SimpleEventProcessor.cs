using EventHubDemo.Common.Contracts;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverRole
{
    class SimpleEventProcessor : IEventProcessor
    {
        PartitionContext partitionContext;

        public Task OpenAsync(PartitionContext context)
        {
            Trace.TraceInformation(string.Format("SimpleEventProcessor OpenAsync.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
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

                        Trace.TraceInformation(string.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}'",
                            this.partitionContext.Lease.PartitionId, newData.DeviceId, newData.Temperature));
                    }
                    catch (Exception oops)
                    {
                        Trace.TraceError(oops.Message);
                    }
                }

                await context.CheckpointAsync();

            }
            catch (Exception exp)
            {
                Trace.TraceError("Error in processing: " + exp.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Trace.TraceWarning(string.Format("SimpleEventProcessor CloseAsync.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        MetricEvent DeserializeEventData(EventData eventData)
        {

            string data = Encoding.UTF8.GetString(eventData.GetBytes());
            return JsonConvert.DeserializeObject<MetricEvent>(data);
        }
    }
}
