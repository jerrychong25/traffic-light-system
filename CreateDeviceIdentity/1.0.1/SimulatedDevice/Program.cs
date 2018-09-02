using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "trafficIoT.azure-devices.net";
        static string deviceKey = "NtN3QbBlBJ/o88+Pf+vNu3fuFA8buNb0nx7hH1lqk60=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Lenovo01", deviceKey));

            SendDeviceToCloudMessagesAsync();
            ReceiveC2dAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            Random rand = new Random();

            while (true)
            {
                int north_traffic1 = rand.Next(1, 50);
                int east_traffic1 = rand.Next(1, 50);
                int south_traffic1 = rand.Next(1, 50);
                int west_traffic1 = rand.Next(1, 50);
                int north_traffic2 = rand.Next(1, 50);
                int east_traffic2 = rand.Next(1, 50);
                int south_traffic2 = rand.Next(1, 50);
                int west_traffic2 = rand.Next(1, 50);

                var telemetryDataPoint = new
                {
                    // deviceId = "myFirstDevice",
                    deviceId = "Lenovo01",
                    nt1 = north_traffic1,
                    et1 = east_traffic1,
                    st1 = south_traffic1,
                    wt1 = west_traffic1,
                    nt2 = north_traffic2,
                    et2 = east_traffic2,
                    st2 = south_traffic2,
                    wt2 = west_traffic2,
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                var test111 = JsonConvert.DeserializeObject(messageString);
                test111 = 0;

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Thread.Sleep(1000);
            }
        }

        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received message: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}

