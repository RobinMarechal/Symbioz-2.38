using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NpcGenericActionRequestMessage : Message {
        public const ushort Id = 5898;

        public override ushort MessageId {
            get { return Id; }
        }

        public int npcId;
        public sbyte npcActionId;
        public int npcMapId;


        public NpcGenericActionRequestMessage() { }

        public NpcGenericActionRequestMessage(int npcId, sbyte npcActionId, int npcMapId) {
            this.npcId = npcId;
            this.npcActionId = npcActionId;
            this.npcMapId = npcMapId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.npcId);
            writer.WriteSByte(this.npcActionId);
            writer.WriteInt(this.npcMapId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.npcId = reader.ReadInt();
            this.npcActionId = reader.ReadSByte();

            if (this.npcActionId < 0)
                throw new Exception("Forbidden value on npcActionId = " + this.npcActionId + ", it doesn't respect the following condition : npcActionId < 0");
            this.npcMapId = reader.ReadInt();
        }
    }
}