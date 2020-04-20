using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DecraftResultMessage : Message {
        public const ushort Id = 6569;

        public override ushort MessageId {
            get { return Id; }
        }

        public DecraftedItemStackInfo[] results;


        public DecraftResultMessage() { }

        public DecraftResultMessage(DecraftedItemStackInfo[] results) {
            this.results = results;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.results.Length);
            foreach (var entry in this.results) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.results = new DecraftedItemStackInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.results[i] = new DecraftedItemStackInfo();
                this.results[i].Deserialize(reader);
            }
        }
    }
}