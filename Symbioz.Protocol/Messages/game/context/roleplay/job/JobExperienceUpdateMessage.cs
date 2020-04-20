using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobExperienceUpdateMessage : Message {
        public const ushort Id = 5654;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobExperience experiencesUpdate;


        public JobExperienceUpdateMessage() { }

        public JobExperienceUpdateMessage(JobExperience experiencesUpdate) {
            this.experiencesUpdate = experiencesUpdate;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.experiencesUpdate.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.experiencesUpdate = new JobExperience();
            this.experiencesUpdate.Deserialize(reader);
        }
    }
}