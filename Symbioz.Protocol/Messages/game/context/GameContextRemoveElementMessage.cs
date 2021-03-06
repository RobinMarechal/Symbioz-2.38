using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextRemoveElementMessage : Message {
        public const ushort Id = 251;

        public override ushort MessageId {
            get { return Id; }
        }

        public double id;


        public GameContextRemoveElementMessage() { }

        public GameContextRemoveElementMessage(double id) {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
        }
    }
}