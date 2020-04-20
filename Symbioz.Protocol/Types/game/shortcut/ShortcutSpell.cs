// Generated on 04/27/2016 01:13:19

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ShortcutSpell : Shortcut {
        public const short Id = 368;

        public override short TypeId {
            get { return Id; }
        }

        public ushort spellId;


        public ShortcutSpell() { }

        public ShortcutSpell(sbyte slot, ushort spellId)
            : base(slot) {
            this.spellId = spellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.spellId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.spellId = reader.ReadVarUhShort();

            if (this.spellId < 0)
                throw new Exception("Forbidden value on spellId = " + this.spellId + ", it doesn't respect the following condition : spellId < 0");
        }
    }
}