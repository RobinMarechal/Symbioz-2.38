// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ObjectEffectDuration : ObjectEffect {
        public const short Id = 75;

        public override short TypeId {
            get { return Id; }
        }

        public ushort days;
        public sbyte hours;
        public sbyte minutes;


        public ObjectEffectDuration() { }

        public ObjectEffectDuration(ushort actionId, ushort days, sbyte hours, sbyte minutes)
            : base(actionId) {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.days);
            writer.WriteSByte(this.hours);
            writer.WriteSByte(this.minutes);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.days = reader.ReadVarUhShort();

            if (this.days < 0)
                throw new Exception("Forbidden value on days = " + this.days + ", it doesn't respect the following condition : days < 0");
            this.hours = reader.ReadSByte();

            if (this.hours < 0)
                throw new Exception("Forbidden value on hours = " + this.hours + ", it doesn't respect the following condition : hours < 0");
            this.minutes = reader.ReadSByte();

            if (this.minutes < 0)
                throw new Exception("Forbidden value on minutes = " + this.minutes + ", it doesn't respect the following condition : minutes < 0");
        }
    }
}