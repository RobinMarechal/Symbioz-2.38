// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations {
        public const short Id = 129;

        public override short TypeId {
            get { return Id; }
        }

        public sbyte sellType;
        public HumanOption[] options;


        public GameRolePlayMerchantInformations() { }

        public GameRolePlayMerchantInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, string name, sbyte sellType, HumanOption[] options)
            : base(contextualId, look, disposition, name) {
            this.sellType = sellType;
            this.options = options;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteSByte(this.sellType);
            writer.WriteUShort((ushort) this.options.Length);
            foreach (var entry in this.options) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.sellType = reader.ReadSByte();

            if (this.sellType < 0)
                throw new Exception("Forbidden value on sellType = " + this.sellType + ", it doesn't respect the following condition : sellType < 0");
            var limit = reader.ReadUShort();
            this.options = new HumanOption[limit];
            for (int i = 0; i < limit; i++) {
                this.options[i] = ProtocolTypeManager.GetInstance<HumanOption>(reader.ReadShort());
                this.options[i].Deserialize(reader);
            }
        }
    }
}