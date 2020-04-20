// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ObjectEffectDate : ObjectEffect {
        public const short Id = 72;

        public override short TypeId {
            get { return Id; }
        }

        public ushort year;
        public sbyte month;
        public sbyte day;
        public sbyte hour;
        public sbyte minute;


        public ObjectEffectDate() { }

        public ObjectEffectDate(ushort actionId, ushort year, sbyte month, sbyte day, sbyte hour, sbyte minute)
            : base(actionId) {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.year);
            writer.WriteSByte(this.month);
            writer.WriteSByte(this.day);
            writer.WriteSByte(this.hour);
            writer.WriteSByte(this.minute);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.year = reader.ReadVarUhShort();

            if (this.year < 0)
                throw new Exception("Forbidden value on year = " + this.year + ", it doesn't respect the following condition : year < 0");
            this.month = reader.ReadSByte();

            if (this.month < 0)
                throw new Exception("Forbidden value on month = " + this.month + ", it doesn't respect the following condition : month < 0");
            this.day = reader.ReadSByte();

            if (this.day < 0)
                throw new Exception("Forbidden value on day = " + this.day + ", it doesn't respect the following condition : day < 0");
            this.hour = reader.ReadSByte();

            if (this.hour < 0)
                throw new Exception("Forbidden value on hour = " + this.hour + ", it doesn't respect the following condition : hour < 0");
            this.minute = reader.ReadSByte();

            if (this.minute < 0)
                throw new Exception("Forbidden value on minute = " + this.minute + ", it doesn't respect the following condition : minute < 0");
        }
    }
}