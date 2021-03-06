using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterCapabilitiesMessage : Message {
        public const ushort Id = 6339;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint guildEmblemSymbolCategories;


        public CharacterCapabilitiesMessage() { }

        public CharacterCapabilitiesMessage(uint guildEmblemSymbolCategories) {
            this.guildEmblemSymbolCategories = guildEmblemSymbolCategories;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.guildEmblemSymbolCategories);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.guildEmblemSymbolCategories = reader.ReadVarUhInt();

            if (this.guildEmblemSymbolCategories < 0)
                throw new Exception("Forbidden value on guildEmblemSymbolCategories = " + this.guildEmblemSymbolCategories + ", it doesn't respect the following condition : guildEmblemSymbolCategories < 0");
        }
    }
}