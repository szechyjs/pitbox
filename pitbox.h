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
#define FIRMWARE_VERSION 1

// Packet settings
#define PREFIX "PB v1:"
#define PREFIX_CRC 0xe3af
#define TRAILER "\r\n"

#define HEADER_LEN 10
#define HEADER_PREFIX_LEN 6
#define HEADER_ID_LEN 2
#define HEADER_PAYLOADLEN_LEN 2

#define FOOTER_LEN 4
#define FOOTER_CRC_LEN 2
#define FOOTER_TRAILER_LEN 2

#define PAYLOAD_MAXLEN 112

// Packets
#define PBM_HELLO_ID    0x01
#define PBM_HELLO_TAG_PROTOCOL_VERSION  0x01
#define PBM_HELLO_TAG_FIRMWARE_VERSION  0x01
