using EventHubDemo.Common.Contracts;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;


namespace EventHubDemo.Sender
{
    public class Sender
    {        
        private string eventHubName;
        private int numberOfDevices;
        private int numberOfMessages;


        public Sender(string eventHubName, int numberOfDevices, int numberOfMessages)
        {
            this.eventHubName = eventHubName;
            this.numberOfDevices = numberOfDevices;
            this.numberOfMessages = numberOfMessages;            
        }

        public void SendEvents()
        {
            // Create EventHubClient
            EventHubClient client = EventHubClient.Create(this.eventHubName);

            try
            {
                List<Task> tasks = new List<Task>();
                
                // Send messages to Event Hub
                Trace.TraceInformation("Sending messages to Event Hub " + client.Path);

                Random random = new Random();
                for (int i = 0; i < this.numberOfMessages; ++i)
                {
                    // Create the device/temperature metric
                    MetricEvent info = new MetricEvent() { DeviceId = random.Next(numberOfDevices), Temperature = random.Next(100) };
                    var serializedString = JsonConvert.SerializeObject(info);

                    EventData data = new EventData(Encoding.UTF8.GetBytes(serializedString));

                    // Set user properties if needed
                    data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());
                    OutputMessageInfo("SENDING: ", data, info);

                    // Send the metric to Event Hub
                    tasks.Add(client.SendAsync(data));
                }
                ;

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception exp)
            {
                Trace.TraceError("Error on send: " + exp.Message);
            }

            client.CloseAsync().Wait();
        }

        static void OutputMessageInfo(string action, EventData data, MetricEvent info)
        {
            if (data == null)
            {
                return;
            }
            if (info != null)
            {
                Trace.TraceInformation("{0}: Device {1}, Temperature {2}.", action, info.DeviceId, info.Temperature);
            }
        }
    }
}
