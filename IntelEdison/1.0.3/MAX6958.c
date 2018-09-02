/*
 * MAX6958.c
 *
 *  Created on: Feb 29, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "MAX6958.h"

void MAX6958_Access_Address(mraa_i2c_context structure, uint8_t address)
{
	mraa_i2c_address(structure, address);
}

void MAX6958_Register_Write(mraa_i2c_context structure, uint8_t address, uint8_t content)
{
	uint8_t rx_tx_buf[2];
    rx_tx_buf[0] = address;						// Access to register
	rx_tx_buf[1] = content;						// Write register content
    mraa_i2c_write(structure, rx_tx_buf, 2);

//	mraa_i2c_write_byte_data(structure, content, address);
}
