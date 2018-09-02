/*
 * Application.c
 *
 *  Created on: Jan 24, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include "mraa.h"
#include "Application.h"
#include "Serial.h"
#include "Utility.h"
#include "MCP23017.h"
#include "MAX6958.h"

void Variable_Assign(mraa_i2c_context structure, uint16_t input)
{
	junction temp_cross;

	temp_cross.northeast = 0;
	temp_cross.southwest = 0;

	temp_cross.northeast |= ((input & 0x0FC0) >> 6);
	temp_cross.southwest |= (input & 0x003F);

	IO_Expander_Write(structure, MCP23017_GPIOA_ADDR, temp_cross.northeast);		// Write cross.northeast to GPIOA register
	delay_microseconds(10);
	IO_Expander_Write(structure, MCP23017_GPIOB_ADDR, temp_cross.southwest);		// Write cross.southwest to GPIOB register
}

void Light_Change(mraa_i2c_context structure, uint16_t input, uint8_t junc, uint8_t phase)
{
	uint8_t i;
	uint16_t temp, final;

	switch(junc)
	{
		case 1:
			temp = 0x0800;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xF1FF) | temp;		// Case 1 0000XXX000000000 (Bit 9-11)
			break;

		case 2:
			temp = 0x0100;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFE3F) | temp;		// Case 2 0000000XXX000000 (Bit 6-8)
			break;

		case 3:
			temp = 0x0020;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFC7) | temp;		// Case 3 0000000000XXX000 (Bit 3-5)
			break;

		case 4:
			temp = 0x0004;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFF8) | temp;		// Case 4 0000000000000XXX (Bit 0-2)
			break;
	}

	Variable_Assign(structure, final);
}

void Traffic_Initial(mraa_i2c_context structure, uint16_t temp_input)
{
	Light_Change(structure, temp_input, NORTH, RED);
	Light_Change(structure, temp_input, EAST, RED);
	Light_Change(structure, temp_input, SOUTH, RED);
	Light_Change(structure, temp_input, WEST, RED);
}

void Traffic_Run(mraa_i2c_context structure, uint16_t temp_input, uint8_t temp_junc, uint8_t temp_time)
{
	Light_Change(structure, temp_input, temp_junc, GREEN);
	delay_seconds(temp_time);
	Light_Change(structure, temp_input, temp_junc, YELLOW);
	delay_seconds(2);
	Light_Change(structure, temp_input, temp_junc, RED);
	delay_microseconds(100);
}

void Traffic_Mode(mraa_i2c_context structure, uint16_t input, uint8_t mode, direction time)
{
	switch(mode)
	{
		case NORTH:
			Traffic_Run(structure, input, NORTH, time.north);
			break;
		case EAST:
			Traffic_Run(structure, input, EAST, time.east);
			break;
		case SOUTH:
			Traffic_Run(structure, input, SOUTH, time.south);
			break;
		case WEST:
			Traffic_Run(structure, input, WEST, time.west);
			break;
	}
}

uint8_t Priority_Sort(direction priority)
{
	uint8_t i, temp_priority, temp_direction;
	uint8_t priority_array[4];
	uint8_t direction_array[4];

	priority_array[0] = priority.north;
	priority_array[1] = priority.east;
	priority_array[2] = priority.south;
	priority_array[3] = priority.west;

	direction_array[0] = NORTH;
	direction_array[1] = EAST;
	direction_array[2] = SOUTH;
	direction_array[3] = WEST;

	for(i = 0; i < 4; i++)
	{
		if (priority_array[i] < priority_array[i+1])
		{
			temp_priority = priority_array[i];
			priority_array[i] = priority_array[i+1];
			priority_array[i+1] = temp_priority;

			temp_direction = direction_array[i];
			direction_array[i] = direction_array[i+1];
			direction_array[i+1] = temp_direction;
		}
	}

	return *direction_array;
}

void System_Run(void)
{
	uint8_t i, mode, final_direction[4];
	static uint8_t first = 1;
	uint16_t initial_state = 0x0249;
	direction time;
	mraa_init();
	mraa_i2c_context i2c;

	if(first == 1)
	{
		mode = 1;
	}

	switch(mode)
	{
		case 1:
			i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
			I2C_Frequency(i2c, 1);

		    IO_Expander_Init(i2c);
//		    Seven_Segment_Driver_Init(i2c);

		    IO_Expander_Write(i2c, MCP23017_GPIOA_ADDR, 0x09);		// Set all to red light
		    IO_Expander_Write(i2c, MCP23017_GPIOB_ADDR, 0x09);		// Set all to red light

		    first = 0;
		    mode = 2;

		case 2:
			time.north = 2;
			time.east = 2;
			time.south = 2;
			time.west = 2;

			///////////////////////// direction time, direction priority

			//	final_direction[] = priority_arrange(priority);
			final_direction[0] = NORTH;
			final_direction[1] = EAST;
			final_direction[2] = SOUTH;
			final_direction[3] = WEST;

			//	Traffic_Initial(structure, initial_state);

			for(i = 0;i < 4; i++)
			{
				Traffic_Mode(i2c, initial_state, final_direction[i], time);
//				delay_microseconds(1);
//				Seven_Segment_Driver_Show(i2c, i, 1);
//				delay_microseconds(1);
//				Seven_Segment_Driver_Show(i2c, i, 2);
//				delay_microseconds(1);
//				Seven_Segment_Driver_Show(i2c, i, 3);
//				delay_microseconds(1);
//				Seven_Segment_Driver_Show(i2c, i, 4);
			}

			//////////////////////////
			break;
	}

}

void Seven_Segment_Driver_Test(mraa_i2c_context structure, uint8_t value)
{
	Seven_Segment_Driver_Show(structure, value, 1);
	Seven_Segment_Driver_Show(structure, value, 2);
	Seven_Segment_Driver_Show(structure, value, 3);
	Seven_Segment_Driver_Show(structure, value, 4);

//	Seven_Segment_Driver_ShowTest(structure, value, value, value, value);
}

void Seven_Segment_Driver_Show(mraa_i2c_context structure, uint8_t value, uint8_t sequence)
{
	if(sequence == 1)
	{
		Seven_Segment_Driver_Write(structure, MAX6958_DIGIT0_ADDR, value);
	}

	else if(sequence == 2)
	{
		Seven_Segment_Driver_Write(structure, MAX6958_DIGIT1_ADDR, value);
	}

	else if(sequence == 3)
	{
		Seven_Segment_Driver_Write(structure, MAX6958_DIGIT2_ADDR, value);
	}

	else if(sequence == 4)
	{
		Seven_Segment_Driver_Write(structure, MAX6958_DIGIT3_ADDR, value);
	}
}

void Seven_Segment_Driver_ShowTest(mraa_i2c_context structure, uint8_t content1, uint8_t content2, uint8_t content3, uint8_t content4)
{
	uint8_t rx_tx_buf[5];
    rx_tx_buf[0] = MAX6958_DIGIT0_ADDR;						// Access to register
	rx_tx_buf[1] = content1;					// Write digit0
	rx_tx_buf[2] = content2;					// Write digit1
	rx_tx_buf[3] = content3;					// Write digit2
	rx_tx_buf[4] = content4;					// Write digit3

	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);
	mraa_i2c_write(structure, rx_tx_buf, 5);
}

void Seven_Segment_Driver_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content)
{
	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);
	MAX6958_Register_Write(structure, register_address, content);
}

void Seven_Segment_Driver_Init(mraa_i2c_context structure)
{
	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);					// Access MAX6958
	MAX6958_Register_Write(structure, MAX6958_DECODE_ADDR, 0x0F);			// Set digit0-3 in decode mode
	MAX6958_Register_Write(structure, MAX6958_INTENSITY_ADDR, 0x04);		// Set intensity 4/64 brightness
	MAX6958_Register_Write(structure, MAX6958_SCANLIMIT_ADDR, 0x03);		// Set scan limit for 4 digits
	MAX6958_Register_Write(structure, MAX6958_CONFIG_ADDR, 0x01);			// Set normal operation
}

void IO_Expander_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content)
{
	MCP23017_Access_Address(structure, MCP23017_I2C_ADDR);
	MCP23017_Register_Write(structure, register_address, content);
}

void IO_Expander_Init(mraa_i2c_context structure)
{
    IO_Expander_Write(structure, MCP23017_IOCON_ADDR, 0x28);				// Write 0x28 to IOCON register
    IO_Expander_Write(structure, MCP23017_IODIRA_ADDR, 0xC0);				// Set all GPIOA as output pins in IODIRA register except GPIO6, 7
    IO_Expander_Write(structure, MCP23017_IODIRB_ADDR, 0xC0);				// Set all GPIOB as output pins in IODIRB register except GPIO6, 7
}
