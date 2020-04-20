using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NpcDialogCreationMessage : Message {
        public const ushort Id = 5618;

        public override ushort MessageId {
            get { return Id; }
        }

        public int mapId;
        public int npcId;


        public NpcDialogCreationMessage() { }

        public NpcDialogCreationMessage(int mapId, int npcId) {
            this.mapId = mapId;
            this.npcId = npcId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.mapId);
            writer.WriteInt(this.npcId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.mapId = reader.ReadInt();
            this.npcId = reader.ReadInt();
        }
    }
}