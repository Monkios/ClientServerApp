using Core.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication
{
    class ServerNetworkConnection : NetworkConnection
    {
        public ServerNetworkConnection(Socket socket) : base(socket) { }

        public void SendNameDenied(string deniedName)
        {
            var packet = CreatePacket(PacketType.NameDenied);
            packet.data.Add(deniedName);

            SendPacket(packet);
        }

        public void SendWelcome(string clientId, string username)
        {
            var packet = CreatePacket(PacketType.Welcome);
            packet.data.Add(clientId);
            packet.data.Add(username);

            SendPacket(packet);
        }

        public void SendMap(string map)
        {
            var packet = CreatePacket(PacketType.Map);
            packet.data.Add(map);

            SendPacket(packet);
        }

        public void SendTick(int tickId)
        {
            var packet = CreatePacket(PacketType.Tick);
            packet.data.Add(tickId.ToString());

            SendPacket(packet);
        }
    }
}
