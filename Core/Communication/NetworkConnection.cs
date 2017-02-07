using Core.Data;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Core.Communication
{
    public class NetworkConnection : IDisposable
    {
        public delegate void PacketManager(Packet p);

        private Socket _socket;

        public bool IsConnected { get; private set; }
        public string ConnectionId { get; set; }
        public DateTime LastExchangeTime { get; private set; }

        public NetworkConnection(Socket socket)
        {
            _socket = socket;

            ConnectionId = "-1";
            IsConnected = true;
            LastExchangeTime = DateTime.Now;
        }

        public void StartListening(PacketManager manager)
        {
            if (IsConnected == true)
            {
                Thread listening = new Thread(Listen);
                listening.Start(manager);
            }
            else
            {
                throw new InvalidOperationException("Socket must be connected before starting to listen.");
            }
        }

        private void Listen(object packetManager)
        {
            PacketManager manager = (PacketManager)packetManager;
            byte[] buffer;
            int readBytes;

            while (IsConnected)
            {
                try
                {
                    buffer = new byte[_socket.SendBufferSize];
                    readBytes = _socket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        Packet p = new Packet(buffer);
                        manager(p);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Connection lost: " + e.Message);

                    IsConnected = false;
                }
            }
        }

        protected Packet CreatePacket(PacketType type)
        {
            return new Packet(type, ConnectionId);
        }

        protected void SendPacket(Packet packet)
        {
            _socket.Send(packet.ToBytes());
            LastExchangeTime = DateTime.Now;
        }

        public void SendMessage(string senderName, string msg)
        {
            var packet = CreatePacket(PacketType.Message);
            packet.data.Add(senderName);
            packet.data.Add(msg);

            SendPacket(packet);
        }

        public void SendQuit(string clientId)
        {
            var packet = CreatePacket(PacketType.Quit);
            packet.data.Add(clientId);

            SendPacket(packet);
        }

        public void SendRegistration(string username)
        {
            var packet = CreatePacket(PacketType.Registration);
            packet.data.Add(username);

            SendPacket(packet);
        }

        public void Disconnect()
        {
            try
            {
                SendQuit(ConnectionId);
                _socket.Close();
            } catch (SocketException e) {
                Console.WriteLine("Connection was already broken: " + e.Message);
            }
            finally
            {
                IsConnected = false;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
