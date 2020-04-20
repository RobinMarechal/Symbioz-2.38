// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ShortcutEmote : Shortcut {
        public const short Id = 389;

        public override short TypeId {
            get { return Id; }
        }

        public byte emoteId;


        public ShortcutEmote() { }

        public ShortcutEmote(sbyte slot, byte emoteId)
            : base(slot) {
            this.emoteId = emoteId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteByte(this.emoteId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.emoteId = reader.ReadByte();

            if (this.emoteId < 0 || this.emoteId > 255)
                throw new Exception("Forbidden value on emoteId = " + this.emoteId + ", it doesn't respect the following condition : emoteId < 0 || emoteId > 255");
        }
    }
}