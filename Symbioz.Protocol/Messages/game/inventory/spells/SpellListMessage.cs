using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SpellListMessage : Message {
        public const ushort Id = 1200;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool spellPrevisualization;
        public SpellItem[] spells;


        public SpellListMessage() { }

        public SpellListMessage(bool spellPrevisualization, SpellItem[] spells) {
            this.spellPrevisualization = spellPrevisualization;
            this.spells = spells;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.spellPrevisualization);
            writer.WriteUShort((ushort) this.spells.Length);
            foreach (var entry in this.spells) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.spellPrevisualization = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.spells = new SpellItem[limit];
            for (int i = 0; i < limit; i++) {
                this.spells[i] = new SpellItem();
                this.spells[i].Deserialize(reader);
            }
        }
    }
}