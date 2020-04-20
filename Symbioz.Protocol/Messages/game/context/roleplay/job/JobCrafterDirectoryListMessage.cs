using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectoryListMessage : Message {
        public const ushort Id = 6046;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobCrafterDirectoryListEntry[] listEntries;


        public JobCrafterDirectoryListMessage() { }

        public JobCrafterDirectoryListMessage(JobCrafterDirectoryListEntry[] listEntries) {
            this.listEntries = listEntries;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.listEntries.Length);
            foreach (var entry in this.listEntries) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.listEntries = new JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++) {
                this.listEntries[i] = new JobCrafterDirectoryListEntry();
                this.listEntries[i].Deserialize(reader);
            }
        }
    }
}