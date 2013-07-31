/*******************************************************************************
                               Pit Box Controller
********************************************************************************

  Author:  Jared Szechy
  Version: v1.0.0
  Date:    1/2/2013

  Description of the operation of the controller will go here.

*******************************************************************************/

#include <EEPROM.h>
#include <Timer.h>
#include "pitbox.h"
#include "packet.h"

static Packet gInputPacket;

// Structure that holds the state of incoming serial bytes.
typedef struct {
  uint8_t header_bytes_read;
  uint8_t payload_bytes_remain;
  bool have_packet;
}  RxPacketStat;

static RxPacketStat gPacketStat;

struct ConfigStruct {
  unsigned int redTime_ms, greenTime_ms, resetTime_ms;
  unsigned int redMode, greenMode;
  char version[4];
} config = {
  8000, 3000, 1000,
  0, 0,
  CONFIG_VERSION
};

unsigned long gLastTrigger = 0;
bool gSequenceActive = false;
bool gSequenceLocked = false;
int gEventRed, gEventGreen, gEventGreenDelay, gEventDelay, gEventSeq;
Timer gTimer;

//
// Serial I/O
//

void writeDebug(String message)
{
#ifdef VERBOSE
  Serial.println(message);
#endif
}

void writeHelloPacket()
{
  int foo = FIRMWARE_VERSION;
  Packet packet;
  packet.SetType(PBM_HELLO_ID);
  packet.AddTag(PBM_HELLO_TAG_FIRMWARE_VERSION, sizeof(foo), (char*)&foo);
  packet.Print();
}

/**
 * Saves the current configuration to the EEPROM
 */
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

/**
 * Loads the configuration stored in the EEPROM
 */
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

/**
 * Sends a packet with the current configuration
 */
void writeConfigPacket()
{
  Packet packet;
  packet.SetType(PBM_CFG);
  packet.AddTag(PBM_CFG_TAG_RED, sizeof(config.redTime_ms), (char*)(&config.redTime_ms));
  packet.AddTag(PBM_CFG_TAG_GRN, sizeof(config.greenTime_ms), (char*)(&config.greenTime_ms));
  packet.AddTag(PBM_CFG_TAG_DLY, sizeof(config.resetTime_ms), (char*)(&config.resetTime_ms));
  packet.AddTag(PBM_CFG_TAG_RED_MODE, sizeof(config.redMode), (char*)(&config.redMode));
  packet.AddTag(PBM_CFG_TAG_GRN_MODE, sizeof(config.greenMode), (char*)(&config.greenMode));
  packet.Print();
}

/**
 * Reads the incoming configuration packet and sets the configuration
 */
void readConfigPacket()
{
  uint16_t red_ms, green_ms, reset_ms;
  uint16_t redMode, greenMode;

  if (!gInputPacket.ReadTag(PBM_CFG_TAG_RED, &red_ms)
      || !gInputPacket.ReadTag(PBM_CFG_TAG_GRN, &green_ms)
      || !gInputPacket.ReadTag(PBM_CFG_TAG_DLY, &reset_ms)
      || !gInputPacket.ReadTag(PBM_CFG_TAG_RED_MODE, &redMode)
      || !gInputPacket.ReadTag(PBM_CFG_TAG_GRN_MODE, &greenMode)) {
    return;
  }
  config.redTime_ms   = red_ms;
  config.greenTime_ms = green_ms;
  config.resetTime_ms = reset_ms;
  config.redMode      = redMode;
  config.greenMode    = greenMode;
  saveConfig();
  writeConfigPacket();
}

void stopEvents()
{
  gTimer.stop(gEventRed);
  gTimer.stop(gEventGreen);
  gTimer.stop(gEventGreenDelay);
  gTimer.stop(gEventDelay);
  gTimer.stop(gEventSeq);
}

void abortSequence()
{
  writeDebug("Aborting");
  stopEvents(); 
  digitalWrite(PIN_RED_LEDS, LOW);
  digitalWrite(PIN_GREEN_LEDS, LOW);
  gSequenceActive = false;
  gSequenceLocked = false;
}

void triggerGreen()
{
  writeDebug("Triggering green");
  gEventGreen = gTimer.pulse(PIN_GREEN_LEDS, config.greenTime_ms, HIGH);
}

void delayTimeout()
{
  writeDebug("Delay timeout");
  gSequenceLocked = false;
}

void sequenceComplete()
{
  writeDebug("Completed");
  stopEvents();
  gSequenceActive = false;
}

/**
 * Handles the interrupt event from a button press
 */
void buttonTriggered()
{
  unsigned long mills = millis();
  if (mills - gLastTrigger < 250)
    return;
  
  gLastTrigger = mills;
  writeDebug("Triggered");
  
  
  if (!gSequenceLocked)
  {
    if (gSequenceActive)
      abortSequence();
  
    writeDebug("Starting sequence");
    gSequenceActive = true;
    gSequenceLocked = true;
    gEventRed = gTimer.pulse(PIN_RED_LEDS, config.redTime_ms, HIGH);
    gEventGreenDelay = gTimer.after(config.redTime_ms + 100, triggerGreen);
    gEventDelay = gTimer.after(config.resetTime_ms, delayTimeout);
    gEventSeq = gTimer.after(config.redTime_ms + config.greenTime_ms + 200, sequenceComplete);
  }
}

/**
 * Setup and configure the board
 */
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
  
  gSequenceActive = false;
  gEventRed = gEventGreen = gEventGreenDelay = gEventDelay = gEventSeq = 0;
  
  // Attach interrput to button
  attachInterrupt(0, buttonTriggered, FALLING);
}

static void readSerialBytes(char *dest_buf, int num_bytes, int offset)
{
  while (num_bytes-- != 0) {
    dest_buf[offset++] = Serial.read();
  }
}

void resetInputPacket()
{
  memset(&gPacketStat, 0, sizeof(RxPacketStat));
  gInputPacket.Reset();
}

void readIncomingSerialData()
{
  char serial_buf[PAYLOAD_MAXLEN];
  volatile uint8_t bytes_available = Serial.available();

  // Do not read a new packet if we have one awaiting processing
  // This should never happen.
  if (gPacketStat.have_packet) {
    return;
  }

  // Look for a new packet.
  if (gPacketStat.header_bytes_read < HEADER_PREFIX_LEN)
  {
    while (bytes_available > 0)
    {
      char next_char = Serial.read();
      bytes_available -= 1;

      if (next_char == PREFIX[gPacketStat.header_bytes_read])
      {
        gPacketStat.header_bytes_read++;
        if (gPacketStat.header_bytes_read == HEADER_PREFIX_LEN) {
          // Found start of packet, break.
          break;
        }
      }
      else
      {
        // Wrong character in prefix; reset framing.
        if (next_char == PREFIX[0]) {
          gPacketStat.header_bytes_read = 1;
        } else {
          gPacketStat.header_bytes_read = 0;
        }
      }
    }
  }

  // Read the remainder of the header, if not yet found.
  if (gPacketStat.header_bytes_read < HEADER_LEN)
  {
    if (bytes_available < 4) {
      return;
    }
    gInputPacket.SetType(Serial.read() | (Serial.read() << 8));
    gPacketStat.payload_bytes_remain = Serial.read() | (Serial.read() << 8);
    bytes_available -= 4;
    gPacketStat.header_bytes_read += 4;

    // Check that the 'len' field is not bogus. If it is, throw out the packet
    // and reset.
    if (gPacketStat.payload_bytes_remain > PAYLOAD_MAXLEN) {
      goto out_reset;
    }
  }

  // If we haven't yet found a frame, or there are no more bytes to read after
  // finda a frame, bail out.
  if (bytes_available == 0 || (gPacketStat.header_bytes_read < HEADER_LEN)) {
    return;
  }

  if (gPacketStat.payload_bytes_remain)
  {
    int bytes_to_read = (gPacketStat.payload_bytes_remain >= bytes_available) ?
      bytes_available : gPacketStat.payload_bytes_remain;
    readSerialBytes(serial_buf, bytes_to_read, 0);
    gInputPacket.AppendBytes(serial_buf, bytes_to_read);
    gPacketStat.payload_bytes_remain -= bytes_to_read;
    bytes_available -= bytes_to_read;
  }

  // Need more payload bytes than are now available.
  if (gPacketStat.payload_bytes_remain > 0) {
    return;
  }

  // We have a complete payload. Now grab the footer.
  if (!gPacketStat.have_packet)
  {
    if (bytes_available < FOOTER_LEN) {
      return;
    }
    readSerialBytes(serial_buf, FOOTER_LEN, 0);

    // Check CRC

    // Check trailer
    if (strncmp((serial_buf + 2), TRAILER, FOOTER_TRAILER_LEN)) {
      goto out_reset;
    }
    gPacketStat.have_packet = true;
  }

  // Done!
  return;

out_reset:
  resetInputPacket();
}

void handleInputPacket()
{
  if (!gPacketStat.have_packet) {
    return;
  }

  // Process the input packet.
  switch(gInputPacket.GetType())
  {
    case PBM_PING:
      writeHelloPacket();
      break;
    case PBM_GET_CFG:
      writeConfigPacket();
      break;
    case PBM_SET_CFG:
      readConfigPacket();
      break;
  }
  resetInputPacket();
}

/**
 * Main loop
 */
void loop()
{
  readIncomingSerialData();
  handleInputPacket();
  gTimer.update();
}
