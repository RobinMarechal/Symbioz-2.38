using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ShortcutBarRefreshMessage : Message {
        public const ushort Id = 6229;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte barType;
        public Shortcut shortcut;


        public ShortcutBarRefreshMessage() { }

        public ShortcutBarRefreshMessage(sbyte barType, Shortcut shortcut) {
            this.barType = barType;
            this.shortcut = shortcut;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.barType);
            writer.WriteShort(this.shortcut.TypeId);
            this.shortcut.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.barType = reader.ReadSByte();

            if (this.barType < 0)
                throw new Exception("Forbidden value on barType = " + this.barType + ", it doesn't respect the following condition : barType < 0");
            this.shortcut = ProtocolTypeManager.GetInstance<Shortcut>(reader.ReadShort());
            this.shortcut.Deserialize(reader);
        }
    }
}