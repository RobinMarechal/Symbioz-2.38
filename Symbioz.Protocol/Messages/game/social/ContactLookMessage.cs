using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ContactLookMessage : Message {
        public const ushort Id = 5934;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint requestId;
        public string playerName;
        public ulong playerId;
        public EntityLook look;


        public ContactLookMessage() { }

        public ContactLookMessage(uint requestId, string playerName, ulong playerId, EntityLook look) {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.requestId);
            writer.WriteUTF(this.playerName);
            writer.WriteVarUhLong(this.playerId);
            this.look.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requestId = reader.ReadVarUhInt();

            if (this.requestId < 0)
                throw new Exception("Forbidden value on requestId = " + this.requestId + ", it doesn't respect the following condition : requestId < 0");
            this.playerName = reader.ReadUTF();
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.look = new EntityLook();
            this.look.Deserialize(reader);
        }
    }
}