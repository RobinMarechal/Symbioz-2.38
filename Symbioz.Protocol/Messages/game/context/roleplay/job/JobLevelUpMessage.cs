using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobLevelUpMessage : Message {
        public const ushort Id = 5656;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte newLevel;
        public JobDescription jobsDescription;


        public JobLevelUpMessage() { }

        public JobLevelUpMessage(byte newLevel, JobDescription jobsDescription) {
            this.newLevel = newLevel;
            this.jobsDescription = jobsDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.newLevel);
            this.jobsDescription.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.newLevel = reader.ReadByte();

            if (this.newLevel < 0 || this.newLevel > 255)
                throw new Exception("Forbidden value on newLevel = " + this.newLevel + ", it doesn't respect the following condition : newLevel < 0 || newLevel > 255");
            this.jobsDescription = new JobDescription();
            this.jobsDescription.Deserialize(reader);
        }
    }
}