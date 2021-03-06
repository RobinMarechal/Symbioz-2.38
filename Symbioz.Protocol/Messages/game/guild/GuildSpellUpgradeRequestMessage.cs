using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildSpellUpgradeRequestMessage : Message {
        public const ushort Id = 5699;

        public override ushort MessageId {
            get { return Id; }
        }

        public int spellId;


        public GuildSpellUpgradeRequestMessage() { }

        public GuildSpellUpgradeRequestMessage(int spellId) {
            this.spellId = spellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.spellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.spellId = reader.ReadInt();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
        }
    }
}