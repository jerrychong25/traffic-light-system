#include <stdio.h>
#include <stdlib.h>
#include "mraa.h"
#include "Serial.h"
#include "MCP23017.h"

int main(int argc, char** argv)
{
	mraa_init();

    mraa_i2c_context i2c;
    i2c = I2C_Pin(6);												// Arduino breakout board only have bus 6 I2C
    I2C_Frequency(i2c, 1);

	MCP23017_Access_Address(i2c, MCP23017_I2C_ADDR);
	MCP23017_Register_Write(i2c, MCP23017_IOCON_ADDR, 0x28);		// Write 0x28 to IOCON register

	MCP23017_Access_Address(i2c, MCP23017_I2C_ADDR);
	MCP23017_Register_Write(i2c, MCP23017_IODIRA_ADDR, 0x00);		// Set all GPIOA as output pins in IODIRA register

    while(1)
    {
    	uint8_t c;

    	for (c = 1; c > 0; c = c << 1)								// Rotate bit
    	{
    		MCP23017_Access_Address(i2c, MCP23017_I2C_ADDR);
    		MCP23017_Register_Write(i2c, MCP23017_GPIOA_ADDR, c);	// Write c to IODIRA register
            sleep(1);												// Wait 1 second between cycles
        }
    }
}
