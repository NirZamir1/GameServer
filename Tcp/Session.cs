using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace Server
{
    public class Session
    {
        public delegate void lobbyHandler(IEnumerable loggedClients);
        public event lobbyHandler updateOnChange;
        private int _playerCount;
        private int lobbySize;
        Dictionary<string, client> _clients;
        public State gameState { get; }
        public Session(client _client, int lobbySize)
        {
            this.lobbySize = lobbySize;
            _playerCount = 1;
            _clients = new Dictionary<string, client>(lobbySize);
            _clients.Add(_client.name, _client);
        }
        IEnumerable join(client client)
        {
            if (_playerCount > lobbySize)
            {
                throw new Exception("Too many players no way you have so many friends. Fuck off you little cunt");
            }
            var ret = new Dictionary<string, client>(_clients);
            _clients.Add(client.name, client);
            updateOnChange?.Invoke(_clients);
            return ret;
        }
        void Leave(client leaver)
        {
            _clients.Remove(leaver.name);
            updateOnChange?.Invoke(_clients);
        }
    }
    public struct client
    {
        public string name;
        public IPEndPoint ip;
    }
    public enum State
    {
        Idle = 0,
        Running = 1
    }
}
