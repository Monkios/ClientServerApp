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

        public NetworkConnection(Socket socket)
        {
            _socket = socket;

            ConnectionId = "-1";
            IsConnected = true;
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

        public void SendRegistration(string from)
        {
            Packet packet = new Packet(PacketType.Registration, ConnectionId);
            packet.data.Add(from);

            _socket.Send(packet.ToBytes());
        }

        public void SendNameDenied(string deniedName)
        {
            Packet packet = new Packet(PacketType.NameDenied, ConnectionId);
            packet.data.Add(deniedName);

            _socket.Send(packet.ToBytes());
        }

        public void SendWelcome(string clientId, string username)
        {
            Packet packet = new Packet(PacketType.Welcome, ConnectionId);
            packet.data.Add(clientId);
            packet.data.Add(username);

            _socket.Send(packet.ToBytes());
        }

        public void SendQuit(string clientId)
        {
            Packet packet = new Packet(PacketType.Quit, ConnectionId);
            packet.data.Add(clientId);

            _socket.Send(packet.ToBytes());
        }

        public void SendMessage(string from, string msg)
        {
            Packet p = new Packet(PacketType.Message, ConnectionId);
            p.data.Add(from);
            p.data.Add(msg);

            _socket.Send(p.ToBytes());
        }

        public void SendMap(string map)
        {
            Packet p = new Packet(PacketType.Map, ConnectionId);
            p.data.Add(map);

            _socket.Send(p.ToBytes());
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
