using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChatServerWithObjectMessage : ChatServerMessage {
        public const ushort Id = 883;

        public override ushort MessageId {
            get { return Id; }
        }

        public ObjectItem[] objects;


        public ChatServerWithObjectMessage() { }

        public ChatServerWithObjectMessage(sbyte channel, string content, int timestamp, string fingerprint, double senderId, string senderName, int senderAccountId, ObjectItem[] objects)
            : base(channel, content, timestamp, fingerprint, senderId, senderName, senderAccountId) {
            this.objects = objects;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.objects.Length);
            foreach (var entry in this.objects) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.objects = new ObjectItem[limit];
            for (int i = 0; i < limit; i++) {
                this.objects[i] = new ObjectItem();
                this.objects[i].Deserialize(reader);
            }
        }
    }
}