#include <inttypes.h>

class Packet {
  public:
    Packet();
    void SetType(int type) {m_type = type;}
    int GetType() {return m_type;}
    void AddTag(uint8_t tag, uint8_t buflen, char *buf);

    bool ReadTag(uint8_t tagnum, uint16_t *value);
    bool ReadTag(uint8_t tagnum, uint16_t **value);

    uint8_t* FindTag(uint8_t tagnum);
    void AppendBytes(char *buf, int buflen);
    void Reset();
    bool IsReset();
    void Print();
    uint16_t GenCrc();

  private:
    int m_type;
    uint8_t m_len;
    uint8_t m_payload[PAYLOAD_MAXLEN];
};
