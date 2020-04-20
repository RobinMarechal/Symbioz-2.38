using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz.Core;
using Symbioz.Core.DesignPattern;
using System.Net.Sockets;
using Symbioz.Core.DesignPattern.StartupEngine;
using Symbioz.Auth;
using SSync;
using SSync.Messages;

namespace Symbioz.Network.Servers {
    public class AuthServer : Singleton<AuthServer> {
        static Logger logger = new Logger();

        private List<AuthClient> m_clients = new List<AuthClient>();

        public SSyncServer Server {
            get;
            private
            set;
        }

        public int ClientsCount {
            get { return this.m_clients.Count; }
        }

        public AuthServer() {
            this.Server = new SSyncServer(AuthConfiguration.Instance.Host, AuthConfiguration.Instance.Port);
            this.Server.OnServerStarted += this.Server_OnServerStarted;
            this.Server.OnServerFailedToStart += this.Server_OnServerFailedToStart;
            this.Server.OnClientConnected += this.Server_OnClientConnected;
        }

        void Server_OnClientConnected(Socket socket) {
            logger.White("New client connected");
            new AuthClient(socket);
        }

        void Server_OnServerFailedToStart(Exception ex) {
            logger.Alert("Unable to start AuthServer " + ex);
        }

        void Server_OnServerStarted() {
            logger.Gray("Server Started (" + this.Server.EndPoint.ToString() + ")");
        }

        public void Start() {
            this.Server.Start();
        }

        public void Send(Message message) {
            foreach (var client in this.m_clients) {
                client.Send(message);
            }
        }

        public void AddClient(AuthClient client) {
            this.m_clients.Add(client);
        }

        public void RemoveClient(AuthClient client) {
            this.m_clients.Remove(client);
            logger.White("Client disconnected");
        }

        public List<AuthClient> GetClients() {
            return this.m_clients;
        }
    }
}