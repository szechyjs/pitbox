/*******************************************************************************
                               Pit Box Controller
********************************************************************************

  Author:  Jared Szechy
  Version: v1.0.0
  Date:    1/2/2013

  Description of the operation of the controller will go here.

*******************************************************************************/

#include <EEPROM.h>
#include "pitbox.h"
#include "packet.h"

static Packet gInputPacket;

// Structure that holds the state of incoming serial bytes.
typedef struct {
  uint8_t header_bytes_read;
  uint8_t payload_bytes_remain;
  bool bave_packet;
}  RxPacketStat;

static RxPacketStat gPacketStat;

struct ConfigStruct {
  unsigned int redTime_ms, greenTime_ms, resetTime_ms;
  char version[4];
} config = {
  8000, 3000, 1000,
  CONFIG_VERSION
};

//
// Serial I/O
//

void writeHelloPacket()
{
  int foo = FIRMWARE_VERSION;
  Packet packet;
  packet.SetType(PBM_HELLO_ID);
  packet.AddTag(PBM_HELLO_TAG_FIRMWARE_VERSION, sizeof(foo), (char*)&foo);
  packet.Print();
}

void saveConfig()
{
  for (unsigned int t=0; t < sizeof(config); t++)
  {
    // Write to EEPROM
    EEPROM.write(CONFIG_START + t, *((char*)&config + t));

    // Verify the data
    if (EEPROM.read(CONFIG_START + t) != *((char*)&config + t))
    {
      // Error writing to the EEPROM
    }
  }
}

void loadConfig()
{
  // Make sure there are settings,
  // if nothing is found, use the default settings.
  if (EEPROM.read(CONFIG_START + sizeof(config) - 1) == config.version[3] &&
      EEPROM.read(CONFIG_START + sizeof(config) - 2) == config.version[2] &&
      EEPROM.read(CONFIG_START + sizeof(config) - 3) == config.version[1] &&
      EEPROM.read(CONFIG_START + sizeof(config) - 4) == config.version[0])
  {
    // Read config from EEPROM
    for (unsigned int t=0; t < sizeof(config); t++)
      *((char*)&config + t) = EEPROM.read(CONFIG_START + t);
  } else {
    // Settings aren't valid, overwrite with defaults
    saveConfig();
  }
}

void setup()
{
  memset(&gPacketStat, 0, sizeof(RxPacketStat));

  // Read configuration from EEPROM
  loadConfig();

  // Set pin modes
  pinMode(PIN_STATUS_LED, OUTPUT);
  pinMode(PIN_RED_LEDS, OUTPUT);
  pinMode(PIN_GREEN_LEDS, OUTPUT);
  pinMode(PIN_TRIGGER, INPUT);

  // Initialize pins
  digitalWrite(PIN_RED_LEDS, LOW);
  digitalWrite(PIN_GREEN_LEDS, LOW);
  digitalWrite(PIN_TRIGGER, HIGH);
  digitalWrite(PIN_STATUS_LED, HIGH);

  Serial.begin(9600);
  writeHelloPacket();
}

void redLeds(int time)
{
  digitalWrite(PIN_RED_LEDS, HIGH);
  delay(time);
  digitalWrite(PIN_RED_LEDS, LOW);
}

void greenLeds(int time)
{
  digitalWrite(PIN_GREEN_LEDS, HIGH);
  delay(time);
  digitalWrite(PIN_GREEN_LEDS, LOW);
}

void loop()
{
  // Check if button has been pressed
  if (digitalRead(PIN_TRIGGER) == LOW)
  {
    // Turn on red LEDs
    redLeds(config.redTime_ms);

    // Turn on green LEDs
    greenLeds(config.greenTime_ms);

    // Don't allow another button press yet
    delay(config.resetTime_ms);
  }
}
