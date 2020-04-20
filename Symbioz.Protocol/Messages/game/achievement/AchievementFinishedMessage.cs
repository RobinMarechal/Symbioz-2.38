using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AchievementFinishedMessage : Message {
        public const ushort Id = 6208;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort id;
        public byte finishedlevel;


        public AchievementFinishedMessage() { }

        public AchievementFinishedMessage(ushort id, byte finishedlevel) {
            this.id = id;
            this.finishedlevel = finishedlevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.id);
            writer.WriteByte(this.finishedlevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadVarUhShort();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
            this.finishedlevel = reader.ReadByte();

            if (this.finishedlevel < 0 || this.finishedlevel > 200)
                throw new Exception("Forbidden value on finishedlevel = " + this.finishedlevel + ", it doesn't respect the following condition : finishedlevel < 0 || finishedlevel > 200");
        }
    }
}