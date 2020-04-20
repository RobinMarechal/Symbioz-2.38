// Generated on 04/27/2016 01:13:13

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class GameRolePlayMountInformations : GameRolePlayNamedActorInformations {
        public const short Id = 180;

        public override short TypeId {
            get { return Id; }
        }

        public string ownerName;
        public byte level;


        public GameRolePlayMountInformations() { }

        public GameRolePlayMountInformations(double contextualId, EntityLook look, EntityDispositionInformations disposition, string name, string ownerName, byte level)
            : base(contextualId, look, disposition, name) {
            this.ownerName = ownerName;
            this.level = level;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUTF(this.ownerName);
            writer.WriteByte(this.level);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.ownerName = reader.ReadUTF();
            this.level = reader.ReadByte();

            if (this.level < 0 || this.level > 255)
                throw new Exception("Forbidden value on level = " + this.level + ", it doesn't respect the following condition : level < 0 || level > 255");
        }
    }
}