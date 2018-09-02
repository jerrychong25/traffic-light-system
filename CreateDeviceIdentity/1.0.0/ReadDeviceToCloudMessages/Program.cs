using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.ServiceBus.Messaging;

// Declare Junction Structure
struct Junction
{
    public string north_traffic;
    public int north_green_time;
    public string east_traffic;
    public int east_green_time;
    public string south_traffic;
    public int south_green_time;
    public string west_traffic;
    public int west_green_time;
};

namespace ReadDeviceToCloudMessages
{
    class Program
    {
        // Global Parameters
        float[] ratio = new float[4];
        int[] ratio_priority = new int[4] { 1, 2, 3, 4 };

        static string connectionString = "HostName=trafficIoT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=gnpmxqawgwF4uybTebpFsh3XpFvHq5XbA40I5RIjNNA=";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;
        static void Main(string[] args)
        {
            Console.WriteLine("Receive messages\n");
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

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

                Program j1 = new Program();
                //Program j2 = new Program();
                Junction Junction1;
                //Junction Junction2;
                
                //string deviceID = test.deviceId;

                // Junction 1
                Junction1.north_traffic = test.nt1;
                Junction1.north_green_time = 0;
                Junction1.east_traffic = test.et1;
                Junction1.east_green_time = 0;
                Junction1.south_traffic = test.st1;
                Junction1.south_green_time = 0;
                Junction1.west_traffic = test.wt1;
                Junction1.west_green_time = 0;

                Console.WriteLine(test.nt1);
                Console.WriteLine(test.et1);
                Console.WriteLine(test.st1);
                Console.WriteLine(test.wt1);

                Junction1 = j1.Algorithm(Junction1);

                int i1 = 0;

                for (i1 = 0; i1 < 3; i1++)
                {
                    if (j1.ratio_priority[i1] == 1)
                    {
                        string j1_north_time = Convert.ToString(Junction1.north_green_time);
                        Console.WriteLine(string.Format("North Green Time: {0}", j1_north_time));
                    }

                    else if (j1.ratio_priority[i1] == 2)
                    {
                        string j1_east_time = Convert.ToString(Junction1.east_green_time);
                        Console.WriteLine(string.Format("East Green Time: {0}", j1_east_time));
                    }

                    else if (j1.ratio_priority[i1] == 3)
                    {
                        string j1_south_time = Convert.ToString(Junction1.south_green_time);
                        Console.WriteLine(string.Format("South Green Time: {0}", j1_south_time));
                    }

                    else if (j1.ratio_priority[i1] == 4)
                    {
                        string j1_west_time = Convert.ToString(Junction1.west_green_time);
                        Console.WriteLine(string.Format("West Green Time: {0}", j1_west_time));
                    }
                }

                //Console.WriteLine(string.Format("North Green Time: {0}", Junction1.north_green_time));
                //Console.WriteLine(string.Format("East Green Time: {0}", Junction1.east_green_time));
                //Console.WriteLine(string.Format("South Green Time: {0}", Junction1.south_green_time));
                //Console.WriteLine(string.Format("West Green Time: {0}", Junction1.west_green_time));

                //// Junction 2
                //Junction2.north_traffic = test.nt2;
                //Junction2.north_green_time = 0;
                //Junction2.east_traffic = test.et2;
                //Junction2.east_green_time = 0;
                //Junction2.south_traffic = test.st2;
                //Junction2.south_green_time = 0;
                //Junction2.west_traffic = test.wt2;
                //Junction2.west_green_time = 0;

                //Junction2 = j2.Algorithm(Junction2);

                //int i2 = 0;

                //for (i2 = 0; i2 < 3; i2++)
                //{
                //    if (j2.ratio_priority[i2] == 1)
                //    {
                //        Console.WriteLine(string.Format("North Green Time: {0}", Junction2.north_green_time));
                //    }

                //    else if (j2.ratio_priority[i2] == 2)
                //    {
                //        Console.WriteLine(string.Format("East Green Time: {0}", Junction2.east_green_time));
                //    }

                //    else if (j2.ratio_priority[i2] == 3)
                //    {
                //        Console.WriteLine(string.Format("South Green Time: {0}", Junction2.south_green_time));
                //    }

                //    else if (j2.ratio_priority[i2] == 4)
                //    {
                //        Console.WriteLine(string.Format("West Green Time: {0}", Junction2.west_green_time));
                //    }
                //}
            }
        }

        Junction Algorithm(Junction junc_traffic)
        {
            /* Setup your example here, code that should run once
             */
            int junc_north, junc_east, junc_south, junc_west;
            int volume1, volume2, volume3, volume4;
            float ratio1, ratio2, ratio3, ratio4;
            int effective1, effective2, effective3, effective4;
            int green1, green2, green3, green4;
            int res1, res2, res3;
            int cycle = 300;                        // One Complete Cycle = 300 Seconds
            int max_flow_rate = 50;
            int i = 0;

            Program j3 = new Program();

            junc_north = Convert.ToInt32(junc_traffic.north_traffic);
            junc_east = Convert.ToInt32(junc_traffic.east_traffic);
            junc_south = Convert.ToInt32(junc_traffic.south_traffic);
            junc_west = Convert.ToInt32(junc_traffic.west_traffic);

            /* Code in this loop will run repeatedly
             */
            volume1 = current_traffic_volume(junc_north, cycle);
            volume2 = current_traffic_volume(junc_east, cycle);
            volume3 = current_traffic_volume(junc_south, cycle);
            volume4 = current_traffic_volume(junc_west, cycle);

            ratio1 = flow_ratio(volume1, max_flow_rate);
            ratio2 = flow_ratio(volume2, max_flow_rate);
            ratio3 = flow_ratio(volume3, max_flow_rate);
            ratio4 = flow_ratio(volume4, max_flow_rate);

            ratio[0] = compare(ratio1, ratio2, ratio3, ratio4);     // Max Ratio

            effective1 = effective_green_time(ratio[0], cycle);

            green1 = phase_green_time(effective1);

            res1 = remaining_green_time(effective1, green1);

            effective2 = remaining_effective_green_time(res1, ratio[1], ratio[0], 1);

            green2 = phase_green_time(effective2);

            res2 = remaining_green_time(effective2, green2);

            effective3 = remaining_effective_green_time(res2, ratio[2], ratio[1], 2);

            green3 = phase_green_time(effective3);

            res3 = remaining_green_time(effective3, green3);

            effective4 = remaining_effective_green_time(res3, ratio[3], ratio[2], 3);

            green4 = phase_green_time(effective4);

            for (i = 0; i < 3; i++)
            {
                if (j3.ratio_priority[i] == 1)
                {
                    junc_traffic.north_green_time = green1;
                }

                else if (j3.ratio_priority[i] == 2)
                {
                    junc_traffic.east_green_time = green2;
                }

                else if (j3.ratio_priority[i] == 3)
                {
                    junc_traffic.south_green_time = green3;
                }

                else if (j3.ratio_priority[i] == 4)
                {
                    junc_traffic.west_green_time = green4;
                }
            }

            return junc_traffic;
        }

        int current_traffic_volume(int num_vechicle, int C)
        {
            int Q_RT;

            Q_RT = (num_vechicle * 3600) / C;
            //Q_RT = (num_vechicle) / C;

            return Q_RT;
        }

        int flow_ratio(int Q_RT, int FS)
        {
            int ratio;

            ratio = (Q_RT / FS);

            return ratio;
        }

        int effective_green_time(float flow_ratio, int C)
        {
            int VE;

            VE = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(flow_ratio * C)));

            return VE;
        }

        int phase_green_time(int VE)
        {
            int V;
            int P = 2;
            int G = 3;

            V = VE + P - G;

            return V;
        }

        int remaining_green_time(int VE, int V)
        {
            int Vres;

            Vres = VE - V;

            return Vres;
        }

        int remaining_effective_green_time(int Vres, float flow_ratio_i, float flow_ratio_j, int n)
        {
            int i;
            int VE_res;
            float flow_ratio_k = 0;

            for (i = n; i < 4; i++)
            {
                flow_ratio_k += ratio[i];           // Got problem!!!!
            }

            if (n == 4)
            {
                decimal temp_Vres = Math.Truncate(Convert.ToDecimal(Vres));
                VE_res = Convert.ToInt32(temp_Vres);
            }

            else
            {
                VE_res = Vres * Convert.ToInt32(flow_ratio_i / (flow_ratio_k - flow_ratio_j));
            }

            return VE_res;
        }

        float compare(float first, float second, float third, float fourth)
        {
            int i;
            float temp_value, final;
            int temp_priority;

            ratio[0] = first;
            ratio[1] = second;
            ratio[2] = third;
            ratio[3] = fourth;

            for (i = 0; i < 3; i++)
            {
                if (ratio[i] < ratio[i + 1])
                {
                    temp_value = ratio[i];
                    ratio[i] = ratio[i + 1];
                    ratio[i + 1] = temp_value;

                    temp_priority = ratio_priority[i];
                    ratio_priority[i] = ratio_priority[i + 1];
                    ratio_priority[i + 1] = temp_priority;
                }
            }

            final = ratio[0];

            return final;
        }
    }
}

