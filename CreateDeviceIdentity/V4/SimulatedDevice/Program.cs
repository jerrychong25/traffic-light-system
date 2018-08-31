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

                Thread.Sleep(4000);
            }
        }

        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");

            // Junction 1 Parameters
            string north1_green_time = null;
            string north1_priority = null;
            string east1_green_time = null;
            string east1_priority = null;
            string south1_green_time = null;
            string south1_priority = null;
            string west1_green_time = null;
            string west1_priority = null;

            // Junction 2 Parameters
            string north2_green_time = null;
            string north2_priority = null;
            string east2_green_time = null;
            string east2_priority = null;
            string south2_green_time = null;
            string south2_priority = null;
            string west2_green_time = null;
            string west2_priority = null;

            int turn = 0;

            while (true)
            {

                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;

                turn++;

                if (turn == 1)
                {
                    north1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 2)
                {
                    north1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("North 1 Green Time: {0}, {1}", north1_green_time, north1_priority);
                }

                else if (turn == 3)
                {
                    east1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 4)
                {
                    east1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("East 1 Green Time: {0}, {1}", east1_green_time, east1_priority);
                }

                else if (turn == 5)
                {
                    south1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 6)
                {
                    south1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("South 1 Green Time: {0}, {1}", south1_green_time, south1_priority);
                }

                else if (turn == 7)
                {
                    west1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 8)
                {
                    west1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("West 1 Green Time: {0}, {1}", west1_green_time, west1_priority);
                    Console.WriteLine("----------------------------");
                }

                else if (turn == 9)
                {
                    north2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 10)
                {
                    north2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("North 2 Green Time: {0}, {1}", north2_green_time, north2_priority);
                }

                else if (turn == 11)
                {
                    east2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 12)
                {
                    east2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("East 2 Green Time: {0}, {1}", east2_green_time, east2_priority);
                }

                else if (turn == 13)
                {
                    south2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 14)
                {
                    south2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("South 2 Green Time: {0}, {1}", south2_green_time, south2_priority);
                }

                else if (turn == 15)
                {
                    west2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 16)
                {
                    west2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("West 2 Green Time: {0}, {1}", west2_green_time, west2_priority);

                    turn = 0;
                    north1_green_time = null;
                    north1_priority = null;
                    east1_green_time = null;
                    east1_priority = null;
                    south1_green_time = null;
                    south1_priority = null;
                    west1_green_time = null;
                    west1_priority = null;
                    north2_green_time = null;
                    north2_priority = null;
                    east2_green_time = null;
                    east2_priority = null;
                    south2_green_time = null;
                    south2_priority = null;
                    west2_green_time = null;
                    west2_priority = null;
                }

                Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}

