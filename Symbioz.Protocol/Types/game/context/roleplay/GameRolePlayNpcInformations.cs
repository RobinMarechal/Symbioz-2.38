// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayNpcInformations : GameRolePlayActorInformations {
        public const short Id = 156;

        public override short TypeId {
            get { return Id; }
        }

        public ushort npcId;
        public bool sex;
        public ushort specialArtworkId;


        public GameRolePlayNpcInformations() { }

        public GameRolePlayNpcInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, ushort npcId, bool sex, ushort specialArtworkId)
            : base(contextualId, look, disposition) {
            this.npcId = npcId;
            this.sex = sex;
            this.specialArtworkId = specialArtworkId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.npcId);
            writer.WriteBoolean(this.sex);
            writer.WriteVarUhShort(this.specialArtworkId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.npcId = reader.ReadVarUhShort();

            if (this.npcId < 0)
                throw new Exception("Forbidden value on npcId = " + this.npcId + ", it doesn't respect the following condition : npcId < 0");
            this.sex = reader.ReadBoolean();
            this.specialArtworkId = reader.ReadVarUhShort();

            if (this.specialArtworkId < 0)
                throw new Exception("Forbidden value on specialArtworkId = " + this.specialArtworkId + ", it doesn't respect the following condition : specialArtworkId < 0");
        }
    }
}