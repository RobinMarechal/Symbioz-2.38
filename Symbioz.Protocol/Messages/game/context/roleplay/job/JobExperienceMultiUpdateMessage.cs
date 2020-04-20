using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobExperienceMultiUpdateMessage : Message {
        public const ushort Id = 5809;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobExperience[] experiencesUpdate;


        public JobExperienceMultiUpdateMessage() { }

        public JobExperienceMultiUpdateMessage(JobExperience[] experiencesUpdate) {
            this.experiencesUpdate = experiencesUpdate;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.experiencesUpdate.Length);
            foreach (var entry in this.experiencesUpdate) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.experiencesUpdate = new JobExperience[limit];
            for (int i = 0; i < limit; i++) {
                this.experiencesUpdate[i] = new JobExperience();
                this.experiencesUpdate[i].Deserialize(reader);
            }
        }
    }
}