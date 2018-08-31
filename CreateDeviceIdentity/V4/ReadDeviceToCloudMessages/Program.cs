using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;      // Added from SendCloudToDevice
using Newtonsoft.Json;
using Microsoft.ServiceBus.Messaging;

// Declare Junction Structure
struct Junction
{
    public string north_traffic;
    public int north_green_time;
    public int north_priority;
    public string east_traffic;
    public int east_green_time;
    public int east_priority;
    public string south_traffic;
    public int south_green_time;
    public int south_priority;
    public string west_traffic;
    public int west_green_time;
    public int west_priority;
};

// Declare Ratio Structure
public struct Ratio
{
    public Ratio(int charArraySize) : this()
    {
        priority1 = new int[charArraySize];
        priority2 = new int[charArraySize];
        flow_value = new float[charArraySize];
        percentage_value = new float[charArraySize];
    }

    public int[] priority1;
    public int[] priority2;
    public float[] flow_value;
    public float[] percentage_value;
}

namespace ReadDeviceToCloudMessages
{

    class Program
    {
        // Added from SendCloudToDevice
        static ServiceClient serviceClient;

        // Original from ReadDeviceToCloudMessages
        static string connectionString = "HostName=trafficIoT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=gnpmxqawgwF4uybTebpFsh3XpFvHq5XbA40I5RIjNNA=";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;
        static void Main(string[] args)
        {
            Console.WriteLine("Receive messages\n");
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            //// Added from SendCloudToDevice
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            foreach (string partition in d2cPartitions)
            {
                ReceiveMessagesFromDeviceAsync(partition);
            }
            Console.ReadLine();
        }

        private async static Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);

            while (true)
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine(string.Format("Message received. Partition: {0} Data: '{1}'", partition, data));
                dynamic test = JsonConvert.DeserializeObject(data);

                //Program j1 = new Program();
                //Program j2 = new Program();
                Junction Junction1;
                Junction Junction2;

                //string deviceID = test.deviceId;

                // Junction 1 Variables Initialization
                Junction1.north_traffic = test.nt1;
                Junction1.north_green_time = 0;
                Junction1.north_priority = 0;
                Junction1.east_traffic = test.et1;
                Junction1.east_green_time = 0;
                Junction1.east_priority = 0;
                Junction1.south_traffic = test.st1;
                Junction1.south_green_time = 0;
                Junction1.south_priority = 0;
                Junction1.west_traffic = test.wt1;
                Junction1.west_green_time = 0;
                Junction1.west_priority = 0;

                //// Show Received Messages 1
                //Console.WriteLine(test.nt1);
                //Console.WriteLine(test.et1);
                //Console.WriteLine(test.st1);
                //Console.WriteLine(test.wt1);
                //Console.WriteLine(test.nt2);
                //Console.WriteLine(test.et2);
                //Console.WriteLine(test.st2);
                //Console.WriteLine(test.wt2);

                //// Show Received Messages 2
                //Console.WriteLine(Junction1.north_traffic);
                //Console.WriteLine(Junction1.east_traffic);
                //Console.WriteLine(Junction1.south_traffic);
                //Console.WriteLine(Junction1.west_traffic);
                //Console.WriteLine(test.nt2);
                //Console.WriteLine(test.et2);
                //Console.WriteLine(test.st2);
                //Console.WriteLine(test.wt2);

                Junction1 = Algorithm(Junction1);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("North 1 Green Time: {0}, {1}", Junction1.north_green_time, Junction1.north_priority));
                Console.WriteLine(string.Format("East 1 Green Time: {0}, {1}", Junction1.east_green_time, Junction1.east_priority));
                Console.WriteLine(string.Format("South 1 Green Time: {0}, {1}", Junction1.south_green_time, Junction1.south_priority));
                Console.WriteLine(string.Format("West 1 Green Time: {0}, {1}", Junction1.west_green_time, Junction1.west_priority));
                Console.WriteLine(string.Format("----------------------------"));

                SendCloudToDeviceMessageAsync(Junction1).Wait();

                // Junction 2 Variables Initialization
                Junction2.north_traffic = test.nt2;
                Junction2.north_green_time = 0;
                Junction2.north_priority = 0;
                Junction2.east_traffic = test.et2;
                Junction2.east_green_time = 0;
                Junction2.east_priority = 0;
                Junction2.south_traffic = test.st2;
                Junction2.south_green_time = 0;
                Junction2.south_priority = 0;
                Junction2.west_traffic = test.wt2;
                Junction2.west_green_time = 0;
                Junction2.west_priority = 0;

                Junction2 = Algorithm(Junction2);

                Console.WriteLine(string.Format("North 2 Green Time: {0}, {1}", Junction2.north_green_time, Junction2.north_priority));
                Console.WriteLine(string.Format("East 2 Green Time: {0}, {1}", Junction2.east_green_time, Junction2.east_priority));
                Console.WriteLine(string.Format("South 2 Green Time: {0}, {1}", Junction2.south_green_time, Junction2.south_priority));
                Console.WriteLine(string.Format("West 2 Green Time: {0}, {1}", Junction2.west_green_time, Junction2.west_priority));
                Console.ResetColor();

                SendCloudToDeviceMessageAsync(Junction2).Wait();
            }
        }

        static Junction Algorithm(Junction junc_traffic)
        {
            /* Setup your example here, code that should run once
             */
            int junc_north, junc_east, junc_south, junc_west;
            int[] volume = new int[4];
            var ratio = new Ratio(4);
            int[] effective = new int[4];
            int[] green = new int[4];
            int[] res = new int[3];
            int cycle = 300;                        // One Complete Cycle = 300 Seconds
            int max_flow_rate = 700;
            int i = 0;
            int x1, x2, y1, y2, temp1, temp2, temp3, temp4, temp5, temp6;

            // Initialize Variables
            //ratio.priority1[0] = 1;
            //ratio.priority1[1] = 2;
            //ratio.priority1[2] = 3;
            //ratio.priority1[3] = 4;
            //ratio.flow_value[0] = 0;
            //ratio.flow_value[1] = 0;
            //ratio.flow_value[2] = 0;
            //ratio.flow_value[3] = 0;
            //ratio.percentage_value[0] = 0;
            //ratio.percentage_value[1] = 0;
            //ratio.percentage_value[2] = 0;
            //ratio.percentage_value[4] = 0;

            junc_north = Convert.ToInt32(junc_traffic.north_traffic);
            junc_east = Convert.ToInt32(junc_traffic.east_traffic);
            junc_south = Convert.ToInt32(junc_traffic.south_traffic);
            junc_west = Convert.ToInt32(junc_traffic.west_traffic);

            /* Code in this loop will run repeatedly
             */
            volume[0] = current_traffic_volume(junc_north, cycle);
            volume[1] = current_traffic_volume(junc_east, cycle);
            volume[2] = current_traffic_volume(junc_south, cycle);
            volume[3] = current_traffic_volume(junc_west, cycle);

            ratio.flow_value[0] = flow_ratio(volume[0], max_flow_rate);
            ratio.flow_value[1] = flow_ratio(volume[1], max_flow_rate);
            ratio.flow_value[2] = flow_ratio(volume[2], max_flow_rate);
            ratio.flow_value[3] = flow_ratio(volume[3], max_flow_rate);

            ratio.percentage_value[0] = (ratio.flow_value[0] / (ratio.flow_value[0] + ratio.flow_value[1] + ratio.flow_value[2] + ratio.flow_value[3]));
            ratio.percentage_value[1] = (ratio.flow_value[1] / (ratio.flow_value[0] + ratio.flow_value[1] + ratio.flow_value[2] + ratio.flow_value[3]));
            ratio.percentage_value[2] = (ratio.flow_value[2] / (ratio.flow_value[0] + ratio.flow_value[1] + ratio.flow_value[2] + ratio.flow_value[3]));
            ratio.percentage_value[3] = (ratio.flow_value[3] / (ratio.flow_value[0] + ratio.flow_value[1] + ratio.flow_value[2] + ratio.flow_value[3]));

            //ratio[0] = compare(ratio1, ratio2, ratio3, ratio4);     // Max Ratio
            ratio = compare(ratio);     // Max Ratio

            effective[0] = effective_green_time(ratio.percentage_value[0], cycle);

            green[0] = phase_green_time(effective[0]);

            res[0] = remaining_green_time(cycle, green[0]);

            effective[1] = remaining_effective_green_time(res[0], ratio.percentage_value[1], ratio.percentage_value[0], 0, ratio.percentage_value);

            green[1] = phase_green_time(effective[1]);

            res[1] = remaining_green_time(res[0], green[1]);

            effective[2] = remaining_effective_green_time(res[1], ratio.percentage_value[2], ratio.percentage_value[1], 1, ratio.percentage_value);

            green[2] = phase_green_time(effective[2]);

            res[2] = remaining_green_time(res[1], green[2]);

            effective[3] = remaining_effective_green_time(res[2], ratio.percentage_value[3], ratio.percentage_value[2], 2, ratio.percentage_value);

            green[3] = phase_green_time(effective[3]);

            for (x1 = 0; x1 < 3; x1++)
            {
                for (y1 = 0; y1 < 3; y1++)
                {
                    if (green[y1] < green[y1 + 1])
                    {
                        temp1 = green[y1];
                        green[y1] = green[y1 + 1];
                        green[y1] = temp1;

                        temp2 = ratio.priority1[y1];
                        ratio.priority1[y1] = ratio.priority1[y1 + 1];
                        ratio.priority1[y1 + 1] = temp2;

                        temp3 = ratio.priority2[y1];
                        ratio.priority2[y1] = ratio.priority2[y1 + 1];
                        ratio.priority2[y1 + 1] = temp3;
                    }
                }
            }

            for (x2 = 0; x2 < 3; x2++)
            {
                for (y2 = 0; y2 < 3; y2++)
                {
                    if (ratio.priority1[y2] > ratio.priority1[y2 + 1])
                    {
                        temp4 = green[y2];
                        green[y2] = green[y2 + 1];
                        green[y2 + 1] = temp4;

                        temp5 = ratio.priority1[y2];
                        ratio.priority1[y2] = ratio.priority1[y2 + 1];
                        ratio.priority1[y2 + 1] = temp5;

                        temp6 = ratio.priority2[y2];
                        ratio.priority2[y2] = ratio.priority2[y2 + 1];
                        ratio.priority2[y2 + 1] = temp6;
                    }
                }
            }

            junc_traffic.north_priority = ratio.priority2[0];
            junc_traffic.east_priority = ratio.priority2[1];
            junc_traffic.south_priority = ratio.priority2[2];
            junc_traffic.west_priority = ratio.priority2[3];

            for (i = 0; i < 4; i++)
            {
                if (ratio.priority1[i] == 1)
                {
                    junc_traffic.north_green_time = green[0];
                }

                else if (ratio.priority1[i] == 2)
                {
                    junc_traffic.east_green_time = green[1];
                }

                else if (ratio.priority1[i] == 3)
                {
                    junc_traffic.south_green_time = green[2];
                }

                else if (ratio.priority1[i] == 4)
                {
                    junc_traffic.west_green_time = green[3];
                }
            }

            return junc_traffic;
        }

        static int current_traffic_volume(int num_vechicle, int C)
        {
            int Q_RT;

            Q_RT = (num_vechicle * 3600) / C;
            //Q_RT = (num_vechicle / C);

            return Q_RT;
        }

        static float flow_ratio(int Q_RT, int FS)
        {
            float ratio;

            ratio = Convert.ToSingle((float)Q_RT / (float)FS);

            return ratio;
        }

        static int effective_green_time(float flow_ratio, int C)
        {
            int VE;

            VE = Convert.ToInt32(Math.Round((flow_ratio * C)));

            return VE;
        }

        static int phase_green_time(int VE)
        {
            int V;
            int P = 2;
            int G = 2;

            V = VE + P - G;

            return V;
        }

        static int remaining_green_time(int VE, int V)
        {
            int Vres;

            Vres = VE - V;

            return Vres;
        }

        static int remaining_effective_green_time(int Vres, float flow_ratio_i, float flow_ratio_j, int n, float[] ratio)
        {
            int i;
            int VE_res;
            float flow_ratio_k = 0;

            for (i = n; i < 4; i++)
            {
                flow_ratio_k = (flow_ratio_k + ratio[i]);           // Got problem!!!!
            }

            if (n == 2)
            {
                decimal temp_Vres = Math.Round(Convert.ToDecimal(Vres));
                VE_res = Convert.ToInt32(temp_Vres);
            }

            //else if (n == 2)
            //{
            //    VE_res = Convert.ToInt32(Vres * (flow_ratio_i / flow_ratio_k));
            //}

            else
            {
                VE_res = Convert.ToInt32(Math.Round(Vres * (flow_ratio_i / (flow_ratio_k - flow_ratio_j))));
            }

            return VE_res;
        }

        static Ratio compare(Ratio ratio)
        {
            int i, n, z, y;
            float temp_value;
            int temp_priority;
            // float final;
            int[] temp_ratio_priority1 = new int[4];
            int[] temp_ratio_priority2 = new int[4];
            float[] temp_ratio_percentage_value = new float[4];

            ratio.priority1[0] = 1;
            ratio.priority1[1] = 2;
            ratio.priority1[2] = 3;
            ratio.priority1[3] = 4;

            ratio.priority2[0] = 1;
            ratio.priority2[1] = 2;
            ratio.priority2[2] = 3;
            ratio.priority2[3] = 4;

            temp_ratio_priority2[0] = 1;
            temp_ratio_priority2[1] = 2;
            temp_ratio_priority2[2] = 3;
            temp_ratio_priority2[3] = 4;

            for (n = 0; n < 3; n++)
            {
                for (i = 0; i < 3; i++)
                {
                    if (ratio.percentage_value[i] < ratio.percentage_value[i + 1])
                    {
                        temp_value = ratio.percentage_value[i];
                        ratio.percentage_value[i] = ratio.percentage_value[i + 1];
                        ratio.percentage_value[i + 1] = temp_value;

                        temp_priority = ratio.priority1[i];
                        ratio.priority1[i] = ratio.priority1[i + 1];
                        ratio.priority1[i + 1] = temp_priority;
                    }
                }
            }

            return ratio;
        }

        private async static Task SendCloudToDeviceMessageAsync(Junction junction)
        {
            var north_green_time = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.north_green_time)));
            north_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", north_green_time);

            var north_priority = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.north_priority)));
            north_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", north_priority);

            var east_green_time = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.east_green_time)));
            east_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", east_green_time);

            var east_priority = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.east_priority)));
            east_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", east_priority);

            var south_green_time = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.south_green_time)));
            south_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", south_green_time);

            var south_priority = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.south_priority)));
            south_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", south_priority);

            var west_green_time = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.west_green_time)));
            west_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", west_green_time);

            var west_priority = new Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.west_priority)));
            west_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", west_priority);
        }
    }
}
