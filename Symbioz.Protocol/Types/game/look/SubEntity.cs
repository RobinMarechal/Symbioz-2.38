// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class SubEntity {
        public const short Id = 54;

        public virtual short TypeId {
            get { return Id; }
        }

        public sbyte bindingPointCategory;
        public sbyte bindingPointIndex;
        public EntityLook subEntityLook;


        public SubEntity() { }

        public SubEntity(sbyte bindingPointCategory, sbyte bindingPointIndex, EntityLook subEntityLook) {
            this.bindingPointCategory = bindingPointCategory;
            this.bindingPointIndex = bindingPointIndex;
            this.subEntityLook = subEntityLook;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.bindingPointCategory);
            writer.WriteSByte(this.bindingPointIndex);
            this.subEntityLook.Serialize(writer);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.bindingPointCategory = reader.ReadSByte();

            if (this.bindingPointCategory < 0)
                throw new Exception("Forbidden value on bindingPointCategory = " + this.bindingPointCategory + ", it doesn't respect the following condition : bindingPointCategory < 0");
            this.bindingPointIndex = reader.ReadSByte();

            if (this.bindingPointIndex < 0)
                throw new Exception("Forbidden value on bindingPointIndex = " + this.bindingPointIndex + ", it doesn't respect the following condition : bindingPointIndex < 0");
            this.subEntityLook = new EntityLook();
            this.subEntityLook.Deserialize(reader);
        }
    }
}