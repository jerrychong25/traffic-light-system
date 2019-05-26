/*
 * Application.c
 *
 *  Created on: Jan 24, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <mraa.h>
#include "Application.h"
#include "Serial.h"
#include "Utility.h"
#include "MCP23017.h"
#include "MAX6958.h"

// Receive JSON Messages Library
#include <string.h>
#include "jsmn.h"

// Microsoft Azure IoT Library
#include "serializer.h"
#include "azure_c_shared_utility/threadapi.h"
#include "azure_c_shared_utility/sastoken.h"
#include "iothub_client.h"
#include "iothubtransportamqp.h"
#include "iothub_client_ll.h"
#include "azure_c_shared_utility/platform.h"

#ifdef MBED_BUILD_TIMESTAMP
#include "certs.h"
#endif // MBED_BUILD_TIMESTAMP

/*String containing Hostname, Device Id & Device Key in the format:             */
/*  "HostName=<host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>"    */
static const char* connectionString = "HostName=trafficIoT.azure-devices.net;DeviceId=Edison01;SharedAccessKey=WxVdxNDST9AVqtr9W1g48CR9f3MZXWITs+hZjtIr+L0=";

const char certificates[] =
/* Baltimore */
"-----BEGIN CERTIFICATE-----\r\n"
"MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\r\n"
"RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\r\n"
"VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\r\n"
"DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\r\n"
"ZTETMBEGA1UECxMKQ3liZXJUcnVzdDEiMCAGA1UEAxMZQmFsdGltb3JlIEN5YmVy\r\n"
"VHJ1c3QgUm9vdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKMEuyKr\r\n"
"mD1X6CZymrV51Cni4eiVgLGw41uOKymaZN+hXe2wCQVt2yguzmKiYv60iNoS6zjr\r\n"
"IZ3AQSsBUnuId9Mcj8e6uYi1agnnc+gRQKfRzMpijS3ljwumUNKoUMMo6vWrJYeK\r\n"
"mpYcqWe4PwzV9/lSEy/CG9VwcPCPwBLKBsua4dnKM3p31vjsufFoREJIE9LAwqSu\r\n"
"XmD+tqYF/LTdB1kC1FkYmGP1pWPgkAx9XbIGevOF6uvUA65ehD5f/xXtabz5OTZy\r\n"
"dc93Uk3zyZAsuT3lySNTPx8kmCFcB5kpvcY67Oduhjprl3RjM71oGDHweI12v/ye\r\n"
"jl0qhqdNkNwnGjkCAwEAAaNFMEMwHQYDVR0OBBYEFOWdWTCCR1jMrPoIVDaGezq1\r\n"
"BE3wMBIGA1UdEwEB/wQIMAYBAf8CAQMwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3\r\n"
"DQEBBQUAA4IBAQCFDF2O5G9RaEIFoN27TyclhAO992T9Ldcw46QQF+vaKSm2eT92\r\n"
"9hkTI7gQCvlYpNRhcL0EYWoSihfVCr3FvDB81ukMJY2GQE/szKN+OMY3EU/t3Wgx\r\n"
"jkzSswF07r51XgdIGn9w/xZchMB5hbgF/X++ZRGjD8ACtPhSNzkE1akxehi/oCr0\r\n"
"Epn3o0WC4zxe9Z2etciefC7IpJ5OCBRLbf1wbWsaY71k5h+3zvDyny67G7fyUIhz\r\n"
"ksLi4xaNmjICq44Y3ekQEe5+NauQrz4wlHrQMz2nZQ/1/I6eYs9HRCwBXbsdtTLS\r\n"
"R9I4LtD+gdwyah617jzV/OeBHRnDJELqYzmp\r\n"
"-----END CERTIFICATE-----\r\n"
/* MSIT */
"-----BEGIN CERTIFICATE-----\r\n"
"MIIFhjCCBG6gAwIBAgIEByeaqTANBgkqhkiG9w0BAQsFADBaMQswCQYDVQQGEwJJ\r\n"
"RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\r\n"
"VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTEzMTIxOTIwMDczMloX\r\n"
"DTE3MTIxOTIwMDY1NVowgYsxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n\r\n"
"dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y\r\n"
"YXRpb24xFTATBgNVBAsTDE1pY3Jvc29mdCBJVDEeMBwGA1UEAxMVTWljcm9zb2Z0\r\n"
"IElUIFNTTCBTSEEyMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA0eg3\r\n"
"p3aKcEsZ8CA3CSQ3f+r7eOYFumqtTicN/HJq2WwhxGQRlXMQClwle4hslAT9x9uu\r\n"
"e9xKCLM+FvHQrdswbdcaHlK1PfBHGQPifaa9VxM/VOo6o7F3/ELwY0lqkYAuMEnA\r\n"
"iusrr/466wddBvfp/YQOkb0JICnobl0JzhXT5+/bUOtE7xhXqwQdvDH593sqE8/R\r\n"
"PVGvG8W1e+ew/FO7mudj3kEztkckaV24Rqf/ravfT3p4JSchJjTKAm43UfDtWBpg\r\n"
"lPbEk9jdMCQl1xzrGZQ1XZOyrqopg3PEdFkFUmed2mdROQU6NuryHnYrFK7sPfkU\r\n"
"mYsHbrznDFberL6u23UykJ5jvXS/4ArK+DSWZ4TN0UI4eMeZtgzOtg/pG8v0Wb4R\r\n"
"DsssMsj6gylkeTyLS/AydGzzk7iWa11XWmjBzAx5ihne9UkCXgiAAYkMMs3S1pbV\r\n"
"S6Dz7L+r9H2zobl82k7X5besufIlXwHLjJaoKK7BM1r2PwiQ3Ov/OdgmyBKdHJqq\r\n"
"qcAWjobtZ1KWAH8Nkj092XA25epCbx+uleVbXfjQOsfU3neG0PyeTuLiuKloNwnE\r\n"
"OeOFuInzH263bR9KLxgJb95KAY8Uybem7qdjnzOkVHxCg2i4pd+/7LkaXRM72a1o\r\n"
"/SAKVZEhZPnXEwGgCF1ZiRtEr6SsxwUQ+kFKqPsCAwEAAaOCASAwggEcMBIGA1Ud\r\n"
"EwEB/wQIMAYBAf8CAQAwUwYDVR0gBEwwSjBIBgkrBgEEAbE+AQAwOzA5BggrBgEF\r\n"
"BQcCARYtaHR0cDovL2N5YmVydHJ1c3Qub21uaXJvb3QuY29tL3JlcG9zaXRvcnku\r\n"
"Y2ZtMA4GA1UdDwEB/wQEAwIBhjAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUH\r\n"
"AwIwHwYDVR0jBBgwFoAU5Z1ZMIJHWMys+ghUNoZ7OrUETfAwQgYDVR0fBDswOTA3\r\n"
"oDWgM4YxaHR0cDovL2NkcDEucHVibGljLXRydXN0LmNvbS9DUkwvT21uaXJvb3Qy\r\n"
"MDI1LmNybDAdBgNVHQ4EFgQUUa8kJpz0aCJXgCYrO0ZiFXsezKUwDQYJKoZIhvcN\r\n"
"AQELBQADggEBAHaFxSMxH7Rz6qC8pe3fRUNqf2kgG4Cy+xzdqn+I0zFBNvf7+2ut\r\n"
"mIx4H50RZzrNS+yovJ0VGcQ7C6eTzuj8nVvoH8tWrnZDK8cTUXdBqGZMX6fR16p1\r\n"
"xRspTMn0baFeoYWTFsLLO6sUfUT92iUphir+YyDK0gvCNBW7r1t/iuCq7UWm6nnb\r\n"
"2DVmVEPeNzPR5ODNV8pxsH3pFndk6FmXudUu0bSR2ndx80oPSNI0mWCVN6wfAc0Q\r\n"
"negqpSDHUJuzbEl4K1iSZIm4lTaoNKrwQdKVWiRUl01uBcSVrcR6ozn7eQaKm6ZP\r\n"
"2SL6RE4288kPpjnngLJev7050UblVUfbvG4=\r\n"
"-----END CERTIFICATE-----\r\n"
/* *.azure-devices.net */
"-----BEGIN CERTIFICATE-----\r\n"
"MIIGcjCCBFqgAwIBAgITWgABtrNbz7vBeV0QWwABAAG2szANBgkqhkiG9w0BAQsF\r\n"
"ADCBizELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT\r\n"
"B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEVMBMGA1UE\r\n"
"CxMMTWljcm9zb2Z0IElUMR4wHAYDVQQDExVNaWNyb3NvZnQgSVQgU1NMIFNIQTIw\r\n"
"HhcNMTUwODI3MDMxODA0WhcNMTcwODI2MDMxODA0WjAeMRwwGgYDVQQDDBMqLmF6\r\n"
"dXJlLWRldmljZXMubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA\r\n"
"nXC/qBUdlnfIm5K3HYu0o/Mb5tNNcsr0xy4Do0Puwq2W1tz0ZHvIIS9VOANhkNCb\r\n"
"VyOncnP6dvmM/rYYKth/NQ8RUiZOYlROZ0SYC8cvxq9WOln4GXtEU8vNVqJbYrJj\r\n"
"rPMHfxqLzTE/0ZnQffnDT3iMUE9kFLHow0YgaSRU0KZsc9KAROmzBzu+QIB1WGKX\r\n"
"D7CN361tG1UuN68Bz7MSnbgk98Z+DjDxfusoDhiiy/Y9MLOJMt4WIy5BqL3lfLnn\r\n"
"r+JLqmpiFuyVUDacFQDprYJ1/AFgcsKYu/ydmASARPzqJhOGaC2sZP0U5oBOoBzI\r\n"
"bz4tfn8Bi0kJKmS53mQt+wIDAQABo4ICOTCCAjUwCwYDVR0PBAQDAgSwMB0GA1Ud\r\n"
"JQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAdBgNVHQ4EFgQUKpYehBSNA53Oxivn\r\n"
"aLCz3+eFUJ0wXQYDVR0RBFYwVIITKi5henVyZS1kZXZpY2VzLm5ldIIaKi5hbXFw\r\n"
"d3MuYXp1cmUtZGV2aWNlcy5uZXSCISouc3UubWFuYWdlbWVudC1henVyZS1kZXZp\r\n"
"Y2VzLm5ldDAfBgNVHSMEGDAWgBRRryQmnPRoIleAJis7RmIVex7MpTB9BgNVHR8E\r\n"
"djB0MHKgcKBuhjZodHRwOi8vbXNjcmwubWljcm9zb2Z0LmNvbS9wa2kvbXNjb3Jw\r\n"
"L2NybC9tc2l0d3d3Mi5jcmyGNGh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kv\r\n"
"bXNjb3JwL2NybC9tc2l0d3d3Mi5jcmwwcAYIKwYBBQUHAQEEZDBiMDwGCCsGAQUF\r\n"
"BzAChjBodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL21zY29ycC9tc2l0d3d3\r\n"
"Mi5jcnQwIgYIKwYBBQUHMAGGFmh0dHA6Ly9vY3NwLm1zb2NzcC5jb20wTgYDVR0g\r\n"
"BEcwRTBDBgkrBgEEAYI3KgEwNjA0BggrBgEFBQcCARYoaHR0cDovL3d3dy5taWNy\r\n"
"b3NvZnQuY29tL3BraS9tc2NvcnAvY3BzADAnBgkrBgEEAYI3FQoEGjAYMAoGCCsG\r\n"
"AQUFBwMBMAoGCCsGAQUFBwMCMA0GCSqGSIb3DQEBCwUAA4ICAQCrjzOSW+X6v+UC\r\n"
"u+JkYyuypXN14pPLcGFbknJWj6DAyFWXKC8ihIYdtf/szWIO7VooplSTZ05u/JYu\r\n"
"ZYh7fAw27qih9CLhhfncXi5yzjgLMlD0mlbORvMJR/nMl7Yh1ki9GyLnpOqMmO+E\r\n"
"yTpOiE07Uyt2uWelLHjMY8kwy2bSRXIp7/+A8qHRaIIdXNtAKIK5jo068BJpo77h\r\n"
"4PljCb9JFdEt6sAKKuaP86Y+8oRZ7YzU4TLDCiK8P8n/gQXH0vvhOE/O0n7gWPqB\r\n"
"n8KxsnRicop6tB6GZy32Stn8w0qktmQNXOGU+hp8OL6irULWZw/781po6d78nmwk\r\n"
"1IFl2TB4+jgyblvJdTM0rx8vPf3F2O2kgsRNs9M5qCI7m+he43Bhue0Fj/h3oIIo\r\n"
"Qx7X/uqc8j3VTNE9hf2A4wksSRgRydjAYoo+bduNagC5s7Eucb4mBG0MMk7HAQU9\r\n"
"m/gyaxqth6ygDLK58wojSV0i4RiU01qZkHzqIWv5FhhMjbFwyKEc6U35Ps7kP/1O\r\n"
"fdGm13ONaYqDl44RyFsLFFiiDYxZFDSsKM0WDxbl9ULAlVc3WR85kEBK6I+pSQj+\r\n"
"7/Z5z2zTz9qOFWgB15SegTbjSR7uk9mEVnj9KDlGtG8W1or0EGrrEDP2CMsp0oEj\r\n"
"VTJbZAxEaZ3cVCKva5sQUxFMjwG32g==\r\n"
"-----END CERTIFICATE-----\r\n";

// Define the Model
BEGIN_NAMESPACE(WeatherStation);

DECLARE_MODEL(ContosoAnemometer,
WITH_DATA(ascii_char_ptr, DeviceId),
WITH_DATA(int, nt1),
WITH_DATA(int, et1),
WITH_DATA(int, st1),
WITH_DATA(int, wt1),
WITH_DATA(int, nt2),
WITH_DATA(int, et2),
WITH_DATA(int, st2),
WITH_DATA(int, wt2),
WITH_ACTION(SetGreenTime, int, north_green_time, int, north_priority, int, east_green_time, int, east_priority, int, south_green_time, int, south_priority, int, west_green_time, int, west_priority)
);

END_NAMESPACE(WeatherStation);

DEFINE_ENUM_STRINGS(IOTHUB_CLIENT_CONFIRMATION_RESULT, IOTHUB_CLIENT_CONFIRMATION_RESULT_VALUES)

void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void* userContextCallback)
{
    int messageTrackingId = (intptr_t)userContextCallback;

    (void)printf("Message Id: %d Received.\r\n", messageTrackingId);

    (void)printf("Result Call Back Called! Result is: %s \r\n", ENUM_TO_STRING(IOTHUB_CLIENT_CONFIRMATION_RESULT, result));
}

static void sendMessage(IOTHUB_CLIENT_HANDLE iotHubClientHandle, const unsigned char* buffer, size_t size)
{
    static unsigned int messageTrackingId;
    IOTHUB_MESSAGE_HANDLE messageHandle = IoTHubMessage_CreateFromByteArray(buffer, size);
    if (messageHandle == NULL)
    {
        printf("unable to create a new IoTHubMessage\r\n");
    }
    else
    {
        if (IoTHubClient_SendEventAsync(iotHubClientHandle, messageHandle, sendCallback, (void*)(uintptr_t)messageTrackingId) != IOTHUB_CLIENT_OK)
        {
            printf("failed to hand over the message to IoTHubClient");
        }
        else
        {
            printf("IoTHubClient accepted the message for delivery\r\n");
        }

        IoTHubMessage_Destroy(messageHandle);
    }
    free((void*)buffer);
    messageTrackingId++;
}

static IOTHUBMESSAGE_DISPOSITION_RESULT IoTHubMessage(IOTHUB_MESSAGE_HANDLE message, void* userContextCallback)
{
    IOTHUBMESSAGE_DISPOSITION_RESULT result;
    const unsigned char* buffer;
    size_t size;
    if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
    {
        printf("unable to IoTHubMessage_GetByteArray\r\n");
        result = EXECUTE_COMMAND_ERROR;
    }

    else
    {
	int i;
	int r;
	jsmn_parser p;
	jsmntok_t t[128];

	jsmn_init(&p);
	r = jsmn_parse(&p, buffer, strlen(buffer), t, sizeof(t) / sizeof(t[0]));

	char keyString[129];

	for(i = 1;i < r;i++)
	{
		jsmntok_t key = t[i];
		unsigned int length = key.end - key.start;
		memcpy(keyString, &buffer[key.start], length);
		keyString[length] = '\0';
		printf("Key: %s\n", keyString);
	}

        /*buffer is not zero terminated*/
        char* temp = malloc(size + 1);
        if (temp == NULL)
        {
            printf("failed to malloc\r\n");
            result = EXECUTE_COMMAND_ERROR;
        }
        else
        {
            memcpy(temp, buffer, size);
            temp[size] = '\0';
            EXECUTE_COMMAND_RESULT executeCommandResult = EXECUTE_COMMAND(userContextCallback, temp);
            result =
                (executeCommandResult == EXECUTE_COMMAND_ERROR) ? IOTHUBMESSAGE_ABANDONED :
                (executeCommandResult == EXECUTE_COMMAND_SUCCESS) ? IOTHUBMESSAGE_ACCEPTED :
                IOTHUBMESSAGE_REJECTED;
            free(temp);
	    (void)printf("Received Messages Success!!\r\n");
        }
    }
    return result;
}

void SendMessage1(void)
{
    if (platform_init() != 0)
    {
        printf("Failed to initialize the platform.\r\n");
    }

    else
    {
        if (serializer_init(NULL) != SERIALIZER_OK)
        {
            (void)printf("Failed on serializer_init\r\n");
        }

        else
        {
            /* Setup IoTHub client configuration */
            IOTHUB_CLIENT_HANDLE iotHubClientHandle = IoTHubClient_CreateFromConnectionString(connectionString, AMQP_Protocol);
            srand((unsigned int)time(NULL));

            if (iotHubClientHandle == NULL)
            {
                (void)printf("Failed on IoTHubClient_Create\r\n");
            }

            else
            {
                // For mbed add the certificate information
                if (IoTHubClient_SetOption(iotHubClientHandle, "TrustedCerts", certificates) != IOTHUB_CLIENT_OK)
                {
                    (void)printf("failure to set option \"TrustedCerts\"\r\n");
                }

                ContosoAnemometer* myWeather = CREATE_MODEL_INSTANCE(WeatherStation, ContosoAnemometer);

                if (myWeather == NULL)
                {
                    (void)printf("Failed on CREATE_MODEL_INSTANCE\r\n");
                }

                else
                {
                    unsigned char* destination;
                    size_t destinationSize;

                    if (IoTHubClient_SetMessageCallback(iotHubClientHandle, IoTHubMessage, myWeather) != IOTHUB_CLIENT_OK)
                    {
                        printf("unable to IoTHubClient_SetMessageCallback\r\n");
                    }

                    else
                    {
                        myWeather->DeviceId = "Edison01";
                        myWeather->nt1 = rand() % 50;
                        myWeather->et1 = rand() % 50;
                        myWeather->st1 = rand() % 50;
                        myWeather->wt1 = rand() % 50;
                        myWeather->nt2 = rand() % 50;
                        myWeather->et2 = rand() % 50;
                        myWeather->st2 = rand() % 50;
                        myWeather->wt2 = rand() % 50;

                        if (SERIALIZE(&destination, &destinationSize, myWeather->DeviceId, myWeather->nt1, myWeather->et1, myWeather->st1, myWeather->wt1, myWeather->nt2, myWeather->et2, myWeather->st2, myWeather->wt2) != IOT_AGENT_OK)
                        {
                            (void)printf("Failed to serialize\r\n");
                        }

                        else
                        {
                            sendMessage(iotHubClientHandle, destination, destinationSize);
                            (void)printf("Serialize Success!!\r\n");
                        }

                        /* wait for commands */
                        (void)getchar();
                    }
                    DESTROY_MODEL_INSTANCE(myWeather);
                }
                IoTHubClient_Destroy(iotHubClientHandle);
            }
            serializer_deinit();
        }
        platform_deinit();
    }
}

void SendMessage2(void)
{
	/* Setup IoTHub client configuration */
        IOTHUB_CLIENT_HANDLE iotHubClientHandle = IoTHubClient_CreateFromConnectionString(connectionString, AMQP_Protocol);
        srand((unsigned int)time(NULL));

        if (iotHubClientHandle == NULL)
        {
            (void)printf("Failed on IoTHubClient_Create\r\n");
        }

        else
        {
            // For mbed add the certificate information
            if (IoTHubClient_SetOption(iotHubClientHandle, "TrustedCerts", certificates) != IOTHUB_CLIENT_OK)
            {
                (void)printf("failure to set option \"TrustedCerts\"\r\n");
            }

            ContosoAnemometer* myWeather = CREATE_MODEL_INSTANCE(WeatherStation, ContosoAnemometer);

            if (myWeather == NULL)
            {
                (void)printf("Failed on CREATE_MODEL_INSTANCE\r\n");
            }

            else
            {
                unsigned char* destination;
                size_t destinationSize;

                if (IoTHubClient_SetMessageCallback(iotHubClientHandle, IoTHubMessage, myWeather) != IOTHUB_CLIENT_OK)
                {
                    printf("unable to IoTHubClient_SetMessageCallback\r\n");
                }

                else
                {
                    myWeather->DeviceId = "Edison01";
                    myWeather->nt1 = rand() % 50;

                    myWeather->et1 = rand() % 50;
                    myWeather->st1 = rand() % 50;
                    myWeather->wt1 = rand() % 50;
                    myWeather->nt2 = rand() % 50;
                    myWeather->et2 = rand() % 50;
                    myWeather->st2 = rand() % 50;
                    myWeather->wt2 = rand() % 50;

                    if (SERIALIZE(&destination, &destinationSize, myWeather->DeviceId, myWeather->nt1, myWeather->et1, myWeather->st1, myWeather->wt1, myWeather->nt2, myWeather->et2, myWeather->st2, myWeather->wt2) != IOT_AGENT_OK)
                    {
                        (void)printf("Failed to serialize\r\n");
                    }

                    else
                    {
                        sendMessage(iotHubClientHandle, destination, destinationSize);
                        (void)printf("Serialize Success!!\r\n");
                    }

                    /* wait for commands */
                    (void)getchar();
                }
                DESTROY_MODEL_INSTANCE(myWeather);
            }
            IoTHubClient_Destroy(iotHubClientHandle);
        }
}

EXECUTE_COMMAND_RESULT SetGreenTime(ContosoAnemometer* device, int north_green_time, int north_priority, int east_green_time, int east_priority, int south_green_time, int south_priority, int west_green_time, int west_priority)
{
    direction temp_junction;

    (void)device;
    (void)printf("North Green Time is: %d.\r\n", north_green_time);
    (void)printf("North Priority is: %d.\r\n", north_priority);
    (void)printf("East Green Time is: %d.\r\n", east_green_time);
    (void)printf("East Priority is: %d.\r\n", east_priority);
    (void)printf("South Green Time is: %d.\r\n", south_green_time);
    (void)printf("South Priority is: %d.\r\n", south_priority);
    (void)printf("West Green Time is: %d.\r\n", west_green_time);
    (void)printf("West Priority is: %d.\r\n", west_priority);

    temp_junction.north_green_time = north_green_time;
    temp_junction.north_priority = north_priority;
    temp_junction.east_green_time = east_green_time;
    temp_junction.east_priority = east_priority;
    temp_junction.south_green_time = south_green_time;
    temp_junction.south_priority = south_priority;
    temp_junction.west_green_time = west_green_time;
    temp_junction.west_priority = west_priority;

    System_Run2(temp_junction);

    return EXECUTE_COMMAND_SUCCESS;
}

void Seven_Segment_Driver_Test(mraa_i2c_context structure, uint8_t value)
{
	Seven_Segment_Driver_Show(structure, value, value, value, value);		// Write four times to make sure seven segment no errors at all
	Seven_Segment_Driver_Show(structure, value, value, value, value);
	Seven_Segment_Driver_Show(structure, value, value, value, value);
	Seven_Segment_Driver_Show(structure, value, value, value, value);
}

void Seven_Segment_Driver_Show(mraa_i2c_context structure, uint8_t value0, uint8_t value1, uint8_t value2, uint8_t value3)
{
	uint8_t rx_tx_buf[5];

    rx_tx_buf[0] = MAX6958_DIGIT0_ADDR;						// Access to register
	rx_tx_buf[1] = value0;					// Write digit0
	rx_tx_buf[2] = value1;					// Write digit1
	rx_tx_buf[3] = value2;					// Write digit2
	rx_tx_buf[4] = value3;					// Write digit3

	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);
	mraa_i2c_write(structure, rx_tx_buf, 5);
}

void Seven_Segment_Driver_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content)
{
	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);
	MAX6958_Register_Write(structure, register_address, content);
}

void Seven_Segment_Driver_Init(mraa_i2c_context structure)
{
	//Method 1

	uint8_t rx_tx_buf2[5];
	rx_tx_buf2[0] = MAX6958_DECODE_ADDR;						// Access to decode register
	rx_tx_buf2[1] = 0x0F;									// Write decode register
	rx_tx_buf2[2] = 0x04;									// Write intensity register
	rx_tx_buf2[3] = 0x03;									// Write scan limit register
	rx_tx_buf2[4] = 0x01;									// Write config register

	MAX6958_Access_Address(structure, MAX6958_I2C_ADDR);					// Access MAX6958
	mraa_i2c_write(structure, rx_tx_buf2, 5);
}

void IO_Expander1_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content)
{
	MCP23017_Access_Address(structure, MCP23017_I2C_ADDR);
	MCP23017_Register_Write(structure, register_address, content);
}

void IO_Expander1_Init(mraa_i2c_context structure)
{
    IO_Expander1_Write(structure, MCP23017_IOCON_ADDR, 0x28);				// Write 0x28 to IOCON register
    IO_Expander1_Write(structure, MCP23017_IODIRA_ADDR, 0xC0);				// Set all GPIOA as output pins in IODIRA register except GPIO6, 7
    IO_Expander1_Write(structure, MCP23017_IODIRB_ADDR, 0xC0);				// Set all GPIOB as output pins in IODIRB register except GPIO6, 7
}

void IO_Expander2_Write(mraa_i2c_context structure, uint8_t register_address, uint8_t content)
{
	MCP23017_Access_Address(structure, MCP23017_I2C_ADDR2);
	MCP23017_Register_Write(structure, register_address, content);
}

uint8_t IO_Expander2_Read(mraa_i2c_context structure, uint8_t register_address)
{
	uint8_t received_byte;

	MCP23017_Access_Address(structure, MCP23017_I2C_ADDR2);
	received_byte = MCP23017_Register_Read(structure, register_address);

	return received_byte;
}

void IO_Expander2_Init(mraa_i2c_context structure)
{
    IO_Expander2_Write(structure, MCP23017_IOCON_ADDR, 0x28);				// Write 0x28 to IOCON register
    IO_Expander2_Write(structure, MCP23017_IODIRA_ADDR, 0x0F);				// Set GPIO0-3 as input pins and GPIO4-7 as output pins in IODIRA register
    IO_Expander2_Write(structure, MCP23017_IODIRB_ADDR, 0x00);				// Set all GPIOB as output pins in IODIRB register
    IO_Expander2_Write(structure, MCP23017_GPINTENA_ADDR, 0x0F);			// Enable GPIOA0-3 interrupt-on-change event in GPINTENA register
    IO_Expander2_Write(structure, MCP23017_DEFVALA_ADDR, 0xF0);				// Set GPIOA0-3 default values zeroes in DEFVALA register
    IO_Expander2_Write(structure, MCP23017_INTCONA_ADDR, 0x0F);				// Set GPIOA0-3 compared for interrupt-on-change in INTCONA register
}

void Variable_Assign(mraa_i2c_context structure, uint16_t input)
{
	IOExpander_junction temp_cross;

	temp_cross.northeast = 0;
	temp_cross.southwest = 0;

	temp_cross.northeast |= ((input & 0x0FC0) >> 6);
	temp_cross.southwest |= (input & 0x003F);

	IO_Expander1_Write(structure, MCP23017_GPIOA_ADDR, temp_cross.northeast);		// Write cross.northeast to GPIOA register
	delay_microseconds(10);
	IO_Expander1_Write(structure, MCP23017_GPIOB_ADDR, temp_cross.southwest);		// Write cross.southwest to GPIOB register
}

void Light_Change(mraa_i2c_context structure, uint16_t input, uint8_t junction_turn, uint8_t phase)
{
	uint8_t i;
	uint16_t temp, final;

	switch(junction_turn)
	{
		case NORTH:
			temp = 0x0800;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xF1FF) | temp;		// Case 1 0000XXX000000000 (Bit 9-11)
			break;

		case EAST:
			temp = 0x0100;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFE3F) | temp;		// Case 2 0000000XXX000000 (Bit 6-8)
			break;

		case SOUTH:
			temp = 0x0020;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFC7) | temp;		// Case 3 0000000000XXX000 (Bit 3-5)
			break;

		case WEST:
			temp = 0x0004;

			for(i = 0; i < phase; i++)
			{
				temp >>= 1;
			}

			final = (input & 0xFFF8) | temp;		// Case 4 0000000000000XXX (Bit 0-2)
			break;
	}

	Variable_Assign(structure, final);
}

void Traffic_Initial(mraa_i2c_context structure, uint16_t temp_input)
{
	Light_Change(structure, temp_input, NORTH, RED);
	Light_Change(structure, temp_input, EAST, RED);
	Light_Change(structure, temp_input, SOUTH, RED);
	Light_Change(structure, temp_input, WEST, RED);
}

void Traffic_Run(mraa_i2c_context structure, uint16_t temp_input, uint8_t temp_junc, uint8_t temp_time)
{
	Light_Change(structure, temp_input, temp_junc, GREEN);
	delay_seconds(temp_time);
	Light_Change(structure, temp_input, temp_junc, YELLOW);
	delay_seconds(2);
	Light_Change(structure, temp_input, temp_junc, RED);
	delay_microseconds(100);
}

void Traffic_Mode(mraa_i2c_context structure, uint16_t input, uint8_t mode, direction junction)
{
	if(junction.north_priority == mode)
	{
		Traffic_Run(structure, input, 1, junction.north_green_time);
	}

	else if(junction.east_priority == mode)
	{
		Traffic_Run(structure, input, 2, junction.east_green_time);
	}

	else if(junction.south_priority == mode)
	{
		Traffic_Run(structure, input, 3, junction.south_green_time);
	}

	else if(junction.west_priority == mode)
	{
		Traffic_Run(structure, input, 4, junction.west_green_time);
	}
}

uint8_t Priority_Sort(direction junction)
{
	uint8_t i, temp_priority, temp_direction;
	uint8_t priority_array[4];
	uint8_t direction_array[4];

	priority_array[0] = junction.north_green_time;
	priority_array[1] = junction.east_green_time;
	priority_array[2] = junction.south_green_time;
	priority_array[3] = junction.west_green_time;

	direction_array[0] = junction.north_priority;
	direction_array[1] = junction.east_priority;
	direction_array[2] = junction.south_priority;
	direction_array[3] = junction.west_priority;

	for(i = 0; i < 4; i++)
	{
		if (priority_array[i] < priority_array[i+1])
		{
			temp_priority = priority_array[i];
			priority_array[i] = priority_array[i+1];
			priority_array[i+1] = temp_priority;

			temp_direction = direction_array[i];
			direction_array[i] = direction_array[i+1];
			direction_array[i+1] = temp_direction;
		}
	}

	return *direction_array;
}

void System_Run(void)
{
	uint8_t i, mode;

	static uint8_t first = 1;
	uint16_t initial_state = 0x0249;
	direction junction;

	mraa_init();
	mraa_i2c_context i2c;

	if(first == 1)
	{
		mode = 1;
	}

	switch(mode)
	{
		case 1:
			i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
			I2C_Frequency(i2c, 1);

		    IO_Expander1_Init(i2c);

		    IO_Expander1_Write(i2c, MCP23017_GPIOA_ADDR, 0x09);		// Set all to red light
		    IO_Expander1_Write(i2c, MCP23017_GPIOB_ADDR, 0x09);		// Set all to red light

		    first = 0;
		    mode = 2;

		case 2:											// Normal Mode

			// Junction Time & Priority
			junction.north_green_time = 5;
			junction.east_green_time = 5;
			junction.south_green_time = 5;
			junction.west_green_time = 5;

			junction.north_priority = 4;
			junction.east_priority = 2;
			junction.south_priority = 1;
			junction.west_priority = 3;

			for(i = 1;i < 5; i++)
			{
				Traffic_Mode(i2c, initial_state, i, junction);
			}
	}
}

void System_Run2(direction junction)
{
	uint8_t i, mode;

	static uint8_t first = 1;
	uint16_t initial_state = 0x0249;

	mraa_init();
	mraa_i2c_context i2c;

	if(first == 1)
	{
		mode = 1;
	}

	switch(mode)
	{
		case 1:
			i2c = I2C_Pin(6);							// Arduino breakout board only have bus 6 I2C
			I2C_Frequency(i2c, 1);

		    IO_Expander1_Init(i2c);

		    IO_Expander1_Write(i2c, MCP23017_GPIOA_ADDR, 0x09);		// Set all to red light
		    IO_Expander1_Write(i2c, MCP23017_GPIOB_ADDR, 0x09);		// Set all to red light

		    first = 0;
		    mode = 2;

		case 2:											// Normal Mode

			for(i = 1;i < 5; i++)
			{
				Traffic_Mode(i2c, initial_state, i, junction);
			}
	}
}

void System_Wait(void)
{
	while(1)
	{
		delay_miliseconds(100);
	}
}
