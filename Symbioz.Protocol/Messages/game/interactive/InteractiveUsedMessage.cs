using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InteractiveUsedMessage : Message {
        public const ushort Id = 5745;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong entityId;
        public uint elemId;
        public ushort skillId;
        public ushort duration;
        public bool canMove;


        public InteractiveUsedMessage() { }

        public InteractiveUsedMessage(ulong entityId, uint elemId, ushort skillId, ushort duration, bool canMove) {
            this.entityId = entityId;
            this.elemId = elemId;
            this.skillId = skillId;
            this.duration = duration;
            this.canMove = canMove;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.entityId);
            writer.WriteVarUhInt(this.elemId);
            writer.WriteVarUhShort(this.skillId);
            writer.WriteVarUhShort(this.duration);
            writer.WriteBoolean(this.canMove);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.entityId = reader.ReadVarUhLong();

            if (this.entityId < 0 || this.entityId > 9007199254740990)
                throw new Exception("Forbidden value on entityId = " + this.entityId + ", it doesn't respect the following condition : entityId < 0 || entityId > 9007199254740990");
            this.elemId = reader.ReadVarUhInt();

            if (this.elemId < 0)
                throw new Exception("Forbidden value on elemId = " + this.elemId + ", it doesn't respect the following condition : elemId < 0");
            this.skillId = reader.ReadVarUhShort();

            if (this.skillId < 0)
                throw new Exception("Forbidden value on skillId = " + this.skillId + ", it doesn't respect the following condition : skillId < 0");
            this.duration = reader.ReadVarUhShort();

            if (this.duration < 0)
                throw new Exception("Forbidden value on duration = " + this.duration + ", it doesn't respect the following condition : duration < 0");
            this.canMove = reader.ReadBoolean();
        }
    }
}