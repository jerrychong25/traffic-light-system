/*
 * MCP23017.c
 *
 *  Created on: Jan 23, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "MCP23017.h"

void MCP23017_Access_Address(mraa_i2c_context structure, uint8_t address)
{
	mraa_i2c_address(structure, address);
}

void MCP23017_Register_Write(mraa_i2c_context structure, uint8_t address, uint8_t content)
{
	uint8_t rx_tx_buf[2];
    rx_tx_buf[0] = address;						// Access to register
	rx_tx_buf[1] = content;						// Write register content
    mraa_i2c_write(structure, rx_tx_buf, 2);
}

uint8_t MCP23017_Register_Read(mraa_i2c_context structure, uint8_t address)
{
	uint8_t buffer;
	buffer = mraa_i2c_read_byte_data(structure, address);

    return buffer;
}
