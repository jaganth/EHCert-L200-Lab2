using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartitionDist
{
    class Program
    {
        static string connectionString = "Endpoint=sb://ehlab2-ns-3dbkbvgazmng2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RwLsqL4YrYQmVl7oB7LY2fD27ZcdHTxc0YoUhwdI638=";
        //static string connectionString = "[REPLACE-WITH-CONNECTION-STRING]";
        static string eventhubName = "EHLab2Hub";
        static void Main(string[] args)
        {
            //Create Event Hub client
            var eventhubClient = EventHubClient.CreateFromConnectionString(connectionString, eventhubName);

            //Send 100 messages to the Event Hub
            var data = new EventData(Encoding.UTF8.GetBytes("Hello World!"));
            data.PartitionKey = "FirstPartition";
            for (int i = 0; i < 100; i++)
            {
                eventhubClient.Send(data.Clone()); 
            }

            //Display the distribution of messages in each parition
            var mgr = NamespaceManager.CreateFromConnectionString(connectionString);
            var ehDesc = mgr.GetEventHub(eventhubName);
            for(int j=0; j<ehDesc.PartitionCount; j++)
            {
                var partInfo = eventhubClient.GetPartitionRuntimeInformation(j.ToString());
                Console.WriteLine($"Partition {j} contains {partInfo.LastEnqueuedSequenceNumber} messages");
            }
        }
    }
}
