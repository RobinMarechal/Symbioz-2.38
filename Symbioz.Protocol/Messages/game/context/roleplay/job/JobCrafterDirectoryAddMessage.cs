using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectoryAddMessage : Message {
        public const ushort Id = 5651;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobCrafterDirectoryListEntry listEntry;


        public JobCrafterDirectoryAddMessage() { }

        public JobCrafterDirectoryAddMessage(JobCrafterDirectoryListEntry listEntry) {
            this.listEntry = listEntry;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.listEntry.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.listEntry = new JobCrafterDirectoryListEntry();
            this.listEntry.Deserialize(reader);
        }
    }
}