using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Core.Communication
{
    [Serializable]
    public class Packet
    {
        public PacketType type;
        public string senderId;
        public List<string> data;

        public Packet(PacketType type, string senderId)
        {
            this.type = type;
            this.senderId = senderId;
            this.data = new List<string>();
        }

        public Packet(byte[] serializedPacket)
        {
            MemoryStream ms = new MemoryStream(serializedPacket);
            BinaryFormatter bf = new BinaryFormatter();
            Packet p = (Packet)bf.Deserialize(ms);
            ms.Close();

            this.data = p.data;
            this.senderId = p.senderId;
            this.type = p.type;
        }

        public byte[] ToBytes()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);

            byte[] serializedPacket = ms.ToArray();
            ms.Close();

            return serializedPacket;
        }
    }
}
