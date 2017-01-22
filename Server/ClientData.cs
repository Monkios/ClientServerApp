using Core.Communication;
using Core.Data;
using Core.GameObjects;
using System.Net.Sockets;

namespace Server
{
    // A client should be a composition of a network connection, some player data and the hero entity.
    public class ClientData
    {
        public NetworkConnection connection;
        public string Name { get { return _character.Name; } }

        private Hero _character;

        public ClientData(Socket clientSocket)
        {
            connection = new NetworkConnection(clientSocket);
        }

        public ClientData(Socket clientSocket, Hero existingCharacter)
        {
            connection = new NetworkConnection(clientSocket);
            _character = existingCharacter;
        }

        public void SetHero(Hero hero)
        {
            _character = hero;
        }

        public void Disconnect()
        {
            connection.Disconnect();
        }
    }
}
