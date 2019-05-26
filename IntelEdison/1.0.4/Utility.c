/*
 * Utility.c
 *
 *  Created on: Jan 24, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "Utility.h"

void delay_seconds(uint16_t seconds)
{
	sleep(seconds);
}

void delay_miliseconds(uint16_t miliseconds)
{
	usleep(miliseconds * 1000);
}

void delay_microseconds(uint16_t microseconds)
{
	usleep(microseconds);
}
