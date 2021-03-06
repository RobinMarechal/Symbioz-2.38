using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DungeonKeyRingUpdateMessage : Message {
        public const ushort Id = 6296;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort dungeonId;
        public bool available;


        public DungeonKeyRingUpdateMessage() { }

        public DungeonKeyRingUpdateMessage(ushort dungeonId, bool available) {
            this.dungeonId = dungeonId;
            this.available = available;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.dungeonId);
            writer.WriteBoolean(this.available);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.dungeonId = reader.ReadVarUhShort();

            if (this.dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + this.dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            this.available = reader.ReadBoolean();
        }
    }
}