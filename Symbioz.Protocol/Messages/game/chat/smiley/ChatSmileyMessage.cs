using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChatSmileyMessage : Message {
        public const ushort Id = 801;

        public override ushort MessageId {
            get { return Id; }
        }

        public double entityId;
        public ushort smileyId;
        public int accountId;


        public ChatSmileyMessage() { }

        public ChatSmileyMessage(double entityId, ushort smileyId, int accountId) {
            this.entityId = entityId;
            this.smileyId = smileyId;
            this.accountId = accountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.entityId);
            writer.WriteVarUhShort(this.smileyId);
            writer.WriteInt(this.accountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.entityId = reader.ReadDouble();

            if (this.entityId < -9007199254740990 || this.entityId > 9007199254740990)
                throw new Exception("Forbidden value on entityId = " + this.entityId + ", it doesn't respect the following condition : entityId < -9007199254740990 || entityId > 9007199254740990");
            this.smileyId = reader.ReadVarUhShort();

            if (this.smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + this.smileyId + ", it doesn't respect the following condition : smileyId < 0");
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
        }
    }
}