using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StartupActionsObjetAttributionMessage : Message {
        public const ushort Id = 1303;

        public override ushort MessageId {
            get { return Id; }
        }

        public int actionId;
        public ulong characterId;


        public StartupActionsObjetAttributionMessage() { }

        public StartupActionsObjetAttributionMessage(int actionId, ulong characterId) {
            this.actionId = actionId;
            this.characterId = characterId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.actionId);
            writer.WriteVarUhLong(this.characterId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.actionId = reader.ReadInt();

            if (this.actionId < 0)
                throw new Exception("Forbidden value on actionId = " + this.actionId + ", it doesn't respect the following condition : actionId < 0");
            this.characterId = reader.ReadVarUhLong();

            if (this.characterId < 0 || this.characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + this.characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
        }
    }
}