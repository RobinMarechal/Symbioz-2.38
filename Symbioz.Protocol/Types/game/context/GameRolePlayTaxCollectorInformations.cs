// Generated on 04/27/2016 01:13:10

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayTaxCollectorInformations : GameRolePlayActorInformations {
        public const short Id = 148;

        public override short TypeId {
            get { return Id; }
        }

        public TaxCollectorStaticInformations identification;
        public byte guildLevel;
        public int taxCollectorAttack;


        public GameRolePlayTaxCollectorInformations() { }

        public GameRolePlayTaxCollectorInformations(double contextualId,
                                                    EntityLook look,
                                                    EntityDispositionInformations disposition,
                                                    TaxCollectorStaticInformations identification,
                                                    byte guildLevel,
                                                    int taxCollectorAttack)
            : base(contextualId, look, disposition) {
            this.identification = identification;
            this.guildLevel = guildLevel;
            this.taxCollectorAttack = taxCollectorAttack;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.identification.TypeId);
            this.identification.Serialize(writer);
            writer.WriteByte(this.guildLevel);
            writer.WriteInt(this.taxCollectorAttack);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.identification = ProtocolTypeManager.GetInstance<TaxCollectorStaticInformations>(reader.ReadShort());
            this.identification.Deserialize(reader);
            this.guildLevel = reader.ReadByte();

            if (this.guildLevel < 0 || this.guildLevel > 255)
                throw new Exception("Forbidden value on guildLevel = " + this.guildLevel + ", it doesn't respect the following condition : guildLevel < 0 || guildLevel > 255");
            this.taxCollectorAttack = reader.ReadInt();
        }
    }
}