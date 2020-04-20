// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations {
        public const short Id = 3;

        public override short TypeId {
            get { return Id; }
        }

        public ushort monsterId;
        public sbyte powerLevel;


        public GameRolePlayMutantInformations() { }

        public GameRolePlayMutantInformations(double contextualId,
                                              EntityLook look,
                                              EntityDispositionInformations disposition,
                                              string name,
                                              HumanInformations humanoidInfo,
                                              int accountId,
                                              ushort monsterId,
                                              sbyte powerLevel)
            : base(contextualId, look, disposition, name, humanoidInfo, accountId) {
            this.monsterId = monsterId;
            this.powerLevel = powerLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.monsterId);
            writer.WriteSByte(this.powerLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.monsterId = reader.ReadVarUhShort();

            if (this.monsterId < 0)
                throw new Exception("Forbidden value on monsterId = " + this.monsterId + ", it doesn't respect the following condition : monsterId < 0");
            this.powerLevel = reader.ReadSByte();
        }
    }
}