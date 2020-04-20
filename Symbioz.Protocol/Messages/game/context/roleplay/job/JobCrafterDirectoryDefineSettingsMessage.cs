using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectoryDefineSettingsMessage : Message {
        public const ushort Id = 5649;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobCrafterDirectorySettings settings;


        public JobCrafterDirectoryDefineSettingsMessage() { }

        public JobCrafterDirectoryDefineSettingsMessage(JobCrafterDirectorySettings settings) {
            this.settings = settings;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.settings.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.settings = new JobCrafterDirectorySettings();
            this.settings.Deserialize(reader);
        }
    }
}