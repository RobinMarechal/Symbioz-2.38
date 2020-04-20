using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ShortcutBarContentMessage : Message {
        public const ushort Id = 6231;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte barType;
        public Shortcut[] shortcuts;


        public ShortcutBarContentMessage() { }

        public ShortcutBarContentMessage(sbyte barType, Shortcut[] shortcuts) {
            this.barType = barType;
            this.shortcuts = shortcuts;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.barType);
            writer.WriteUShort((ushort) this.shortcuts.Length);
            foreach (var entry in this.shortcuts) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.barType = reader.ReadSByte();

            if (this.barType < 0)
                throw new Exception("Forbidden value on barType = " + this.barType + ", it doesn't respect the following condition : barType < 0");
            var limit = reader.ReadUShort();
            this.shortcuts = new Shortcut[limit];
            for (int i = 0; i < limit; i++) {
                this.shortcuts[i] = ProtocolTypeManager.GetInstance<Shortcut>(reader.ReadShort());
                this.shortcuts[i].Deserialize(reader);
            }
        }
    }
}