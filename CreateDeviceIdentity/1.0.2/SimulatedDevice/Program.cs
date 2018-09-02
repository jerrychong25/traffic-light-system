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

                Thread.Sleep(2500);
            }
        }

        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");

            string north_green_time = null;
            string north_priority = null;
            string east_green_time = null;
            string east_priority = null;
            string south_green_time = null;
            string south_priority = null;
            string west_green_time = null;
            string west_priority = null;

            int turn = 0;

            while (true)
            {

                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;

                turn++;

                if (turn == 1)
                {
                    north_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 2)
                {
                    north_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("North Green Time: {0}, {1}", north_green_time, north_priority);
                }

                else if (turn == 3)
                {
                    east_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 4)
                {
                    east_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("East Green Time: {0}, {1}", east_green_time, east_priority);
                }

                else if (turn == 5)
                {
                    south_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 6)
                {
                    south_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("South Green Time: {0}, {1}", south_green_time, south_priority);
                }

                else if (turn == 7)
                {
                    west_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 8)
                {
                    west_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("West Green Time: {0}, {1}", west_green_time, west_priority);

                    turn = 0;
                    north_green_time = null;
                    north_priority = null;
                    east_green_time = null;
                    east_priority = null;
                    south_green_time = null;
                    south_priority = null;
                    west_green_time = null;
                    west_priority = null;
                }

                Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}

