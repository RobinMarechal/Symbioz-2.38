using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SelectedServerDataMessage : Message {
        public const ushort Id = 42;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort serverId;
        public string address;
        public ushort port;
        public bool canCreateNewCharacter;
        public sbyte[] ticket;


        public SelectedServerDataMessage() { }

        public SelectedServerDataMessage(ushort serverId, string address, ushort port, bool canCreateNewCharacter, sbyte[] ticket) {
            this.serverId = serverId;
            this.address = address;
            this.port = port;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.ticket = ticket;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.serverId);
            writer.WriteUTF(this.address);
            writer.WriteUShort(this.port);
            writer.WriteBoolean(this.canCreateNewCharacter);
            writer.WriteVarUhShort((ushort) this.ticket.Count());
            foreach (var entry in this.ticket) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.serverId = reader.ReadVarUhShort();

            if (this.serverId < 0)
                throw new Exception("Forbidden value on serverId = " + this.serverId + ", it doesn't respect the following condition : serverId < 0");
            this.address = reader.ReadUTF();
            this.port = reader.ReadUShort();

            if (this.port < 0 || this.port > 65535)
                throw new Exception("Forbidden value on port = " + this.port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            this.canCreateNewCharacter = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.ticket = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.ticket[i] = reader.ReadSByte();
            }
        }
    }
}