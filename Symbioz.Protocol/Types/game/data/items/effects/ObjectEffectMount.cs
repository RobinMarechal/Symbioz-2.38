// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ObjectEffectMount : ObjectEffect {
        public const short Id = 179;

        public override short TypeId {
            get { return Id; }
        }

        public int mountId;
        public double date;
        public ushort modelId;


        public ObjectEffectMount() { }

        public ObjectEffectMount(ushort actionId, int mountId, double date, ushort modelId)
            : base(actionId) {
            this.mountId = mountId;
            this.date = date;
            this.modelId = modelId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.mountId);
            writer.WriteDouble(this.date);
            writer.WriteVarUhShort(this.modelId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.mountId = reader.ReadInt();

            if (this.mountId < 0)
                throw new Exception("Forbidden value on mountId = " + this.mountId + ", it doesn't respect the following condition : mountId < 0");
            this.date = reader.ReadDouble();

            if (this.date < -9007199254740990 || this.date > 9007199254740990)
                throw new Exception("Forbidden value on date = " + this.date + ", it doesn't respect the following condition : date < -9007199254740990 || date > 9007199254740990");
            this.modelId = reader.ReadVarUhShort();

            if (this.modelId < 0)
                throw new Exception("Forbidden value on modelId = " + this.modelId + ", it doesn't respect the following condition : modelId < 0");
        }
    }
}