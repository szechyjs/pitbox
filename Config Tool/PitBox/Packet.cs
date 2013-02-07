using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBox
{
    public class Packet
    {
        private UInt16 mLength;
        public UInt16 Type { get; set; }
        private List<byte> mPayload;

        public Packet()
        {
            mPayload = new List<byte>();
        }

        public void AddTag(byte tagnum, UInt16 value)
        {
            var data = BitConverter.GetBytes(value);
            var length = BitConverter.GetBytes(data.Length);

            mPayload.Add(tagnum);
            mLength++;

            mPayload.Add(length[0]);
            mLength++;

            foreach (byte b in data)
            {
                mPayload.Add(b);
                mLength++;
            }
        }

        public UInt16 GetTag(byte tagnum)
        {
            int pos = 0;

            while (pos < mLength && pos < 112)
            {
                byte tag = mPayload[pos];
                if (tag == tagnum)
                {
                    return BitConverter.ToUInt16(mPayload.ToArray(), (pos + 2));
                }
                pos += 2 + mPayload[pos + 1];
            }
            return 0;
        }

        public byte[] GetPacket()
        {
            var encoding = new ASCIIEncoding();
            var packet = new List<byte>();
            var payload = new List<byte>();

            
            var prefix = encoding.GetBytes("PBSP v1:");
            var footer = encoding.GetBytes("\r\n");

            payload.AddRange(BitConverter.GetBytes(Type));
            payload.AddRange(BitConverter.GetBytes(mLength));
            payload.AddRange(mPayload);

            Crc16Ccitt crc = new Crc16Ccitt(InitialCrcValue.PitBox);
            var crcBytes = crc.ComputeChecksumBytes(payload.ToArray());

            packet.AddRange(prefix);
            packet.AddRange(payload);
            packet.AddRange(crcBytes);
            packet.AddRange(footer);

            return packet.ToArray();
        }

        public void AppendByte(byte[] buf, int buflen)
        {
            var list = buf.ToList();
            mPayload.AddRange(list);
            mLength += (UInt16)buflen;
        }
    }
}
