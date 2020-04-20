using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountEmoteIconUsedOkMessage : Message {
        public const ushort Id = 5978;

        public override ushort MessageId {
            get { return Id; }
        }

        public int mountId;
        public sbyte reactionType;


        public MountEmoteIconUsedOkMessage() { }

        public MountEmoteIconUsedOkMessage(int mountId, sbyte reactionType) {
            this.mountId = mountId;
            this.reactionType = reactionType;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarInt(this.mountId);
            writer.WriteSByte(this.reactionType);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mountId = reader.ReadVarInt();
            this.reactionType = reader.ReadSByte();

            if (this.reactionType < 0)
                throw new Exception("Forbidden value on reactionType = " + this.reactionType + ", it doesn't respect the following condition : reactionType < 0");
        }
    }
}