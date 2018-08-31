using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using System.Threading;             // For delay function

namespace SendCloudToDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=trafficIoT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=gnpmxqawgwF4uybTebpFsh3XpFvHq5XbA40I5RIjNNA=";

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            ReceiveFeedbackAsync();

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();

            //// Testing For Continuous Sending Messages
            //Thread.Sleep(2000);
            //while (true)
            //{
            //    SendCloudToDeviceMessageAsync().Wait();
            //    Thread.Sleep(2000);
            //}
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", commandMessage);

            //var val1 = new Message(Encoding.ASCII.GetBytes("1"));
            //val1.Ack = DeliveryAcknowledgement.Full;
            //await serviceClient.SendAsync("Lenovo01", val1);

            //var val2 = new Message(Encoding.ASCII.GetBytes("2"));
            //val2.Ack = DeliveryAcknowledgement.Full;
            //await serviceClient.SendAsync("Lenovo01", val2);

            //var val3 = new Message(Encoding.ASCII.GetBytes("3"));
            //val3.Ack = DeliveryAcknowledgement.Full;
            //await serviceClient.SendAsync("Lenovo01", val3);

            //var val4 = new Message(Encoding.ASCII.GetBytes("4"));
            //val4.Ack = DeliveryAcknowledgement.Full;
            //await serviceClient.SendAsync("Lenovo01", val4);
        }

        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
    }
}
