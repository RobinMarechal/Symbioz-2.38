// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayTreasureHintInformations : GameRolePlayActorInformations {
        public const short Id = 471;

        public override short TypeId {
            get { return Id; }
        }

        public ushort npcId;


        public GameRolePlayTreasureHintInformations() { }

        public GameRolePlayTreasureHintInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, ushort npcId)
            : base(contextualId, look, disposition) {
            this.npcId = npcId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.npcId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.npcId = reader.ReadVarUhShort();

            if (this.npcId < 0)
                throw new Exception("Forbidden value on npcId = " + this.npcId + ", it doesn't respect the following condition : npcId < 0");
        }
    }
}