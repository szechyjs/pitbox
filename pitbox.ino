/*******************************************************************************
                               Pit Box Controller
********************************************************************************

  Author:  Jared Szechy
  Version: v1.0.0
  Date:    1/2/2013

  Description of the operation of the controller will go here.

*******************************************************************************/

// To Disable serial messages, comment out the following line
#define VERBOSE

// Pin Configuration
#define PIN_STATUS_LED  13
#define PIN_RED_LEDS    11
#define PIN_GREEN_LEDS  10
#define PIN_TRIGGER     9

// Constants
#define RED_ON_TIME     10 * 1000
#define GREEN_ON_TIME   3 * 1000

void setup()
{

#ifdef VERBOSE
  Serial.begin(9600);
#endif

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

void loop()
{
  // Check if button has been pressed
  if (digitalRead(PIN_TRIGGER) == LOW)
  {
    // Turn on red LEDs
    digitalWrite(PIN_RED_LEDS, HIGH);

    // Wait X seconds
    delay(RED_ON_TIME);

    // Turn off red LEDs
    digitalWrite(PIN_RED_LEDS, LOW);

    // Turn on green LEDs
    digitalWrite(PIN_GREEN_LEDS, HIGH);

    // Wait Y seconds
    delay(GREEN_ON_TIME);

    // Turn off green LEDs
    digitalWrite(PIN_GREEN_LEDS, LOW);
  }
}