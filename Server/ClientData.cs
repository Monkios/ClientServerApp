using Core.Communication;
using Core.Data;
using System.Net.Sockets;

namespace Server
{
    public class ClientData
    {
        public NetworkConnection connection;
        public string Name { get { return _player.Name; } }

        private Player _player;

        public ClientData(Socket clientSocket)
        {
            connection = new NetworkConnection(clientSocket);
        }

        public ClientData(Socket clientSocket, Player existingPlayer)
        {
            connection = new NetworkConnection(clientSocket);
            _player = existingPlayer;
        }

        public void CreatePlayer(string name)
        {
            _player = new Player(name);
        }

        public void Disconnect()
        {
            connection.Disconnect();
        }
    }
}
