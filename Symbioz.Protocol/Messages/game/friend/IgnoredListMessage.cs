using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IgnoredListMessage : Message {
        public const ushort Id = 5674;

        public override ushort MessageId {
            get { return Id; }
        }

        public IgnoredInformations[] ignoredList;


        public IgnoredListMessage() { }

        public IgnoredListMessage(IgnoredInformations[] ignoredList) {
            this.ignoredList = ignoredList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.ignoredList.Length);
            foreach (var entry in this.ignoredList) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.ignoredList = new IgnoredInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.ignoredList[i] = ProtocolTypeManager.GetInstance<IgnoredInformations>(reader.ReadShort());
                this.ignoredList[i].Deserialize(reader);
            }
        }
    }
}