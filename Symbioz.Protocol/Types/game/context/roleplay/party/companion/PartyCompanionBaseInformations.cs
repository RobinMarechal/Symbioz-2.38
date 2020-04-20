// Generated on 04/27/2016 01:13:15

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class PartyCompanionBaseInformations {
        public const short Id = 453;

        public virtual short TypeId {
            get { return Id; }
        }

        public sbyte indexId;
        public sbyte companionGenericId;
        public EntityLook entityLook;


        public PartyCompanionBaseInformations() { }

        public PartyCompanionBaseInformations(sbyte indexId, sbyte companionGenericId, EntityLook entityLook) {
            this.indexId = indexId;
            this.companionGenericId = companionGenericId;
            this.entityLook = entityLook;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.indexId);
            writer.WriteSByte(this.companionGenericId);
            this.entityLook.Serialize(writer);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.indexId = reader.ReadSByte();

            if (this.indexId < 0)
                throw new Exception("Forbidden value on indexId = " + this.indexId + ", it doesn't respect the following condition : indexId < 0");
            this.companionGenericId = reader.ReadSByte();

            if (this.companionGenericId < 0)
                throw new Exception("Forbidden value on companionGenericId = " + this.companionGenericId + ", it doesn't respect the following condition : companionGenericId < 0");
            this.entityLook = new EntityLook();
            this.entityLook.Deserialize(reader);
        }
    }
}