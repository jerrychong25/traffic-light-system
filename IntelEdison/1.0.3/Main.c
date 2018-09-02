#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "Application.h"
//#include "Serial.h"
//#include "MCP23017.h"
//////#include "MAX6958.h"
#include "Utility.h"

int main(int argc, char** argv)
{
//    while(1)
//    {
//    	System_Run();
//    }
//	while(1)
//	{
		SendMessage1();
//		delay_seconds(25);
//	}
}

//int main(int argc, char** argv)
//{
//	// Global Variable
//	uint8_t temp_buffer = 0;
//	uint8_t north_input, east_input, south_input, west_input;
//
////	// Define GPIO varialbes
////	static int gpio4, gpio5, gpio6, gpio7, gpio12;
////	gpio4 = 4;
////	gpio5 = 5;
////	gpio6 = 6;
////	gpio7 = 7;
////	gpio12 = 12;
//
////	static int gpio12;
////	gpio12 = 12;
//
//	mraa_init();
//
////	mraa_gpio_context emergency;
////	emergency = mraa_gpio_init(gpio12);
////	mraa_gpio_dir(emergency, MRAA_GPIO_IN);
//
//    mraa_i2c_context i2c;
//
////	// Configure Sensor 1 on Pin 4
////	mraa_gpio_context sensor1;
////	sensor1 = mraa_gpio_init(gpio4);
////	mraa_gpio_dir(sensor1, MRAA_GPIO_IN);
////
////	// Configure Sensor 2 on Pin 5
////	mraa_gpio_context sensor2;
////	sensor2 = mraa_gpio_init(gpio5);
////	mraa_gpio_dir(sensor2, MRAA_GPIO_IN);
////
////	// Configure Sensor 3 on Pin 6
////	mraa_gpio_context sensor3;
////	sensor3 = mraa_gpio_init(gpio6);
////	mraa_gpio_dir(sensor3, MRAA_GPIO_IN);
////
////	// Configure Sensor 4 on Pin 7
////	mraa_gpio_context sensor4;
////	sensor4 = mraa_gpio_init(gpio7);
////	mraa_gpio_dir(sensor4, MRAA_GPIO_IN);
////
////	// Configure Emergency Pin on Pin 12
////	mraa_gpio_context emergency;
////	emergency = mraa_gpio_init(gpio12);
////	mraa_gpio_dir(emergency, MRAA_GPIO_IN);
//
//    i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
//    I2C_Frequency(i2c, 1);
//
////    IO_Expander1_Init(i2c);
//    IO_Expander2_Init(i2c);
//
//    while(1)
//    {
////    	IO_Expander1_Write(i2c, MCP23017_GPIOA_ADDR, 0x3F);		// Write to GPIOA register
////    	IO_Expander1_Write(i2c, MCP23017_GPIOB_ADDR, 0x3F);		// Write to GPIOB register
////    	delay_seconds(1);
//
//    	// Read Data
//    	temp_buffer = IO_Expander2_Read(i2c, MCP23017_GPIOA_ADDR);
//
//    	if((temp_buffer & 0x01) == 0x01)
//    	{
//    		north_input += 1;
//    	}
//
//    	if((temp_buffer & 0x02) == 0x02)
//    	{
//    		east_input += 1;
//    	}
//
//    	if((temp_buffer & 0x04) == 0x04)
//    	{
//    		south_input += 1;
//    	}
//
//    	if((temp_buffer & 0x08) == 0x08)
//    	{
//    		west_input += 1;
//    	}
//
////    	north_input = 0;
////    	east_input = 0;
////    	south_input = 0;
////    	west_input = 0;
//    	temp_buffer = 0;
//
//    	delay_miliseconds(100);
//
//
////    	if(mraa_gpio_read(emergency))
////    	{
////    		IO_Expander_Write(i2c, MCP23017_GPIOA_ADDR, 0x00);		// Write to GPIOA register
////    		IO_Expander_Write(i2c, MCP23017_GPIOB_ADDR, 0x00);		// Write to GPIOB register
////    		delay_seconds(1);
////    	}
//
//    }
//}

//int main(int argc, char** argv)
//{
//	static int gpio7, gpio8;
//
//    mraa_init();
//
//    mraa_gpio_context seven_segment_driver1, seven_segment_driver2;
//    mraa_i2c_context i2c;
//
//    gpio7 = 7;
//    gpio8 = 8;
//
//    seven_segment_driver1 = mraa_gpio_init(gpio7);
//    seven_segment_driver2 = mraa_gpio_init(gpio8);
//
//    mraa_gpio_dir(seven_segment_driver1, MRAA_GPIO_OUT);
//    mraa_gpio_dir(seven_segment_driver2, MRAA_GPIO_OUT);
//
//    i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
//    I2C_Frequency(i2c, 1);
//
////    mraa_gpio_write(seven_segment_driver1, 1);
////    Seven_Segment_Driver_Init(i2c);
//
//    while(1)
//    {
////    	unsigned char i;
//////    	unsigned int a;
////
////    	for(i=0; i<9; i++)
////    	{
//////    		for(a=0; a<1; a++)
//////    		{
////    			mraa_gpio_write(seven_segment_driver1, 1);
////    			mraa_gpio_write(seven_segment_driver2, 0);
////    			Seven_Segment_Driver_Init(i2c);
////    			delay_miliseconds(10);
////    			Seven_Segment_Driver_Test(i2c, i);
////    			delay_seconds(1);
////
////    			mraa_gpio_write(seven_segment_driver1, 0);
////    			mraa_gpio_write(seven_segment_driver2, 1);
////    			Seven_Segment_Driver_Init(i2c);
////    			delay_miliseconds(10);
////    			Seven_Segment_Driver_Test(i2c, i);
////    			delay_seconds(1);
////
//////    			mraa_gpio_write(seven_segment_driver1, 1);
//////    			delay_seconds(1);
//////    		}
////    	}
//
////    	mraa_gpio_write(seven_segment_driver1, 1);
////    	mraa_gpio_write(seven_segment_driver2, 0);
//    	Seven_Segment_Driver_Init(i2c);
//    	Seven_Segment_Driver_Test(i2c, 1);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 2);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 3);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 4);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 5);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 6);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 7);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 8);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 9);
//    	delay_seconds(1);
//    	Seven_Segment_Driver_Test(i2c, 0);
//    	delay_seconds(1);
//    }
//}

