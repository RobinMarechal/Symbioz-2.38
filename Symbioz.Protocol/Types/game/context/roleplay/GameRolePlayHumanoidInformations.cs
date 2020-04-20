// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayHumanoidInformations : GameRolePlayNamedActorInformations {
        public const short Id = 159;

        public override short TypeId {
            get { return Id; }
        }

        public HumanInformations humanoidInfo;
        public int accountId;


        public GameRolePlayHumanoidInformations() { }

        public GameRolePlayHumanoidInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, string name, HumanInformations humanoidInfo, int accountId)
            : base(contextualId, look, disposition, name) {
            this.humanoidInfo = humanoidInfo;
            this.accountId = accountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.humanoidInfo.TypeId);
            this.humanoidInfo.Serialize(writer);
            writer.WriteInt(this.accountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.humanoidInfo = ProtocolTypeManager.GetInstance<HumanInformations>(reader.ReadShort());
            this.humanoidInfo.Deserialize(reader);
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
        }
    }
}