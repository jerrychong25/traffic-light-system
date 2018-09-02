#include <stdio.h>
#include <stdlib.h>
#include "mraa.h"
#include "Application.h"
#include "i2c.h"
#include "Serial.h"
//#include "MCP23017.h"
#include "MAX6958.h"
#include "Utility.h"

//int main(int argc, char** argv)
//{
//    while(1)
//    {
//    	System_Run();
//    }
//}

//int main(int argc, char** argv)
//{
//    mraa_init();
//    mraa_i2c_context i2c;

//    i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
//    I2C_Frequency(i2c, 1);

//    IO_Expander_Init(i2c);

//    while(1)
//    {
//    	IO_Expander_Write(i2c, MCP23017_GPIOA_ADDR, 0x3F);		// Write to GPIOA register
//    	IO_Expander_Write(i2c, MCP23017_GPIOB_ADDR, 0x3F);		// Write to GPIOB register
//    	delay_seconds(1);
//    	IO_Expander_Write(i2c, MCP23017_GPIOA_ADDR, 0x00);		// Write to GPIOA register
//    	IO_Expander_Write(i2c, MCP23017_GPIOB_ADDR, 0x00);		// Write to GPIOB register
//    	delay_seconds(1);
//    }
//}

int main(int argc, char** argv)
{
    mraa_init();
    mraa_i2c_context i2c;

    i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
    I2C_Frequency(i2c, 1);

    Seven_Segment_Driver_Init(i2c);

    while(1)
    {
    	Seven_Segment_Driver_Test(i2c, 0x00);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x01);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x02);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x03);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x04);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x05);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x06);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x07);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x08);
    	delay_seconds(1);
    	Seven_Segment_Driver_Test(i2c, 0x09);
    	delay_seconds(1);
    }
}
