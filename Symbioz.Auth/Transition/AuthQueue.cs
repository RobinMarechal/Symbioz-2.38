using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Core.DesignPattern;
using Symbioz.Network;
using Symbioz.Core;
using Symbioz.Protocol.Messages;
using Symbioz.Auth.Handlers;
using Symbioz.Protocol.Enums;

namespace Symbioz.Auth.Transition {
    /// <summary>
    /// From Stump
    /// </summary>
    public class AuthQueue : Singleton<AuthQueue> {
        public static int QueueDelay = 3000;

        private ConcurrentQueue<AuthClient> m_clients = new ConcurrentQueue<AuthClient>();
        private Logger logger = new Logger();
        private bool m_running;

        public bool IsInQueue(AuthClient client) {
            return this.m_clients.Contains(client);
        }

        public void AddClient(AuthClient client) {
            this.m_clients.Enqueue(client);
            if (!this.m_running) {
                this.m_running = true;
                this.Authentificate();
                Task.Factory.StartNewDelayed(QueueDelay,
                                             () => {
                                                 try {
                                                     this.Authentificate();
                                                 }
                                                 catch (Exception ex) {
                                                     this.logger.Alert(string.Format("errror while Authentificate some clients\n{1}", ex.Message));
                                                 }
                                             });
            }
        }

        private void Authentificate() {
            AuthClient client;

            if (!this.m_running || !this.m_clients.TryDequeue(out client)) {
                return;
            }

            while (client == null || (client != null && !client.IsConnected)) {
                if (this.m_clients.Count == 0) {
                    break;
                }

                this.m_clients.TryDequeue(out client);
                if (client != null && !client.IsConnected) {
                    client = null;
                }
            }

            if (client == null || client.IdentificationMessage == null) {
                if (this.m_clients.Count == 0) {
                    this.m_running = false;
                }

                if (this.m_running) {
                    Task.Factory.StartNewDelayed(QueueDelay, this.Authentificate);
                }
            }

            Task.Factory.StartNew(this.RefreshQueue);

            try {
                this.Indentificate(client);
            }
            catch (Exception ex) {
                this.logger.Alert(string.Format("errror while Indentificate {0} :\n{1}", client, ex.ToString()));
            }

            if (this.m_clients.Count == 0) {
                this.m_running = false;
            }

            if (this.m_running) {
                Task.Factory.StartNewDelayed(QueueDelay, this.Authentificate);
            }
        }

        public void Indentificate(AuthClient client) {
            try {
                ConnectionHandler.ProcessIdentification(client);
            }
            catch (Exception ex) {
                client.Send(new IdentificationFailedMessage((sbyte) (IdentificationFailureReasonEnum.SERVICE_UNAVAILABLE)));
                this.logger.Alert(ex.ToString());
            }
        }

        private void RefreshQueue() {
            foreach (var client in this.m_clients) {
                client.Send(new LoginQueueStatusMessage(this.GetPosition(client), this.GetCount()));
            }
        }

        private ushort GetPosition(AuthClient client) {
            return (ushort) this.m_clients.ToList().IndexOf(client);
        }

        private ushort GetCount() {
            return (ushort) this.m_clients.Count;
        }
    }
}