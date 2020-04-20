using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicDateMessage : Message {
        public const ushort Id = 177;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte day;
        public sbyte month;
        public short year;


        public BasicDateMessage() { }

        public BasicDateMessage(sbyte day, sbyte month, short year) {
            this.day = day;
            this.month = month;
            this.year = year;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.day);
            writer.WriteSByte(this.month);
            writer.WriteShort(this.year);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.day = reader.ReadSByte();

            if (this.day < 0)
                throw new Exception("Forbidden value on day = " + this.day + ", it doesn't respect the following condition : day < 0");
            this.month = reader.ReadSByte();

            if (this.month < 0)
                throw new Exception("Forbidden value on month = " + this.month + ", it doesn't respect the following condition : month < 0");
            this.year = reader.ReadShort();

            if (this.year < 0)
                throw new Exception("Forbidden value on year = " + this.year + ", it doesn't respect the following condition : year < 0");
        }
    }
}