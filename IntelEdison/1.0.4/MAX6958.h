/*
 * MAX6958.h
 *
 *  Created on: Feb 29, 2016
 *      Author: user
 */

#ifndef MAX6958_H_
#define MAX6958_H_

// MAX6958 Device Address
#define MAX6958_I2C_ADDR 			0x38		// 7-bits slave address

// Register Address
#define MAX6958_DECODE_ADDR			0x01
#define MAX6958_INTENSITY_ADDR		0x02
#define MAX6958_SCANLIMIT_ADDR		0x03
#define MAX6958_CONFIG_ADDR			0x04
#define MAX6958_DISPLAYTEST_ADDR	0x07
#define MAX6958_DIGIT0_ADDR			0x20
#define MAX6958_DIGIT1_ADDR			0x21
#define MAX6958_DIGIT2_ADDR			0x22
#define MAX6958_DIGIT3_ADDR			0x23
#define MAX6958_SEGMENT_ADDR		0x24

// Global Function
extern void MAX6958_Access_Address(mraa_i2c_context structure, uint8_t address);
extern void MAX6958_Register_Write(mraa_i2c_context structure, uint8_t address, uint8_t content);

#endif /* MAX6958_H_ */
