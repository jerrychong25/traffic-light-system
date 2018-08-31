/*
 * Serial.h
 *
 *  Created on: Jan 23, 2016
 *      Author: user
 */

#ifndef SERIAL_H_
#define SERIAL_H_

// Global Function
extern mraa_i2c_context I2C_Pin(int pin);
extern void I2C_Frequency(mraa_i2c_context structure, mraa_i2c_mode_t speed);

#endif /* SERIAL_H_ */
