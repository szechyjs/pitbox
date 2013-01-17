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
#define PREFIX "PBSP v1:"
#define PREFIX_CRC 0xe3af
#define TRAILER "\r\n"

#define HEADER_LEN 12
#define HEADER_PREFIX_LEN 8
#define HEADER_ID_LEN 2
#define HEADER_PAYLOADLEN_LEN 2

#define FOOTER_LEN 4
#define FOOTER_CRC_LEN 2
#define FOOTER_TRAILER_LEN 2

#define PAYLOAD_MAXLEN 112

#define PBM_PING 0x81
#define PBM_GET_CFG  0x10

// Packets
#define PBM_CFG  0x20
#define PBM_CFG_TAG_RED 0x01
#define PBM_CFG_TAG_GRN 0x02
#define PBM_CFG_TAG_DLY 0x03

#define PBM_HELLO_ID    0x01
#define PBM_HELLO_TAG_PROTOCOL_VERSION  0x01
#define PBM_HELLO_TAG_FIRMWARE_VERSION  0x01
