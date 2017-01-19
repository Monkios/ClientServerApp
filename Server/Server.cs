using Core.Communication;
using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class Server
    {
        static Socket _listenerSocket;
        static List<NetworkConnection> _clients;

        static int lastConnectionId;

        static Area map;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            map = new Area();
            map.gameObjects.Add(new BasicObject(0, 0, Direction.RIGHT));

            lastConnectionId = 0;
            try
            {
                _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listenerSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65535));
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was thrown when trying to launch the server: " + e.Message);
            }

            _clients = new List<NetworkConnection>();
            new Thread(ConnectClients).Start();
        }

        private static void ConnectClients()
        {
            Console.WriteLine("Ready to connect clients.");

            NetworkConnection client;
            while (true)
            {
                _listenerSocket.Listen(0);

                Console.WriteLine("Waiting...");
                client = new NetworkConnection(_listenerSocket.Accept());
                client.ConnectionId = (lastConnectionId++).ToString();
                client.StartListening(ManagePacket);

                Console.WriteLine("New client connected, sending registration packet.");
                client.SendRegistration("server");

                _clients.Add(client);
            }
        }

        private static NetworkConnection GetClient(string clientId)
        {
            return (from client in _clients
                    where client.ConnectionId == clientId
                    select client).FirstOrDefault(); ;
        }

        private static void RemoveClient(string clientId)
        {
            NetworkConnection removedClient = GetClient(clientId);
            removedClient.Disconnect();
            _clients.Remove(removedClient);

            BroadcastQuit(clientId);
        }

        private static void ManagePacket(Packet packet)
        {
            switch (packet.type)
            {
                case PacketType.Registration:
                    Console.WriteLine("User " + packet.data[0] + " is connected.");

                    // Send the new connection to all clients
                    BroadcastWelcome(packet.data[0]);
                    // Send the map to the new client
                    PushMap(packet.senderId);
                    break;
                case PacketType.Message:
                    Console.WriteLine(packet.senderId + "-" + packet.data[0] + " says : " + packet.data[1]);

                    // Transfer the message back to all other clients
                    BroadcastMessage(packet.data[0], packet.data[1]);
                    break;
                case PacketType.Quit:
                    // A client is telling the server before quitting
                    Console.WriteLine("User " + packet.data[0] + " has disconnected.");

                    RemoveClient(packet.data[0]);
                    break;
                case PacketType.Map:
                    // The client wants a clean copy of the map
                    PushMap(packet.senderId);
                    break;
                case PacketType.Welcome:
                default:
                    Console.WriteLine("Invalid packet type: " + packet.type.ToString());
                    break;
            }
        }

        private static void PushMap(string clientId)
        {
            GetClient(clientId).SendMap(map.ToJSON());
        }

        private static void BroadcastWelcome(string username)
        {
            foreach (NetworkConnection client in _clients)
            {
                client.SendWelcome(username);
            }
        }

        private static void BroadcastQuit(string clientId)
        {
            foreach (NetworkConnection client in _clients)
            {
                client.SendQuit(clientId);
            }
        }

        private static void BroadcastMessage(string msg, string senderName)
        {
            foreach (NetworkConnection client in _clients)
            {
                client.SendMessage(senderName, msg);
            }
        }
    }
}