#Copyright (c) Microsoft. All rights reserved.
#Licensed under the MIT license. See LICENSE file in the project root for full license information.

#this is CMakeLists.txt for sample3

compileAsC99()

if(NOT ${use_amqp})
	message(FATAL_ERROR "Test being generated without amqp support")
endif()

set(sample3_c_files
Application.c
MAX6958.c
MCP23017.c
Serial.c
Utility.c
jsmn.c
)

set(sample3_c_files ${sample3_c_files} ./Main.c)

set(sample3_h_files
Application.h
MAX6958.h
MCP23017.h
Serial.h
Utility.h
jsmn.h
)

include_directories(. ${SHARED_UTIL_INC_FOLDER} ${IOTHUB_CLIENT_INC_FOLDER})
link_directories(${whatIsBuilding}_dll ${SHARED_UTIL_LIB_DIR})

add_executable(sample3 ${sample3_c_files} ${sample3_h_files})

target_link_libraries(sample3 serializer iothub_client iothub_client_amqp_transport mraa)

linkSharedUtil(sample3)
linkUAMQP(sample3)
