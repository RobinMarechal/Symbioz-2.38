using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StartupActionsAllAttributionMessage : Message {
        public const ushort Id = 6537;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong characterId;


        public StartupActionsAllAttributionMessage() { }

        public StartupActionsAllAttributionMessage(ulong characterId) {
            this.characterId = characterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.characterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.characterId = reader.ReadVarUhLong();

            if (this.characterId < 0 || this.characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + this.characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
        }
    }
}