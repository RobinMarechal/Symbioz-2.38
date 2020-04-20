using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaSwitchToFightServerMessage : Message {
        public const ushort Id = 6575;

        public override ushort MessageId {
            get { return Id; }
        }

        public string address;
        public ushort port;
        public sbyte[] ticket;


        public GameRolePlayArenaSwitchToFightServerMessage() { }

        public GameRolePlayArenaSwitchToFightServerMessage(string address, ushort port, sbyte[] ticket) {
            this.address = address;
            this.port = port;
            this.ticket = ticket;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.address);
            writer.WriteUShort(this.port);
            writer.WriteUShort((ushort) this.ticket.Length);
            foreach (var entry in this.ticket) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.address = reader.ReadUTF();
            this.port = reader.ReadUShort();

            if (this.port < 0 || this.port > 65535)
                throw new Exception("Forbidden value on port = " + this.port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            var limit = reader.ReadUShort();
            this.ticket = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.ticket[i] = reader.ReadSByte();
            }
        }
    }
}