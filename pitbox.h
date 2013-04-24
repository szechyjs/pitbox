// To enable serial debugging, uncomment the following line
//#define VERBOSE

// Pin Configuration
#define PIN_STATUS_LED  17
#define PIN_RED_LEDS    6
#define PIN_GREEN_LEDS  5
#define PIN_TRIGGER     4

// Configuration settings
#define CONFIG_VERSION "v2"
#define CONFIG_START 32
#define FIRMWARE_VERSION 3

// Packet settings
#define PREFIX "PBSP v1:"
#define PREFIX_CRC 0x7621
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
#define PBM_SET_CFG  0x20

// Packets
#define PBM_CFG  0x20
#define PBM_CFG_TAG_RED 0x01
#define PBM_CFG_TAG_GRN 0x02
#define PBM_CFG_TAG_DLY 0x03
#define PBM_CFG_TAG_RED_MODE 0x04
#define PBM_CFG_TAG_GRN_MODE 0x05

#define PBM_HELLO_ID    0x01
#define PBM_HELLO_TAG_PROTOCOL_VERSION  0x01
#define PBM_HELLO_TAG_FIRMWARE_VERSION  0x01
