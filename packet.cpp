#include "pitbox.h"
#include "packet.h"

#include <avr/io.h>
#include <HardwareSerial.h>
#include <string.h>
#include <util/crc16.h>

static uint16_t crc_ccitt_update_int(uint16_t crc, int value) {
  crc = _crc_ccitt_update(crc, value & 0xff);
  return _crc_ccitt_update(crc, (value >> 8) & 0xff);
}

static void serial_print_int(int value) {
  Serial.write(value & 0xff);
  Serial.write((value >> 8) & 0xff);
}

Packet::Packet()
{
  Reset();
}

bool Packet::IsReset() {
  return (m_type == 0) && (m_len == 0);
}

void Packet::Reset()
{
  m_len = 0;
  m_type = 0;
}

void Packet::AddTag(uint8_t tag, uint8_t buflen, char *buf)
{
  int i=0;
  m_payload[m_len++] = tag;
  m_payload[m_len++] = buflen;
  AppendBytes(buf, buflen);
}

uint8_t* Packet::FindTag(uint8_t tagnum) {
  uint8_t pos=0;
  while (pos < m_len && pos < PAYLOAD_MAXLEN) {
    uint8_t tag = m_payload[pos];
    if (tag == tagnum) {
      return m_payload + pos;
    }
    pos += 2 + m_payload[pos+1];
  }
  return NULL;
}

bool Packet::ReadTag(uint8_t tagnum, uint16_t *value) {
  uint8_t *offptr = FindTag(tagnum);
  if (offptr == NULL) {
    return false;
  }
  *value = *reinterpret_cast<uint16_t*>(offptr + 2);
  return value;
}

bool Packet::ReadTag(uint8_t tagnum, uint16_t** value) {
  uint8_t *offptr = FindTag(tagnum);
  if (offptr == NULL) {
    return false;
  }
  uint8_t slen = *(offptr + 1);
  memcpy(*value, (offptr + 2), slen);
  return true;
}

void Packet::AppendBytes(char *buf, int buflen)
{
  int i=0;
  while (i < buflen && m_len < PAYLOAD_MAXLEN) {
    m_payload[m_len++] = (uint8_t) (*(buf+i));
    i++;
  }
}

uint16_t Packet::GenCrc()
{
  uint16_t crc = PREFIX_CRC;

  crc = crc_ccitt_update_int(crc, m_type);
  crc = crc_ccitt_update_int(crc, m_len);

  for (int i=0; i<m_len; i++) {
    crc = _crc_ccitt_update(crc, m_payload[i]);
  }

  return crc;
}

void Packet::Print()
{
  int i;
  uint16_t crc = PREFIX_CRC;

  //header
  //header: prefix
  Serial.print(PREFIX);

  //header: message_id
  serial_print_int(m_type);
  crc = crc_ccitt_update_int(crc, m_type);

  //header: payload_len
  serial_print_int(m_len);
  crc = crc_ccitt_update_int(crc, m_len);

  //payload
  for (i=0; i<m_len; i++) {
    Serial.write(m_payload[i]);
    crc = _crc_ccitt_update(crc, m_payload[i]);
  }

  //trailer
  serial_print_int(crc);
  Serial.write('\r');
  Serial.write('\n');
}
