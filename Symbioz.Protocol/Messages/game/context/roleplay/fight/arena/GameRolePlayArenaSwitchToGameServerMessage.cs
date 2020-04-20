using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaSwitchToGameServerMessage : Message {
        public const ushort Id = 6574;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool validToken;
        public sbyte[] ticket;
        public short homeServerId;


        public GameRolePlayArenaSwitchToGameServerMessage() { }

        public GameRolePlayArenaSwitchToGameServerMessage(bool validToken, sbyte[] ticket, short homeServerId) {
            this.validToken = validToken;
            this.ticket = ticket;
            this.homeServerId = homeServerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.validToken);
            writer.WriteUShort((ushort) this.ticket.Length);
            foreach (var entry in this.ticket) {
                writer.WriteSByte(entry);
            }

            writer.WriteShort(this.homeServerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.validToken = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.ticket = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.ticket[i] = reader.ReadSByte();
            }

            this.homeServerId = reader.ReadShort();
        }
    }
}