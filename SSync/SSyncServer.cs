using System;
using System.Net;
using System.Net.Sockets;

namespace SSync {
    public class SSyncServer {
        public delegate void OnSocketAcceptedDel(Socket socket);

        public delegate void OnServerFailedToStartDel(Exception ex);

        /// <summary>
        /// Called when a client connect to the server on the EndPoint
        /// </summary>
        public event OnSocketAcceptedDel OnClientConnected;

        /// <summary>
        /// Called when server failed to start on the EndPoint
        /// </summary>
        public event OnServerFailedToStartDel OnServerFailedToStart;

        /// <summary>
        /// Called When server is started
        /// </summary>
        public event Action OnServerStarted;

        private Socket ListenSocket { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public SSyncServer(string ip, int port) {
            this.EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        void OnListenSucces() {
            this.OnServerStarted?.Invoke();
        }

        /// <summary>
        /// Try to start the server and call OnListenFailed() its not possible
        /// </summary>
        public void Start() {
            try {
                this.ListenSocket.Bind(this.EndPoint);
            }
            catch (Exception ex) {
                this.OnListenFailed(ex);

                return;
            }

            this.ListenSocket.Listen(100);
            this.StartAccept(null);
            this.OnListenSucces();
        }

        void OnListenFailed(Exception ex) {
            this.OnServerFailedToStart?.Invoke(ex);
        }

        protected void StartAccept(SocketAsyncEventArgs args) {
            if (args == null) {
                args = new SocketAsyncEventArgs();
                args.Completed += this.AcceptEventCompleted;
            }
            else {
                args.AcceptSocket = null;
            }

            bool willRaiseEvent = this.ListenSocket.AcceptAsync(args);
            if (!willRaiseEvent) {
                this.ProcessAccept(args);
            }
        }

        private void AcceptEventCompleted(object sender, SocketAsyncEventArgs e) {
            this.ProcessAccept(e);
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop() {
            this.ListenSocket.Shutdown(SocketShutdown.Both);
        }

        void ProcessAccept(SocketAsyncEventArgs args) {
            this.OnClientConnected?.Invoke(args.AcceptSocket);
            this.StartAccept(args);
        }
    }
}