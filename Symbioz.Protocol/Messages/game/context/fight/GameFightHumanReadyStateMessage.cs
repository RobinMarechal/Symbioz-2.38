using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightHumanReadyStateMessage : Message {
        public const ushort Id = 740;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong characterId;
        public bool isReady;


        public GameFightHumanReadyStateMessage() { }

        public GameFightHumanReadyStateMessage(ulong characterId, bool isReady) {
            this.characterId = characterId;
            this.isReady = isReady;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.characterId);
            writer.WriteBoolean(this.isReady);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.characterId = reader.ReadVarUhLong();

            if (this.characterId < 0 || this.characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + this.characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
            this.isReady = reader.ReadBoolean();
        }
    }
}