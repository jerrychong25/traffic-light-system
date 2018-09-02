/*
 * MCP23017.h
 *
 *  Created on: Jan 23, 2016
 *      Author: user
 */

#ifndef MCP23017_H_
#define MCP23017_H_

// MCP23017 Device Address
#define MCP23017_I2C_ADDR 		0x20		// 7-bits slave address, last three digits can be modified by wiring
#define MCP23017_I2C_ADDR2 		0x21		// 7-bits slave address, last three digits can be modified by wiring

// Register Address
#define MCP23017_IODIRA_ADDR	0x00
#define MCP23017_IODIRB_ADDR	0x01
#define MCP23017_IPOLA_ADDR		0x02
#define MCP23017_IPOLB_ADDR		0x03
#define MCP23017_GPINTENA_ADDR	0x04
#define MCP23017_GPINTENB_ADDR	0x05
#define MCP23017_DEFVALA_ADDR	0x06
#define MCP23017_DEFVALB_ADDR	0x07
#define MCP23017_INTCONA_ADDR	0x08
#define MCP23017_INTCONB_ADDR	0x09
#define MCP23017_IOCON_ADDR		0x0A
#define MCP23017_GPPUA_ADDR		0x0C
#define MCP23017_GPPUB_ADDR		0X0D
#define MCP23017_INTFA_ADDR		0X0E
#define MCP23017_INTFB_ADDR		0X0F
#define MCP23017_INTCAPA_ADDR	0X10
#define MCP23017_INTCAPB_ADDR	0X11
#define MCP23017_GPIOA_ADDR		0X12
#define MCP23017_GPIOB_ADDR		0X13
#define MCP23017_OLATA_ADDR		0X14
#define MCP23017_OLATB_ADDR		0X15

// Global Function
extern void MCP23017_Access_Address(mraa_i2c_context structure, uint8_t address);
extern void MCP23017_Register_Write(mraa_i2c_context structure, uint8_t address, uint8_t content);
extern uint8_t MCP23017_Register_Read(mraa_i2c_context structure, uint8_t address);

#endif /* MCP23017_H_ */
