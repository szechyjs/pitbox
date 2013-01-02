/*******************************************************************************
                               Pit Box Controller
********************************************************************************

  Author:  Jared Szechy
  Version: v1.0.0
  Date:    1/2/2013

  Description of the operation of the controller will go here.

*******************************************************************************/

#include <EEPROM.h>

// To enable serial debugging, uncomment the following line
//#define VERBOSE

// Pin Configuration
#define PIN_STATUS_LED  13
#define PIN_RED_LEDS    11
#define PIN_GREEN_LEDS  10
#define PIN_TRIGGER     9

// Configuration settings
#define CONFIG_VERSION "v1"
#define CONFIG_START 32

struct ConfigStruct {
  unsigned int redTime, greenTime;
  char version[4];
} config = {
  8, 3,
  CONFIG_VERSION
};

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

#ifdef VERBOSE
  Serial.begin(9600);
#endif

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
}

void redLeds(int time)
{
  digitalWrite(PIN_RED_LEDS, HIGH);
  delay(time * 1000);
  digitalWrite(PIN_RED_LEDS, LOW);
}

void greenLeds(int time)
{
  digitalWrite(PIN_GREEN_LEDS, HIGH);
  delay(time * 1000);
  digitalWrite(PIN_GREEN_LEDS, LOW);
}

void loop()
{
  // Check if button has been pressed
  if (digitalRead(PIN_TRIGGER) == LOW)
  {
    // Turn on red LEDs
    redLeds(config.redTime);

    // Turn on green LEDs
    greenLeds(config.greenTime);
  }
}
