/*
 * Serial.c
 *
 *  Created on: Jan 23, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "Serial.h"

mraa_i2c_context I2C_Pin(int pin)
{
	return mraa_i2c_init(pin);
}

void I2C_Frequency(mraa_i2c_context structure, mraa_i2c_mode_t speed)
{

	if(speed == 2)
	{
		speed = MRAA_I2C_HIGH;					// Up to 3.4MHz
	}

	else if(speed == 1)
	{
		speed = MRAA_I2C_FAST;					// Up to 400kHz
	}

	else if(speed == 0)
	{
		speed = MRAA_I2C_STD;					// Up to 100kHz
	}

	mraa_i2c_frequency(structure, speed);
}
