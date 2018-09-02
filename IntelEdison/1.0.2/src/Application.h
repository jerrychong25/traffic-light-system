/*
 * Application.h
 *
 *  Created on: Jan 24, 2016
 *      Author: user
 */

#ifndef APPLICATION_H_
#define APPLICATION_H_

// Define Structure
typedef struct
{
	uint8_t northeast;
	uint8_t southwest;
} junction;

typedef struct
{
	uint8_t north;
	uint8_t east;
	uint8_t south;
	uint8_t west;
} direction;

// Define Cardinal Direction
#define NORTH			2
#define EAST			1
#define	SOUTH			4
#define	WEST			3

// Define Traffic Light Phase
#define GREEN			0
#define YELLOW			1
#define RED				2

// Global Function
extern void Variable_Assign(mraa_i2c_context structure, uint16_t input);
extern void Light_Change(mraa_i2c_context structure, uint16_t input, uint8_t junc, uint8_t phase);
extern void Traffic_Initial(mraa_i2c_context structure, uint16_t temp_input);
extern void Traffic_Run(mraa_i2c_context structure, uint16_t temp_input, uint8_t temp_junc, uint8_t temp_time);
extern void Traffic_Mode(mraa_i2c_context structure, uint16_t input, uint8_t mode, direction time);
extern uint8_t Priority_Sort(direction priority);
extern void System_Run(void);
extern void Seven_Segment_Driver_Test(mraa_i2c_context structure, uint8_t value);
extern void Seven_Segment_Driver_Show(mraa_i2c_context structure, uint8_t value, uint8_t sequence);
extern void Seven_Segment_Driver_ShowTest(mraa_i2c_context structure, uint8_t content1, uint8_t content2, uint8_t content3, uint8_t content4);
extern void Seven_Segment_Driver_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content);
extern void Seven_Segment_Driver_Init(mraa_i2c_context structure);
extern void IO_Expander_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content);
extern void IO_Expander_Init(mraa_i2c_context structure);

#endif /* APPLICATION_H_ */
