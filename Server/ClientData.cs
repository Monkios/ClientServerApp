using Core.Communication;
using Core.Data;
using Core.GameObjects;
using System.Net.Sockets;

namespace Server
{
    // A client should be a composition of a network connection, some player data and the hero entity.
    public class ClientData
    {
        public string Name { get { return _character.Name; } }
        public string ConnectionId { get { return _connection.ConnectionId; } }

        private NetworkConnection _connection;
        private Hero _character;

        public ClientData(Socket clientSocket, string connectionId, NetworkConnection.PacketManager packetManager)
        {
            _connection = new NetworkConnection(clientSocket);
            _connection.ConnectionId = connectionId;

            _connection.StartListening(packetManager);

            _connection.SendRegistration("server");
        }

        public ClientData(Socket clientSocket, Hero existingCharacter)
        {
            _connection = new NetworkConnection(clientSocket);
            _character = existingCharacter;
        }

        public void SetHero(Hero hero)
        {
            _character = hero;
        }

        public void Disconnect()
        {
            _connection.Disconnect();
        }

        public void SendMap(string map){
            _connection.SendMap(map);
        }

        public void SendNameDenied(string deniedName){
            _connection.SendNameDenied(deniedName);
        }

        public void SendWelcome(string clientId, string username){
            _connection.SendWelcome(clientId, username);
        }

        public void SendQuit(string clientId){
            _connection.SendQuit(clientId);
        }

        public void SendMessage(string senderName, string msg){
            _connection.SendMessage(senderName, msg);
        }
    }
}
