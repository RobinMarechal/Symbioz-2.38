using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MountInformationRequestMessage : Message {
        public const ushort Id = 5972;

        public override ushort MessageId {
            get { return Id; }
        }

        public double id;
        public double time;


        public MountInformationRequestMessage() { }

        public MountInformationRequestMessage(double id, double time) {
            this.id = id;
            this.time = time;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
            writer.WriteDouble(this.time);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            this.time = reader.ReadDouble();

            if (this.time < -9007199254740990 || this.time > 9007199254740990)
                throw new Exception("Forbidden value on time = " + this.time + ", it doesn't respect the following condition : time < -9007199254740990 || time > 9007199254740990");
        }
    }
}