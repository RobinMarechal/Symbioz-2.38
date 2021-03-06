using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterSelectedForceMessage : Message {
        public const ushort Id = 6068;

        public override ushort MessageId {
            get { return Id; }
        }

        public int id;


        public CharacterSelectedForceMessage() { }

        public CharacterSelectedForceMessage(int id) {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.id);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadInt();

            if (this.id < 1 || this.id > 2147483647)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 1 || id > 2147483647");
        }
    }
}