using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectorySettingsMessage : Message {
        public const ushort Id = 5652;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobCrafterDirectorySettings[] craftersSettings;


        public JobCrafterDirectorySettingsMessage() { }

        public JobCrafterDirectorySettingsMessage(JobCrafterDirectorySettings[] craftersSettings) {
            this.craftersSettings = craftersSettings;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.craftersSettings.Length);
            foreach (var entry in this.craftersSettings) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.craftersSettings = new JobCrafterDirectorySettings[limit];
            for (int i = 0; i < limit; i++) {
                this.craftersSettings[i] = new JobCrafterDirectorySettings();
                this.craftersSettings[i].Deserialize(reader);
            }
        }
    }
}