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

namespace InnovateTraffic
{
    public partial class Form1 : Form
    {
        private int _blink = 0;
        byte[] priority = new byte[4];
        byte temp_priority;
        int[] temp_time = new int[4];
        int[] remaining_green_time = new int[4];

        public Form1()
        {
            priority[0] = 1;
            priority[1] = 2;
            priority[2] = 3;
            priority[3] = 4;

            InitializeComponent();
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
            int temp_delay;

            temp_time[0] = 15;           // 1 second
            temp_time[1] = 15;           // 2 seconds
            temp_time[2] = 15;           // 3 seconds
            temp_time[3] = 15;           // 4 seconds

            calc_remaining_time();
            show_remaining_time();

            temp_priority = priority[0];
            redgreen_assign(temp_priority);
            for(temp_delay = 0; temp_delay < temp_time[0]; temp_delay++)
            {
                await Task.Delay(1000);
                show_remaining_time();
            }
            greenyellow_assign(temp_priority);
            await Task.Delay(1000);
            show_remaining_time();
            yellowred_assign(temp_priority);
            
            temp_priority = priority[1];
            redgreen_assign(temp_priority);
            for (temp_delay = 0; temp_delay < temp_time[1]; temp_delay++)
            {
                await Task.Delay(1000);
                show_remaining_time();
            }
            greenyellow_assign(temp_priority);
            await Task.Delay(1000);
            show_remaining_time();
            yellowred_assign(temp_priority);

            temp_priority = priority[2];
            redgreen_assign(temp_priority);
            for (temp_delay = 0; temp_delay < temp_time[2]; temp_delay++)
            {
                await Task.Delay(1000);
                show_remaining_time();
            }
            greenyellow_assign(temp_priority);
            await Task.Delay(1000);
            show_remaining_time();
            yellowred_assign(temp_priority);
            
            temp_priority = priority[3];
            redgreen_assign(temp_priority);
            for (temp_delay = 0; temp_delay < temp_time[3]; temp_delay++)
            {
                await Task.Delay(1000);
                show_remaining_time();
            }
            greenyellow_assign(temp_priority);
            await Task.Delay(1000);
            show_remaining_time();
            yellowred_assign(temp_priority);

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
        }

        async void Delay_seconds(int seconds)
        {
            seconds = seconds*1000;
            await Task.Delay(seconds);
        }

        void calc_remaining_time()
        {
            remaining_green_time[0] = 0;
            remaining_green_time[1] = (temp_time[0] + 1);
            remaining_green_time[2] = (temp_time[0] + temp_time[1] + 2);
            remaining_green_time[3] = (temp_time[0] + temp_time[1] + temp_time[2] + 3);
        }

        void show_remaining_time()
        {
            sevensegment_display();

            if (remaining_green_time[0] == 0 && remaining_green_time[1] == 0 && remaining_green_time[2] == 0 && remaining_green_time[3] == 0)
            {
                label1.Text = Convert.ToString(0);
                label2.Text = Convert.ToString(0);
                label3.Text = Convert.ToString(0);
                label4.Text = Convert.ToString(0);
            }

            else if (remaining_green_time[0] == 0 && remaining_green_time[1] == 0 && remaining_green_time[2] == 0)
            {
                label1.Text = Convert.ToString(0);
                label2.Text = Convert.ToString(0);
                label3.Text = Convert.ToString(0);
                label4.Text = Convert.ToString(remaining_green_time[3]--);
            }

            else if (remaining_green_time[0] == 0 && remaining_green_time[1] == 0)
            {
                label1.Text = Convert.ToString(0);
                label2.Text = Convert.ToString(0);
                label3.Text = Convert.ToString(remaining_green_time[2]--);
                label4.Text = Convert.ToString(remaining_green_time[3]--);
            }

            else if (remaining_green_time[0] == 0)
            {
                label1.Text = Convert.ToString(0);
                label2.Text = Convert.ToString(remaining_green_time[1]--);
                label3.Text = Convert.ToString(remaining_green_time[2]--);
                label4.Text = Convert.ToString(remaining_green_time[3]--);
            }

            //else
            //{
            //    label1.Text = Convert.ToString(remaining_green_time[0]--);
            //    label2.Text = Convert.ToString(remaining_green_time[1]--);
            //    label3.Text = Convert.ToString(remaining_green_time[2]--);
            //    label4.Text = Convert.ToString(remaining_green_time[3]--);
            //}

        }

        void redgreen_assign(byte sequence)
        {
            if (sequence == 1)
            {
                ledBulb1.On = false;
                ledBulb3.On = true;
            }

            else if (sequence == 2)
            {
                ledBulb4.On = false;
                ledBulb6.On = true;
            }

            else if (sequence == 3)
            {
                ledBulb7.On = false;
                ledBulb9.On = true;
            }

            else if (sequence == 4)
            {
                ledBulb10.On = false;
                ledBulb12.On = true;
            }
        }

        void greenyellow_assign(byte sequence)
        {
            if (sequence == 1)
            {
                ledBulb3.On = false;
                ledBulb2.On = true;
            }

            else if (sequence == 2)
            {
                ledBulb6.On = false;
                ledBulb5.On = true;
            }

            else if (sequence == 3)
            {
                ledBulb9.On = false;
                ledBulb8.On = true;
            }

            else if (sequence == 4)
            {
                ledBulb12.On = false;
                ledBulb11.On = true;
            }
        }

        void yellowred_assign (byte sequence)
        {
            if(sequence == 1)
            {
                ledBulb2.On = false;
                ledBulb1.On = true;
            }

            else if (sequence == 2)
            {
                ledBulb5.On = false;
                ledBulb4.On = true;
            }

            else if (sequence == 3)
            {
                ledBulb8.On = false;
                ledBulb7.On = true;
            }

            else if (sequence == 4)
            {
                ledBulb11.On = false;
                ledBulb10.On = true;
            }
        }

        void sevensegment_display ()
        {
            int temp_sevensegment;
            if (remaining_green_time[0] < 10)
            {
                sevenSegment1.Value = Convert.ToString(0);
                sevenSegment2.Value = Convert.ToString(remaining_green_time[0]);
            }

            else if (remaining_green_time[0] >= 10)
            {
                temp_sevensegment = remaining_green_time[0] / 10;
                sevenSegment1.Value = Convert.ToString(remaining_green_time[0] / 10);
                sevenSegment2.Value = Convert.ToString(remaining_green_time[0] - (temp_sevensegment*10));
            }
                    
            if (remaining_green_time[1] < 10)
            {
                sevenSegment3.Value = Convert.ToString(0);
                sevenSegment4.Value = Convert.ToString(remaining_green_time[1]);
            }

            else if (remaining_green_time[1] >= 10)
            {
                temp_sevensegment = remaining_green_time[1] / 10;
                sevenSegment3.Value = Convert.ToString(remaining_green_time[1] / 10);
                sevenSegment4.Value = Convert.ToString(remaining_green_time[1] - (temp_sevensegment * 10));
            }
                    
            if (remaining_green_time[2] < 10)
            {
                sevenSegment5.Value = Convert.ToString(0);
                sevenSegment6.Value = Convert.ToString(remaining_green_time[2]);
            }

            else if (remaining_green_time[2] >= 10)
            {
                temp_sevensegment = remaining_green_time[2] / 10;
                sevenSegment5.Value = Convert.ToString(remaining_green_time[2] / 10);
                sevenSegment6.Value = Convert.ToString(remaining_green_time[2] - (temp_sevensegment * 10));
            }
                    
            if (remaining_green_time[3] < 10)
            {
                sevenSegment7.Value = Convert.ToString(0);
                sevenSegment8.Value = Convert.ToString(remaining_green_time[3]);
            }

            else if (remaining_green_time[3] >= 10)
            {
                temp_sevensegment = remaining_green_time[3] / 10;
                sevenSegment7.Value = Convert.ToString(remaining_green_time[3] / 10);
                sevenSegment8.Value = Convert.ToString(remaining_green_time[3] - (temp_sevensegment * 10));
            }
        }
    }
}
