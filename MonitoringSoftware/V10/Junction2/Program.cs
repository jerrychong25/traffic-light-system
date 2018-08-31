using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace Junction1
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "trafficIoT.azure-devices.net";
        static string deviceKey = "opjcPjYT2qFnZN/mY0AVp9WyVwo7W5iSBEnE70NM6R4=";

        static void Main(string[] args)
        {
            Console.WriteLine("Junction 2\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Lenovo02", deviceKey));

            SendDeviceToCloudMessagesAsync();
            ReceiveC2dAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            while (true)
            {
                //int north_traffic1 = rand.Next(1, 50);
                //int east_traffic1 = rand.Next(1, 50);
                //int south_traffic1 = rand.Next(1, 50);
                //int west_traffic1 = rand.Next(1, 50);
                Thread.Sleep(100);
                Random rand = new Random();
                int north_traffic2 = rand.Next(1, 50);
                int east_traffic2 = rand.Next(1, 50);
                int south_traffic2 = rand.Next(1, 50);
                int west_traffic2 = rand.Next(1, 50);

                var telemetryDataPoint = new
                {
                    // deviceId = "myFirstDevice",
                    deviceId = "Lenovo02",
                    //nt1 = north_traffic1,
                    //et1 = east_traffic1,
                    //st1 = south_traffic1,
                    //wt1 = west_traffic1,
                    nt2 = north_traffic2,
                    et2 = east_traffic2,
                    st2 = south_traffic2,
                    wt2 = west_traffic2,
                };

                Thread.Sleep(5000);

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nSending Device Messages To Microsoft Azure Cloud Server...");
                //Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                Console.WriteLine("Sent Time: {0}", DateTime.Now);
                //Console.WriteLine("Junction 1 North Traffic: {0}", north_traffic1);
                //Console.WriteLine("Junction 1 East Traffic: {0}", east_traffic1);
                //Console.WriteLine("Junction 1 South Traffic: {0}", south_traffic1);
                //Console.WriteLine("Junction 1 West Traffic: {0}", west_traffic1);
                //Console.WriteLine("----------------------------");
                Console.WriteLine("Junction 2 North Traffic: {0}", north_traffic2);
                Console.WriteLine("Junction 2 East Traffic: {0}", east_traffic2);
                Console.WriteLine("Junction 2 South Traffic: {0}", south_traffic2);
                Console.WriteLine("Junction 2 West Traffic: {0}", west_traffic2);
                Console.ResetColor();

                Thread.Sleep(27000);
            }
        }

        private static async void ReceiveC2dAsync()
        {
            //Console.WriteLine("\nSending device messages to Microsoft Azure cloud server");

            //// Junction 1 Parameters
            //string north1_green_time = null;
            //string north1_priority = null;
            //string east1_green_time = null;
            //string east1_priority = null;
            //string south1_green_time = null;
            //string south1_priority = null;
            //string west1_green_time = null;
            //string west1_priority = null;

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

                //if (turn == 1)
                //{
                //    north1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 2)
                //{
                //    north1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("North 1 Green Time: {0}, {1}", north1_green_time, north1_priority);
                //}

                //else if (turn == 3)
                //{
                //    east1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 4)
                //{
                //    east1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("East 1 Green Time: {0}, {1}", east1_green_time, east1_priority);
                //}

                //else if (turn == 5)
                //{
                //    south1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 6)
                //{
                //    south1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("South 1 Green Time: {0}, {1}", south1_green_time, south1_priority);
                //}

                //else if (turn == 7)
                //{
                //    west1_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 8)
                //{
                //    west1_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("West 1 Green Time: {0}, {1}", west1_green_time, west1_priority);
                //    Console.WriteLine("----------------------------");
                //}

                //else if (turn == 9)
                //{
                //    north2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 10)
                //{
                //    north2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("North 2 Green Time: {0}, {1}", north2_green_time, north2_priority);
                //}

                //else if (turn == 11)
                //{
                //    east2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 12)
                //{
                //    east2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("East 2 Green Time: {0}, {1}", east2_green_time, east2_priority);
                //}

                //else if (turn == 13)
                //{
                //    south2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 14)
                //{
                //    south2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("South 2 Green Time: {0}, {1}", south2_green_time, south2_priority);
                //}

                //else if (turn == 15)
                //{
                //    west2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //}

                //else if (turn == 16)
                //{
                //    west2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //    Console.WriteLine("West 2 Green Time: {0}, {1}", west2_green_time, west2_priority);

                //    turn = 0;
                //    north1_green_time = null;
                //    north1_priority = null;
                //    east1_green_time = null;
                //    east1_priority = null;
                //    south1_green_time = null;
                //    south1_priority = null;
                //    west1_green_time = null;
                //    west1_priority = null;
                //    north2_green_time = null;
                //    north2_priority = null;
                //    east2_green_time = null;
                //    east2_priority = null;
                //    south2_green_time = null;
                //    south2_priority = null;
                //    west2_green_time = null;
                //    west2_priority = null;
                //}

                if (turn == 1)
                {
                    north2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 2)
                {
                    north2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 3)
                {
                    east2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 4)
                {
                    east2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 5)
                {
                    south2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 6)
                {
                    south2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 7)
                {
                    west2_green_time = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                }

                else if (turn == 8)
                {
                    west2_priority = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                    Console.WriteLine("\nReceiving Device Messages From Microsoft Azure Cloud Server...");
                    Console.WriteLine("Received Time: {0}", DateTime.Now);
                    //Console.WriteLine("Junction 1 North Green Time: {0}, {1}", north1_green_time, north1_priority);
                    //Console.WriteLine("Junction 1 East Green Time: {0}, {1}", east1_green_time, east1_priority);
                    //Console.WriteLine("Junction 1 South Green Time: {0}, {1}", south1_green_time, south1_priority);
                    //Console.WriteLine("Junction 1 West Green Time: {0}, {1}", west1_green_time, west1_priority);
                    //Console.WriteLine("----------------------------");
                    //Console.WriteLine("Junction 2 North Green Time: {0}, {1}", north2_green_time, north2_priority);
                    //Console.WriteLine("Junction 2 East Green Time: {0}, {1}", east2_green_time, east2_priority);
                    //Console.WriteLine("Junction 2 South Green Time: {0}, {1}", south2_green_time, south2_priority);
                    //Console.WriteLine("Junction 2 West Green Time: {0}, {1}", west2_green_time, west2_priority);
                    //Console.WriteLine("Junction 1 North Green Time: {0}", north1_green_time);
                    //Console.WriteLine("Junction 1 North Priority: {0}", north1_priority);
                    //Console.WriteLine("Junction 1 East Green Time: {0}", east1_green_time);
                    //Console.WriteLine("Junction 1 East Priority: {0}", east1_priority);
                    //Console.WriteLine("Junction 1 South Green Time: {0}", south1_green_time);
                    //Console.WriteLine("Junction 1 South Priority: {0}", south1_priority);
                    //Console.WriteLine("Junction 1 West Green Time: {0}", west1_green_time);
                    //Console.WriteLine("Junction 1 West Priority: {0}", west1_priority);
                    //Console.WriteLine("----------------------------");
                    Console.WriteLine("Junction 2 North Green Time: {0}", north2_green_time);
                    Console.WriteLine("Junction 2 North Priority: {0}", north2_priority);
                    Console.WriteLine("Junction 2 East Green Time: {0}", east2_green_time);
                    Console.WriteLine("Junction 2 East Priority: {0}", east2_priority);
                    Console.WriteLine("Junction 2 South Green Time: {0}", south2_green_time);
                    Console.WriteLine("Junction 2 South Priority: {0}", south2_priority);
                    Console.WriteLine("Junction 2 West Green Time: {0}", west2_green_time);
                    Console.WriteLine("Junction 2 West Priority: {0}", west2_priority);

                    turn = 0;
                    //north1_green_time = null;
                    //north1_priority = null;
                    //east1_green_time = null;
                    //east1_priority = null;
                    //south1_green_time = null;
                    //south1_priority = null;
                    //west1_green_time = null;
                    //west1_priority = null;
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
