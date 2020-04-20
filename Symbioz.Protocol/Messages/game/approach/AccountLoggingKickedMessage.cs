using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AccountLoggingKickedMessage : Message {
        public const ushort Id = 6029;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort days;
        public sbyte hours;
        public sbyte minutes;


        public AccountLoggingKickedMessage() { }

        public AccountLoggingKickedMessage(ushort days, sbyte hours, sbyte minutes) {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.days);
            writer.WriteSByte(this.hours);
            writer.WriteSByte(this.minutes);
        }

        public override void Deserialize(ICustomDataInput reader) {
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