using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ShortcutBarSwapRequestMessage : Message {
        public const ushort Id = 6230;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte barType;
        public sbyte firstSlot;
        public sbyte secondSlot;


        public ShortcutBarSwapRequestMessage() { }

        public ShortcutBarSwapRequestMessage(sbyte barType, sbyte firstSlot, sbyte secondSlot) {
            this.barType = barType;
            this.firstSlot = firstSlot;
            this.secondSlot = secondSlot;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.barType);
            writer.WriteSByte(this.firstSlot);
            writer.WriteSByte(this.secondSlot);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.barType = reader.ReadSByte();

            if (this.barType < 0)
                throw new Exception("Forbidden value on barType = " + this.barType + ", it doesn't respect the following condition : barType < 0");
            this.firstSlot = reader.ReadSByte();

            if (this.firstSlot < 0 || this.firstSlot > 99)
                throw new Exception("Forbidden value on firstSlot = " + this.firstSlot + ", it doesn't respect the following condition : firstSlot < 0 || firstSlot > 99");
            this.secondSlot = reader.ReadSByte();

            if (this.secondSlot < 0 || this.secondSlot > 99)
                throw new Exception("Forbidden value on secondSlot = " + this.secondSlot + ", it doesn't respect the following condition : secondSlot < 0 || secondSlot > 99");
        }
    }
}