using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ReloginTokenStatusMessage : Message {
        public const ushort Id = 6539;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool validToken;
        public sbyte[] ticket;


        public ReloginTokenStatusMessage() { }

        public ReloginTokenStatusMessage(bool validToken, sbyte[] ticket) {
            this.validToken = validToken;
            this.ticket = ticket;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.validToken);
            writer.WriteUShort((ushort) this.ticket.Length);
            foreach (var entry in this.ticket) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.validToken = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.ticket = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.ticket[i] = reader.ReadSByte();
            }
        }
    }
}