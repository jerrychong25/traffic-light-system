/**
* Read a card using a mfrc522 reader on your SPI interface
* Pin layout should be as follows (on Arduino Uno):
* MOSI: Pin 11 / ICSP-4
* MISO: Pin 12 / ICSP-1
* SCK: Pin 13 / ISCP-3
* SS: Pin 10
* RST: Pin 9
*
* Script is based on the script of Miguel Balboa. 
* New cardnumber is printed when card has changed. Only a dot is printed
* if card is the same.
*
* @version 0.1
* @author Henri de Jong
* @since 06-01-2013
*/

#include <SPI.h>
#include "D:\University\Malaysia\UTAR\Course\Robot\Programming\Code\Arduino Uno\RFID Reader\rfid_buzzer\RFID.h"

#define SS_PIN 10
#define RST_PIN 9

RFID rfid(SS_PIN, RST_PIN); 

int buzzPin = 3;
int EdisonPin = 8;

// Setup variables:
int serNum0;
int serNum1;
int serNum2;
int serNum3;
int serNum4;

// Setup Card 1 
unsigned int Card1[5] = {67, 78, 99, 2, 108};

// Setup Card 2
unsigned int Card2[5] = {153, 22, 41, 0, 166};

void setup()
{ 
    pinMode(EdisonPin, OUTPUT);
    Serial.begin(9600);
    SPI.begin(); 
    rfid.init();
}

void loop()
{
    if (rfid.isCard()) 
    {
        if (rfid.readCardSerial()) 
        {
            if (rfid.serNum[0] != Card1[0] && rfid.serNum[1] != Card1[1] && rfid.serNum[2] != Card1[2] && rfid.serNum[3] != Card1[3] && rfid.serNum[4] != Card1[4]) 
            {
                Serial.print("Card 1 Detected!\n");
                digitalWrite(EdisonPin, HIGH);
            }
            
            else if (rfid.serNum[0] != Card2[0] && rfid.serNum[1] != Card2[1] && rfid.serNum[2] != Card2[2] && rfid.serNum[3] != Card2[3] && rfid.serNum[4] != Card2[4]) 
            {
                Serial.print("Card 2 Detected!\n");
                digitalWrite(EdisonPin, HIGH);
            }
          
            else
            {
                /* With a new cardnumber, show it. */
                Serial.println(" ");
                Serial.println("Card found");
                serNum0 = rfid.serNum[0];
                serNum1 = rfid.serNum[1];
                serNum2 = rfid.serNum[2];
                serNum3 = rfid.serNum[3];
                serNum4 = rfid.serNum[4];
               
                //Serial.println(" ");
                Serial.println("Cardnumber:");
                Serial.print("Dec: ");
		        Serial.print(rfid.serNum[0],DEC);
                Serial.print(", ");
		        Serial.print(rfid.serNum[1],DEC);
                Serial.print(", ");
		        Serial.print(rfid.serNum[2],DEC);
                Serial.print(", ");
		        Serial.print(rfid.serNum[3],DEC);
                Serial.print(", ");
		        Serial.print(rfid.serNum[4],DEC);
                Serial.println(" ");
                        
                Serial.print("Hex: ");
		        Serial.print(rfid.serNum[0],HEX);
                Serial.print(", ");
		        Serial.print(rfid.serNum[1],HEX);
                Serial.print(", ");
		        Serial.print(rfid.serNum[2],HEX);
                Serial.print(", ");
		        Serial.print(rfid.serNum[3],HEX);
                Serial.print(", ");
		        Serial.print(rfid.serNum[4],HEX);
                Serial.println(" ");
            } 
            
            delay(500);
            digitalWrite(EdisonPin, LOW);
          }
    }
    
    rfid.halt();
}



