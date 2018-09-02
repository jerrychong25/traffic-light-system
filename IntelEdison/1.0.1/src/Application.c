/*
 * Application.c
 *
 *  Created on: Jan 24, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include "mraa.h"
#include "Serial.h"
#include "MCP23017.h"
#include "Application.h"
#include "Utility.h"

void Variable_Assign(mraa_i2c_context structure, uint16_t input)
{
	junction temp_cross;

	temp_cross.northeast = 0;
	temp_cross.southwest = 0;

	temp_cross.northeast |= ((input & 0x0FC0) >> 6);
	temp_cross.southwest |= (input & 0x003F);

	IO_Expander_Write(structure, MCP23017_GPIOA_ADDR, temp_cross.northeast);		// Write cross.northeast to GPIOA register
	delay_microseconds(1);
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

			final = (input & 0xF1FF) | temp;
			break;

		case 2:
			temp = 0x0100;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFE3F) | temp;
			break;

		case 3:
			temp = 0x0020;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFC7) | temp;
			break;

		case 4:
			temp = 0x0004;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFF8) | temp;
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
			}

			//////////////////////////
			break;
	}


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
