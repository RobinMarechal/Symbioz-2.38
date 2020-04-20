using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ShortcutBarRemovedMessage : Message {
        public const ushort Id = 6224;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte barType;
        public sbyte slot;


        public ShortcutBarRemovedMessage() { }

        public ShortcutBarRemovedMessage(sbyte barType, sbyte slot) {
            this.barType = barType;
            this.slot = slot;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.barType);
            writer.WriteSByte(this.slot);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.barType = reader.ReadSByte();

            if (this.barType < 0)
                throw new Exception("Forbidden value on barType = " + this.barType + ", it doesn't respect the following condition : barType < 0");
            this.slot = reader.ReadSByte();

            if (this.slot < 0 || this.slot > 99)
                throw new Exception("Forbidden value on slot = " + this.slot + ", it doesn't respect the following condition : slot < 0 || slot > 99");
        }
    }
}