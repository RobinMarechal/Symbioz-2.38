using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterSelectionMessage : Message {
        public const ushort Id = 152;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong id;


        public CharacterSelectionMessage() { }

        public CharacterSelectionMessage(ulong id) {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.id);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadVarUhLong();

            if (this.id < 0 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0 || id > 9007199254740990");
        }
    }
}