using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LivingObjectMessageMessage : Message {
        public const ushort Id = 6065;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort msgId;
        public int timeStamp;
        public string owner;
        public ushort objectGenericId;


        public LivingObjectMessageMessage() { }

        public LivingObjectMessageMessage(ushort msgId, int timeStamp, string owner, ushort objectGenericId) {
            this.msgId = msgId;
            this.timeStamp = timeStamp;
            this.owner = owner;
            this.objectGenericId = objectGenericId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.msgId);
            writer.WriteInt(this.timeStamp);
            writer.WriteUTF(this.owner);
            writer.WriteVarUhShort(this.objectGenericId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.msgId = reader.ReadVarUhShort();

            if (this.msgId < 0)
                throw new Exception("Forbidden value on msgId = " + this.msgId + ", it doesn't respect the following condition : msgId < 0");
            this.timeStamp = reader.ReadInt();

            if (this.timeStamp < 0)
                throw new Exception("Forbidden value on timeStamp = " + this.timeStamp + ", it doesn't respect the following condition : timeStamp < 0");
            this.owner = reader.ReadUTF();
            this.objectGenericId = reader.ReadVarUhShort();

            if (this.objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + this.objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
        }
    }
}