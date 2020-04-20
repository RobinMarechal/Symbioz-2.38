using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobDescriptionMessage : Message {
        public const ushort Id = 5655;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobDescription[] jobsDescription;


        public JobDescriptionMessage() { }

        public JobDescriptionMessage(JobDescription[] jobsDescription) {
            this.jobsDescription = jobsDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.jobsDescription.Length);
            foreach (var entry in this.jobsDescription) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.jobsDescription = new JobDescription[limit];
            for (int i = 0; i < limit; i++) {
                this.jobsDescription[i] = new JobDescription();
                this.jobsDescription[i].Deserialize(reader);
            }
        }
    }
}