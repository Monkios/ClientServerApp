using Core.Communication;
using System;
using System.Net;
using System.Net.Sockets;

namespace ConsoleClient
{
    class Client
    {
        static string _name;
        static NetworkConnection _server;

        static void Main(string[] args)
        {
            _name = AskForUserName();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65535));

                _server = new NetworkConnection(socket);
                _server.StartListening(ManagePacket);

                OpenChatBox();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was thrown when trying to connect: " + e.Message);
            }
        }

        private static string AskForUserName()
        {
            string name = "";
            do
            {
                Console.Clear();

                Console.Write("Please, enter your name: ");
                name = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(name));

            return name;
        }

        private static void ManagePacket(Packet p)
        {
            switch (p.type)
            {
                case PacketType.Registration:
                    // Client must validate its connection
                    _server.ConnectionId = p.senderId;
                    _server.SendRegistration(_name);
                    break;
                case PacketType.Welcome:
                    // A new client connected to the server
                    if (p.data[0] != _server.ConnectionId)
                    {
                        Console.WriteLine("New player " + p.data[1] + " connected.");
                    }
                    break;
                case PacketType.Quit:
                    // A client disconnected from the server
                    if (p.data[0] != _name)
                    {
                        Console.WriteLine(p.data[0] + " has disconnected.");
                    }
                    break;
                case PacketType.Message:
                    // A client sent a message
                    Console.WriteLine(p.data[0] + ": " + p.data[1]);
                    break;
                case PacketType.Map:
                    // The server sent a complete refresh of the map
                    Console.WriteLine(p.data[0]);
                    break;
            }
        }

        private static void OpenChatBox()
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "exit":
                        _server.SendQuit(_server.ConnectionId);
                        _server.Disconnect();
                        break;
                    default:
                        _server.SendMessage(_name, input);
                        break;
                }
            }
        }
    }
}
