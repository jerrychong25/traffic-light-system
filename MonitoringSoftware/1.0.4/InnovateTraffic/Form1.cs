using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
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

namespace InnovateTraffic
{
    public partial class Form1 : Form
    {
        // Added from SendCloudToDevice
        static ServiceClient serviceClient;

        // Original from ReadDeviceToCloudMessages
        static string connectionString = "HostName=trafficIoT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=gnpmxqawgwF4uybTebpFsh3XpFvHq5XbA40I5RIjNNA=";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;

        //private int _blink = 0;

        // Global Parameters
        int total_time;
        static int receive_status;

        // Junction 1 Parameters
        static byte[] junc1_priority = new byte[4];
        byte[] junc1_direction = new byte[4] { 1, 2, 3, 4 };
        byte junc1_temp_priority;
        static int[] junc1_green_time = new int[4];
        int[] junc1_remaining_green_time = new int[4];

        // Junction 2 Parameters
        static byte[] junc2_priority = new byte[4];
        byte[] junc2_direction = new byte[4] { 1, 2, 3, 4 };
        byte junc2_temp_priority;
        static int[] junc2_green_time = new int[4];
        int[] junc2_remaining_green_time = new int[4];

        //class junction
        //{
        //    byte[] priority = new byte[4];
        //    byte[] direction = new byte[4] { 1, 2, 3, 4 };
        //    byte temp_priority;
        //    int[] temp_time = new int[4];
        //    int[] remaining_green_time = new int[4];
        //}

        public Form1()
        {
            InitializeComponent();
            //TrafficLight_Init();
            Azure_Main();
        }

        void Azure_Main()
        {
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            // Added from SendCloudToDevice
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            foreach (string partition in d2cPartitions)
            {
                ReceiveMessagesFromDeviceAsync(partition);
            }
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);

            while (true)
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                dynamic test = JsonConvert.DeserializeObject(data);

                Junction Junction1;
                Junction Junction2;

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

                Junction1 = Algorithm(Junction1);

                SendCloudToDeviceMessageAsync(Junction1);

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

                SendCloudToDeviceMessageAsync(Junction2);

                receive_status = TrafficLight_Init(Junction1, Junction2);

                Main();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Turn the bulb On or Off
        private void ledBulb_Click(object sender, EventArgs e)
        {
            //((LedBulb)sender).On = !((LedBulb)sender).On;
        }

        private void ledBulb1_Click(object sender, EventArgs e)
        {
            //if (_blink == 0) _blink = 500;
            //else _blink = 0;
            //((LedBulb)sender).Blink(_blink);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            byte i;
            int counter1, counter2;
            int initial1 = 0;
            int initial2 = 0;

            while (receive_status == 0)
            {
                await Task.Delay(100);
            }

            Priority_Sort();
            show_green_time();
            calc_total_time();
            calc_remaining_time();
            show_remaining_time();

            int[] junc1_north_array = new int[total_time];
            int[] junc1_east_array = new int[total_time];
            int[] junc1_south_array = new int[total_time];
            int[] junc1_west_array = new int[total_time];
            int[] junc2_north_array = new int[total_time];
            int[] junc2_east_array = new int[total_time];
            int[] junc2_south_array = new int[total_time];
            int[] junc2_west_array = new int[total_time];

            for (i = 0; i < 4; i++)
            {

                // Junction 1 All Direction Array
                junc1_temp_priority = junc1_direction[i];

                if (junc1_temp_priority == 1)
                { 
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_north_array[counter1] = 3;
                        junc1_east_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_north_array[counter1] = 2;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_north_array[++counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 2)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_east_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_east_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_east_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 3)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_south_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_east_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_south_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_south_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 4)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_west_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_east_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                    }

                    junc1_west_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;

                    junc1_west_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;

                    initial1 = counter1;
                }

                // Junction 2 All Direction Array
                junc2_temp_priority = junc2_direction[i];

                if (junc2_temp_priority == 1)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_north_array[counter2] = 3;
                        junc2_east_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_north_array[counter2] = 2;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_north_array[++counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 2)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_east_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_east_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_east_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 3)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_south_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_east_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_south_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_south_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 4)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_west_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_east_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                    }

                    junc2_west_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;

                    junc2_west_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;

                    initial2 = counter2;
                }

            }

            for (i = 0; i < total_time; i++)
            {
                junc1_traffic_light(junc1_north_array[i], junc1_east_array[i], junc1_south_array[i], junc1_west_array[i]);
                junc2_traffic_light(junc2_north_array[i], junc2_east_array[i], junc2_south_array[i], junc2_west_array[i]);
                show_remaining_time();
                await Task.Delay(1000);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ledBulb1.On = false;
            ledBulb2.On = false;
            ledBulb3.On = false;
            ledBulb4.On = false;
            ledBulb5.On = false;
            ledBulb6.On = false;
            ledBulb7.On = false;
            ledBulb8.On = false;
            ledBulb9.On = false;
            ledBulb10.On = false;
            ledBulb11.On = false;
            ledBulb12.On = false;
            ledBulb13.On = false;
            ledBulb14.On = false;
            ledBulb15.On = false;
            ledBulb16.On = false;
            ledBulb17.On = false;
            ledBulb18.On = false;
            ledBulb19.On = false;
            ledBulb20.On = false;
            ledBulb21.On = false;
            ledBulb22.On = false;
            ledBulb23.On = false;
            ledBulb24.On = false;
        }

        async void Main()
        {
            byte i;
            int counter1, counter2;
            int initial1 = 0;
            int initial2 = 0;

            //sevenSegment1.ColorLight = Color.Red;
            //sevenSegment2.ColorLight = Color.Red;
            //sevenSegment3.ColorLight = Color.Red;
            //sevenSegment4.ColorLight = Color.Red;
            //sevenSegment5.ColorLight = Color.Red;
            //sevenSegment6.ColorLight = Color.Red;
            //sevenSegment7.ColorLight = Color.Red;
            //sevenSegment8.ColorLight = Color.Red;
            //sevenSegment9.ColorLight = Color.Red;
            //sevenSegment10.ColorLight = Color.Red;
            //sevenSegment11.ColorLight = Color.Red;
            //sevenSegment12.ColorLight = Color.Red;
            //sevenSegment13.ColorLight = Color.Red;
            //sevenSegment14.ColorLight = Color.Red;
            //sevenSegment15.ColorLight = Color.Red;
            //sevenSegment16.ColorLight = Color.Red;

            while (receive_status == 0)
            {
                await Task.Delay(100);
            }

            show_green_time();
            Priority_Sort();
            calc_total_time();
            calc_remaining_time();
            show_remaining_time();

            int[] junc1_north_array = new int[total_time];
            int[] junc1_east_array = new int[total_time];
            int[] junc1_south_array = new int[total_time];
            int[] junc1_west_array = new int[total_time];
            int[] junc2_north_array = new int[total_time];
            int[] junc2_east_array = new int[total_time];
            int[] junc2_south_array = new int[total_time];
            int[] junc2_west_array = new int[total_time];

            for (i = 0; i < 4; i++)
            {

                // Junction 1 All Direction Array
                junc1_temp_priority = junc1_direction[i];

                if (junc1_temp_priority == 1)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_north_array[counter1] = 3;
                        junc1_east_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_north_array[counter1] = 2;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_north_array[++counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 2)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_east_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_east_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_east_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 3)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_south_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_east_array[counter1] = 1;
                        junc1_west_array[counter1] = 1;
                    }

                    junc1_south_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    junc1_south_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_west_array[counter1] = 1;

                    initial1 = counter1;
                }

                if (junc1_temp_priority == 4)
                {
                    for (counter1 = initial1; counter1 < (initial1 + junc1_green_time[(junc1_temp_priority - 1)]); counter1++)
                    {
                        junc1_west_array[counter1] = 3;
                        junc1_north_array[counter1] = 1;
                        junc1_east_array[counter1] = 1;
                        junc1_south_array[counter1] = 1;
                    }

                    junc1_west_array[counter1] = 2;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;

                    junc1_west_array[++counter1] = 1;
                    junc1_north_array[counter1] = 1;
                    junc1_east_array[counter1] = 1;
                    junc1_south_array[counter1] = 1;

                    initial1 = counter1;
                }

                // Junction 2 All Direction Array
                junc2_temp_priority = junc2_direction[i];

                if (junc2_temp_priority == 1)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_north_array[counter2] = 3;
                        junc2_east_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_north_array[counter2] = 2;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_north_array[++counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 2)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_east_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_east_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_east_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 3)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_south_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_east_array[counter2] = 1;
                        junc2_west_array[counter2] = 1;
                    }

                    junc2_south_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    junc2_south_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_west_array[counter2] = 1;

                    initial2 = counter2;
                }

                if (junc2_temp_priority == 4)
                {
                    for (counter2 = initial2; counter2 < (initial2 + junc2_green_time[(junc2_temp_priority - 1)]); counter2++)
                    {
                        junc2_west_array[counter2] = 3;
                        junc2_north_array[counter2] = 1;
                        junc2_east_array[counter2] = 1;
                        junc2_south_array[counter2] = 1;
                    }

                    junc2_west_array[counter2] = 2;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;

                    junc2_west_array[++counter2] = 1;
                    junc2_north_array[counter2] = 1;
                    junc2_east_array[counter2] = 1;
                    junc2_south_array[counter2] = 1;

                    initial2 = counter2;
                }

            }

            for (i = 0; i < total_time; i++)
            {
                junc1_traffic_light(junc1_north_array[i], junc1_east_array[i], junc1_south_array[i], junc1_west_array[i]);
                junc2_traffic_light(junc2_north_array[i], junc2_east_array[i], junc2_south_array[i], junc2_west_array[i]);
                show_remaining_time();
                await Task.Delay(1000);
            }

            //Reset();
        }

        void Reset()
        {
            ledBulb1.On = false;
            ledBulb2.On = false;
            ledBulb3.On = false;
            ledBulb4.On = false;
            ledBulb5.On = false;
            ledBulb6.On = false;
            ledBulb7.On = false;
            ledBulb8.On = false;
            ledBulb9.On = false;
            ledBulb10.On = false;
            ledBulb11.On = false;
            ledBulb12.On = false;
            ledBulb13.On = false;
            ledBulb14.On = false;
            ledBulb15.On = false;
            ledBulb16.On = false;
            ledBulb17.On = false;
            ledBulb18.On = false;
            ledBulb19.On = false;
            ledBulb20.On = false;
            ledBulb21.On = false;
            ledBulb22.On = false;
            ledBulb23.On = false;
            ledBulb24.On = false;
            //sevenSegment1.ColorLight = Color.DimGray;
            //sevenSegment2.ColorLight = Color.DimGray;
            //sevenSegment3.ColorLight = Color.DimGray;
            //sevenSegment4.ColorLight = Color.DimGray;
            //sevenSegment5.ColorLight = Color.DimGray;
            //sevenSegment6.ColorLight = Color.DimGray;
            //sevenSegment7.ColorLight = Color.DimGray;
            //sevenSegment8.ColorLight = Color.DimGray;
            //sevenSegment9.ColorLight = Color.DimGray;
            //sevenSegment10.ColorLight = Color.DimGray;
            //sevenSegment11.ColorLight = Color.DimGray;
            //sevenSegment12.ColorLight = Color.DimGray;
            //sevenSegment13.ColorLight = Color.DimGray;
            //sevenSegment14.ColorLight = Color.DimGray;
            //sevenSegment15.ColorLight = Color.DimGray;
            //sevenSegment16.ColorLight = Color.DimGray;
            //sevenSegment1.Value = Convert.ToString(8);
            //sevenSegment2.Value = Convert.ToString(8);
            //sevenSegment3.Value = Convert.ToString(8);
            //sevenSegment4.Value = Convert.ToString(8);
            //sevenSegment5.Value = Convert.ToString(8);
            //sevenSegment6.Value = Convert.ToString(8);
            //sevenSegment7.Value = Convert.ToString(8);
            //sevenSegment8.Value = Convert.ToString(8);
            //sevenSegment9.Value = Convert.ToString(8);
            //sevenSegment10.Value = Convert.ToString(8);
            //sevenSegment11.Value = Convert.ToString(8);
            //sevenSegment12.Value = Convert.ToString(8);
            //sevenSegment13.Value = Convert.ToString(8);
            //sevenSegment14.Value = Convert.ToString(8);
            //sevenSegment15.Value = Convert.ToString(8);
            //sevenSegment16.Value = Convert.ToString(8);
        }

        static int TrafficLight_Init(Junction junc1, Junction junc2)
        {
            //// Junction 1 Traffic Light Priority
            //junc1_priority[0] = 2;
            //junc1_priority[1] = 4;
            //junc1_priority[2] = 3;
            //junc1_priority[3] = 1;

            // Junction 1 Traffic Light Priority
            junc1_priority[0] = Convert.ToByte(junc1.north_priority);
            junc1_priority[1] = Convert.ToByte(junc1.east_priority);
            junc1_priority[2] = Convert.ToByte(junc1.south_priority);
            junc1_priority[3] = Convert.ToByte(junc1.west_priority);

            //// Junction 1 Traffic Light Duration
            //junc1_green_time[0] = 5;           // 5 seconds
            //junc1_green_time[1] = 5;           // 5 seconds
            //junc1_green_time[2] = 5;           // 5 seconds
            //junc1_green_time[3] = 5;           // 5 seconds

            // Junction 1 Traffic Light Duration
            junc1_green_time[0] = junc1.north_green_time;
            junc1_green_time[1] = junc1.east_green_time;
            junc1_green_time[2] = junc1.south_green_time;
            junc1_green_time[3] = junc1.west_green_time;

            //// Junction 2 Traffic Light Priority
            //junc2_priority[0] = 2;
            //junc2_priority[1] = 4;
            //junc2_priority[2] = 3;
            //junc2_priority[3] = 1;

            // Junction 2 Traffic Light Priority
            junc2_priority[0] = Convert.ToByte(junc2.north_priority);
            junc2_priority[1] = Convert.ToByte(junc2.east_priority);
            junc2_priority[2] = Convert.ToByte(junc2.south_priority);
            junc2_priority[3] = Convert.ToByte(junc2.west_priority);

            //// Junction 2 Traffic Light Duration
            //junc2_green_time[0] = 5;           // 5 seconds
            //junc2_green_time[1] = 5;           // 5 seconds
            //junc2_green_time[2] = 5;           // 5 seconds
            //junc2_green_time[3] = 5;           // 5 seconds

            // Junction 2 Traffic Light Duration
            junc2_green_time[0] = junc2.north_green_time;
            junc2_green_time[1] = junc2.east_green_time;
            junc2_green_time[2] = junc2.south_green_time;
            junc2_green_time[3] = junc2.west_green_time;

            // Junction 1 Traffic Lights Initial State
            ledBulb1.On = true;
            ledBulb2.On = false;
            ledBulb3.On = false;
            ledBulb4.On = true;
            ledBulb5.On = false;
            ledBulb6.On = false;
            ledBulb7.On = true;
            ledBulb8.On = false;
            ledBulb9.On = false;
            ledBulb10.On = true;
            ledBulb11.On = false;
            ledBulb12.On = false;

            // Junction 2 Traffic Lights Initial State
            ledBulb13.On = true;
            ledBulb14.On = false;
            ledBulb15.On = false;
            ledBulb16.On = true;
            ledBulb17.On = false;
            ledBulb18.On = false;
            ledBulb19.On = true;
            ledBulb20.On = false;
            ledBulb21.On = false;
            ledBulb22.On = true;
            ledBulb23.On = false;
            ledBulb24.On = false;

            return 1;
        }

        void calc_total_time()
        {
            total_time = junc1_green_time[0] + junc1_green_time[1] + junc1_green_time[2] + junc1_green_time[3];
            total_time += 10;            // 10 seconds tolerance
        }

        void calc_remaining_time()
        {
            int[] junc1_actual_direction = new int[4];
            int[] junc2_actual_direction = new int[4];

            junc1_actual_direction[0] = junc1_direction[0] - 1;
            junc1_actual_direction[1] = junc1_direction[1] - 1;
            junc1_actual_direction[2] = junc1_direction[2] - 1;
            junc1_actual_direction[3] = junc1_direction[3] - 1;

            junc1_remaining_green_time[junc1_actual_direction[0]] = 0;
            junc1_remaining_green_time[junc1_actual_direction[1]] = (junc1_green_time[junc1_actual_direction[0]] + 2);
            junc1_remaining_green_time[junc1_actual_direction[2]] = (junc1_green_time[junc1_actual_direction[0]] + junc1_green_time[junc1_actual_direction[1]] + 3);
            junc1_remaining_green_time[junc1_actual_direction[3]] = (junc1_green_time[junc1_actual_direction[0]] + junc1_green_time[junc1_actual_direction[1]] + junc1_green_time[junc1_actual_direction[2]] + 4);

            junc2_actual_direction[0] = junc2_direction[0] - 1;
            junc2_actual_direction[1] = junc2_direction[1] - 1;
            junc2_actual_direction[2] = junc2_direction[2] - 1;
            junc2_actual_direction[3] = junc2_direction[3] - 1;

            junc2_remaining_green_time[junc2_actual_direction[0]] = 0;
            junc2_remaining_green_time[junc2_actual_direction[1]] = (junc2_green_time[junc2_actual_direction[0]] + 2);
            junc2_remaining_green_time[junc2_actual_direction[2]] = (junc2_green_time[junc2_actual_direction[0]] + junc2_green_time[junc2_actual_direction[1]] + 3);
            junc2_remaining_green_time[junc2_actual_direction[3]] = (junc2_green_time[junc2_actual_direction[0]] + junc2_green_time[junc2_actual_direction[1]] + junc2_green_time[junc2_actual_direction[2]] + 4);
        }

        void show_remaining_time()
        {
            sevensegment_display();

            //if (junc1_remaining_green_time[0] == 0)
            //{
            //    label1.Text = Convert.ToString(0);
            //}

            //else if (junc1_remaining_green_time[0] > 0)
            //{
            //    label1.Text = Convert.ToString(junc1_remaining_green_time[0]--);
            //}

            //if (junc1_remaining_green_time[1] == 0)
            //{
            //    label2.Text = Convert.ToString(0);
            //}

            //else if (junc1_remaining_green_time[1] > 0)
            //{
            //    label2.Text = Convert.ToString(junc1_remaining_green_time[1]--);
            //}

            //if (junc1_remaining_green_time[2] == 0)
            //{
            //    label3.Text = Convert.ToString(0);
            //}

            //else if (junc1_remaining_green_time[2] > 0)
            //{
            //    label3.Text = Convert.ToString(junc1_remaining_green_time[2]--);
            //}

            //if (junc1_remaining_green_time[3] == 0)
            //{
            //    label4.Text = Convert.ToString(0);
            //}

            //else if (junc1_remaining_green_time[3] > 0)
            //{
            //    label4.Text = Convert.ToString(junc1_remaining_green_time[3]--);
            //}

            //if (junc2_remaining_green_time[0] == 0)
            //{
            //    label5.Text = Convert.ToString(0);
            //}

            //else if (junc2_remaining_green_time[0] > 0)
            //{
            //    label5.Text = Convert.ToString(junc2_remaining_green_time[0]--);
            //}

            //if (junc2_remaining_green_time[1] == 0)
            //{
            //    label6.Text = Convert.ToString(0);
            //}

            //else if (junc2_remaining_green_time[1] > 0)
            //{
            //    label6.Text = Convert.ToString(junc2_remaining_green_time[1]--);
            //}

            //if (junc2_remaining_green_time[2] == 0)
            //{
            //    label7.Text = Convert.ToString(0);
            //}

            //else if (junc2_remaining_green_time[2] > 0)
            //{
            //    label7.Text = Convert.ToString(junc2_remaining_green_time[2]--);
            //}

            //if (junc2_remaining_green_time[3] == 0)
            //{
            //    label8.Text = Convert.ToString(0);
            //}

            //else if (junc2_remaining_green_time[3] > 0)
            //{
            //    label8.Text = Convert.ToString(junc2_remaining_green_time[3]--);
            //}

            // Improved Code Due To Show Green Time
            if (junc1_remaining_green_time[0] == 0)
            {
                
            }

            else if (junc1_remaining_green_time[0] > 0)
            {
                junc1_remaining_green_time[0]--;
            }

            if (junc1_remaining_green_time[1] == 0)
            {
                
            }

            else if (junc1_remaining_green_time[1] > 0)
            {
                junc1_remaining_green_time[1]--;
            }

            if (junc1_remaining_green_time[2] == 0)
            {
                
            }

            else if (junc1_remaining_green_time[2] > 0)
            {
                junc1_remaining_green_time[2]--;
            }

            if (junc1_remaining_green_time[3] == 0)
            {
                
            }

            else if (junc1_remaining_green_time[3] > 0)
            {
                junc1_remaining_green_time[3]--;
            }

            if (junc2_remaining_green_time[0] == 0)
            {
                
            }

            else if (junc2_remaining_green_time[0] > 0)
            {
                junc2_remaining_green_time[0]--;
            }

            if (junc2_remaining_green_time[1] == 0)
            {
                
            }

            else if (junc2_remaining_green_time[1] > 0)
            {
                junc2_remaining_green_time[1]--;
            }

            if (junc2_remaining_green_time[2] == 0)
            {
                
            }

            else if (junc2_remaining_green_time[2] > 0)
            {
                junc2_remaining_green_time[2]--;
            }

            if (junc2_remaining_green_time[3] == 0)
            {
                
            }

            else if (junc2_remaining_green_time[3] > 0)
            {
                junc2_remaining_green_time[3]--;
            }

        }

        void show_green_time()
        {
            label1.Text = Convert.ToString(junc1_green_time[0]);
            label2.Text = Convert.ToString(junc1_green_time[1]);
            label3.Text = Convert.ToString(junc1_green_time[2]);
            label4.Text = Convert.ToString(junc1_green_time[3]);
            label5.Text = Convert.ToString(junc2_green_time[0]);
            label6.Text = Convert.ToString(junc2_green_time[1]);
            label7.Text = Convert.ToString(junc2_green_time[2]);
            label8.Text = Convert.ToString(junc2_green_time[3]);

        }

            void junc1_traffic_light(int a, int b, int c, int d)
        {
            ledBulb1.On = false;
            ledBulb2.On = false;
            ledBulb3.On = false;

            if (a == 1)
            {
                ledBulb1.On = true;
            }

            else if (a == 2)
            {
                ledBulb2.On = true;
            }

            else if (a == 3)
            {
                ledBulb3.On = true;
            }

            ledBulb4.On = false;
            ledBulb5.On = false;
            ledBulb6.On = false;

            if (b == 1)
            {
                ledBulb4.On = true;
            }

            else if (b == 2)
            {
                ledBulb5.On = true;
            }

            else if (b == 3)
            {
                ledBulb6.On = true;
            }

            ledBulb7.On = false;
            ledBulb8.On = false;
            ledBulb9.On = false;

            if (c == 1)
            {
                ledBulb7.On = true;
            }

            else if (c == 2)
            {
                ledBulb8.On = true;
            }

            else if (c == 3)
            {
                ledBulb9.On = true;
            }

            ledBulb10.On = false;
            ledBulb11.On = false;
            ledBulb12.On = false;

            if (d == 1)
            {
                ledBulb10.On = true;
            }

            else if (d == 2)
            {
                ledBulb11.On = true;
            }

            else if (d == 3)
            {
                ledBulb12.On = true;
            }
        }

        void junc2_traffic_light(int a, int b, int c, int d)
        {
            ledBulb13.On = false;
            ledBulb14.On = false;
            ledBulb15.On = false;

            if (a == 1)
            {
                ledBulb13.On = true;
            }

            else if (a == 2)
            {
                ledBulb14.On = true;
            }

            else if (a == 3)
            {
                ledBulb15.On = true;
            }

            ledBulb16.On = false;
            ledBulb17.On = false;
            ledBulb18.On = false;

            if (b == 1)
            {
                ledBulb16.On = true;
            }

            else if (b == 2)
            {
                ledBulb17.On = true;
            }

            else if (b == 3)
            {
                ledBulb18.On = true;
            }

            ledBulb19.On = false;
            ledBulb20.On = false;
            ledBulb21.On = false;

            if (c == 1)
            {
                ledBulb19.On = true;
            }

            else if (c == 2)
            {
                ledBulb20.On = true;
            }

            else if (c == 3)
            {
                ledBulb21.On = true;
            }

            ledBulb22.On = false;
            ledBulb23.On = false;
            ledBulb24.On = false;

            if (d == 1)
            {
                ledBulb22.On = true;
            }

            else if (d == 2)
            {
                ledBulb23.On = true;
            }

            else if (d == 3)
            {
                ledBulb24.On = true;
            }
        }

        void sevensegment_display ()
        {
            int temp_sevensegment;

            if (junc1_remaining_green_time[0] < 10)
            {
                sevenSegment1.Value = Convert.ToString(0);
                sevenSegment2.Value = Convert.ToString(junc1_remaining_green_time[0]);
            }

            else if (junc1_remaining_green_time[0] >= 10)
            {
                temp_sevensegment = junc1_remaining_green_time[0] / 10;
                sevenSegment1.Value = Convert.ToString(junc1_remaining_green_time[0] / 10);
                sevenSegment2.Value = Convert.ToString(junc1_remaining_green_time[0] - (temp_sevensegment*10));
            }
                    
            if (junc1_remaining_green_time[1] < 10)
            {
                sevenSegment3.Value = Convert.ToString(0);
                sevenSegment4.Value = Convert.ToString(junc1_remaining_green_time[1]);
            }

            else if (junc1_remaining_green_time[1] >= 10)
            {
                temp_sevensegment = junc1_remaining_green_time[1] / 10;
                sevenSegment3.Value = Convert.ToString(junc1_remaining_green_time[1] / 10);
                sevenSegment4.Value = Convert.ToString(junc1_remaining_green_time[1] - (temp_sevensegment * 10));
            }
                    
            if (junc1_remaining_green_time[2] < 10)
            {
                sevenSegment5.Value = Convert.ToString(0);
                sevenSegment6.Value = Convert.ToString(junc1_remaining_green_time[2]);
            }

            else if (junc1_remaining_green_time[2] >= 10)
            {
                temp_sevensegment = junc1_remaining_green_time[2] / 10;
                sevenSegment5.Value = Convert.ToString(junc1_remaining_green_time[2] / 10);
                sevenSegment6.Value = Convert.ToString(junc1_remaining_green_time[2] - (temp_sevensegment * 10));
            }
                    
            if (junc1_remaining_green_time[3] < 10)
            {
                sevenSegment7.Value = Convert.ToString(0);
                sevenSegment8.Value = Convert.ToString(junc1_remaining_green_time[3]);
            }

            else if (junc1_remaining_green_time[3] >= 10)
            {
                temp_sevensegment = junc1_remaining_green_time[3] / 10;
                sevenSegment7.Value = Convert.ToString(junc1_remaining_green_time[3] / 10);
                sevenSegment8.Value = Convert.ToString(junc1_remaining_green_time[3] - (temp_sevensegment * 10));
            }

            if (junc2_remaining_green_time[0] < 10)
            {
                sevenSegment9.Value = Convert.ToString(0);
                sevenSegment10.Value = Convert.ToString(junc2_remaining_green_time[0]);
            }

            else if (junc2_remaining_green_time[0] >= 10)
            {
                temp_sevensegment = junc2_remaining_green_time[0] / 10;
                sevenSegment9.Value = Convert.ToString(junc2_remaining_green_time[0] / 10);
                sevenSegment10.Value = Convert.ToString(junc2_remaining_green_time[0] - (temp_sevensegment * 10));
            }

            if (junc2_remaining_green_time[1] < 10)
            {
                sevenSegment11.Value = Convert.ToString(0);
                sevenSegment12.Value = Convert.ToString(junc2_remaining_green_time[1]);
            }

            else if (junc2_remaining_green_time[1] >= 10)
            {
                temp_sevensegment = junc2_remaining_green_time[1] / 10;
                sevenSegment11.Value = Convert.ToString(junc2_remaining_green_time[1] / 10);
                sevenSegment12.Value = Convert.ToString(junc2_remaining_green_time[1] - (temp_sevensegment * 10));
            }

            if (junc2_remaining_green_time[2] < 10)
            {
                sevenSegment13.Value = Convert.ToString(0);
                sevenSegment14.Value = Convert.ToString(junc2_remaining_green_time[2]);
            }

            else if (junc2_remaining_green_time[2] >= 10)
            {
                temp_sevensegment = junc2_remaining_green_time[2] / 10;
                sevenSegment13.Value = Convert.ToString(junc2_remaining_green_time[2] / 10);
                sevenSegment14.Value = Convert.ToString(junc2_remaining_green_time[2] - (temp_sevensegment * 10));
            }

            if (junc2_remaining_green_time[3] < 10)
            {
                sevenSegment15.Value = Convert.ToString(0);
                sevenSegment16.Value = Convert.ToString(junc2_remaining_green_time[3]);
            }

            else if (junc2_remaining_green_time[3] >= 10)
            {
                temp_sevensegment = junc2_remaining_green_time[3] / 10;
                sevenSegment15.Value = Convert.ToString(junc2_remaining_green_time[3] / 10);
                sevenSegment16.Value = Convert.ToString(junc2_remaining_green_time[3] - (temp_sevensegment * 10));
            }
        }

        void Priority_Sort()
        {
            byte i, i2, temp_priority, temp_direction;
            int temp_time_buffer;
            byte[] priority_array = new byte[4];
            byte[] direction_array = new byte[4];
            int[] time_array = new int[4];
            int[] remaining_time_array = new int[4];

            // Junction 1 Priority Sort

            priority_array[0] = junc1_priority[0];
            priority_array[1] = junc1_priority[1];
            priority_array[2] = junc1_priority[2];
            priority_array[3] = junc1_priority[3];

            time_array[0] = junc1_green_time[0];
            time_array[1] = junc1_green_time[1];
            time_array[2] = junc1_green_time[2];
            time_array[3] = junc1_green_time[3];

            direction_array[0] = junc1_direction[0];
            direction_array[1] = junc1_direction[1];
            direction_array[2] = junc1_direction[2];
            direction_array[3] = junc1_direction[3];

            for (i2 = 0; i2 < 3; i2++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (i != 3)
                    {
                        if (priority_array[i] < priority_array[i + 1])
                        {
                            temp_priority = priority_array[i];
                            priority_array[i] = priority_array[i + 1];
                            priority_array[i + 1] = temp_priority;

                            temp_time_buffer = time_array[i];
                            time_array[i] = time_array[i + 1];
                            time_array[i + 1] = temp_time_buffer;

                            temp_direction = direction_array[i];
                            direction_array[i] = direction_array[i + 1];
                            direction_array[i + 1] = temp_direction;
                        }
                    }

                }
            }

            junc1_priority[0] = priority_array[0];
            junc1_priority[1] = priority_array[1];
            junc1_priority[2] = priority_array[2];
            junc1_priority[3] = priority_array[3];

            junc1_green_time[0] = time_array[0];
            junc1_green_time[1] = time_array[1];
            junc1_green_time[2] = time_array[2];
            junc1_green_time[3] = time_array[3];

            junc1_direction[0] = direction_array[0];
            junc1_direction[1] = direction_array[1];
            junc1_direction[2] = direction_array[2];
            junc1_direction[3] = direction_array[3];

            // Junction 2 Priority Sort

            priority_array[0] = junc2_priority[0];
            priority_array[1] = junc2_priority[1];
            priority_array[2] = junc2_priority[2];
            priority_array[3] = junc2_priority[3];

            time_array[0] = junc2_green_time[0];
            time_array[1] = junc2_green_time[1];
            time_array[2] = junc2_green_time[2];
            time_array[3] = junc2_green_time[3];

            direction_array[0] = junc2_direction[0];
            direction_array[1] = junc2_direction[1];
            direction_array[2] = junc2_direction[2];
            direction_array[3] = junc2_direction[3];

            for (i2 = 0; i2 < 3; i2++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (i != 3)
                    {
                        if (priority_array[i] < priority_array[i + 1])
                        {
                            temp_priority = priority_array[i];
                            priority_array[i] = priority_array[i + 1];
                            priority_array[i + 1] = temp_priority;

                            temp_time_buffer = time_array[i];
                            time_array[i] = time_array[i + 1];
                            time_array[i + 1] = temp_time_buffer;

                            temp_direction = direction_array[i];
                            direction_array[i] = direction_array[i + 1];
                            direction_array[i + 1] = temp_direction;
                        }
                    }

                }
            }

            junc2_priority[0] = priority_array[0];
            junc2_priority[1] = priority_array[1];
            junc2_priority[2] = priority_array[2];
            junc2_priority[3] = priority_array[3];

            junc2_green_time[0] = time_array[0];
            junc2_green_time[1] = time_array[1];
            junc2_green_time[2] = time_array[2];
            junc2_green_time[3] = time_array[3];

            junc2_direction[0] = direction_array[0];
            junc2_direction[1] = direction_array[1];
            junc2_direction[2] = direction_array[2];
            junc2_direction[3] = direction_array[3];
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
            int cycle = 20;                        // One Complete Cycle = 20 Seconds
            int max_flow_rate = 700;
            int i = 0;
            int x1, x2, y1, y2, temp1, temp2, temp3, temp4, temp5, temp6;

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

            ratio = compare(ratio);     // Find Max Ratio

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

            else
            {
                VE_res = Convert.ToInt32(Math.Round(Vres * (flow_ratio_i / (flow_ratio_k - flow_ratio_j))));
            }

            return VE_res;
        }

        static Ratio compare(Ratio ratio)
        {
            int i, n;
            float temp_value;
            int temp_priority;
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

        private async static void SendCloudToDeviceMessageAsync(Junction junction)
        {
            var north_green_time = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.north_green_time)));
            north_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", north_green_time);

            var north_priority = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.north_priority)));
            north_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", north_priority);

            var east_green_time = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.east_green_time)));
            east_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", east_green_time);

            var east_priority = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.east_priority)));
            east_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", east_priority);

            var south_green_time = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.south_green_time)));
            south_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", south_green_time);

            var south_priority = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.south_priority)));
            south_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", south_priority);

            var west_green_time = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.west_green_time)));
            west_green_time.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", west_green_time);

            var west_priority = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(Convert.ToString(junction.west_priority)));
            west_priority.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Lenovo01", west_priority);
        }
    }
}
