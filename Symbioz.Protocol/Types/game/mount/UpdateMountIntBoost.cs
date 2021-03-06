// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class UpdateMountIntBoost : UpdateMountBoost {
        public const short Id = 357;

        public override short TypeId {
            get { return Id; }
        }

        public int value;


        public UpdateMountIntBoost() { }

        public UpdateMountIntBoost(sbyte type, int value)
            : base(type) {
            this.value = value;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.value);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.value = reader.ReadInt();
        }
    }
}