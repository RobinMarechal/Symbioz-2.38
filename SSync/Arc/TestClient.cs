using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SSync.Arc {
    public class TestClient : AbstractClient {
        public TestClient(Socket sock)
            : base(sock) {
            this.m_buffer = new byte[this.BufferLenght];
            this.Socket.BeginReceive(this.m_buffer, 0, this.BufferLenght, SocketFlags.None, this.OnReceived, null);
        }

        public TestClient() { }
        public override void OnClosed() { }

        public override void OnConnected() {
            throw new NotImplementedException();
        }

        public override void OnDataArrival(byte[] buffer) {
            throw new NotImplementedException();
        }

        public override void OnFailToConnect(Exception ex) {
            throw new NotImplementedException();
        }

        public override void Send(byte[] buffer) {
            this.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, this.OnSended, null);
        }

        public async void OnSended(IAsyncResult result) { }

        public override void Connect(string host, int port) {
            this.Socket.BeginConnect(this.EndPoint, new AsyncCallback(this.OnConnectedAsync), this.Socket);
        }

        byte[] m_buffer;
        public int BufferLenght = 8192;
        public void OnConnectedAsync(IAsyncResult result) { }

        public void OnReceived(IAsyncResult result) {
            this.Socket.EndReceive(result);
            //if (m_buffer[0] == 0)
            //{
            //    Disconnect();
            //    return;
            //}
            this.OnDataArrival(this.m_buffer);
            this.Socket.BeginReceive(this.m_buffer, 0, this.BufferLenght, SocketFlags.None, this.OnReceived, null);
        }

        public override void Disconnect() {
            this.Socket.Shutdown(SocketShutdown.Both);
            this.Socket.Close();
            this.OnClosed();
        }
    }
}