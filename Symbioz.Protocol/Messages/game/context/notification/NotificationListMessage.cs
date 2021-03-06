using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NotificationListMessage : Message {
        public const ushort Id = 6087;

        public override ushort MessageId {
            get { return Id; }
        }

        public int[] flags;


        public NotificationListMessage() { }

        public NotificationListMessage(int[] flags) {
            this.flags = flags;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.flags.Length);
            foreach (var entry in this.flags) {
                writer.WriteVarInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.flags = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.flags[i] = reader.ReadVarInt();
            }
        }
    }
}