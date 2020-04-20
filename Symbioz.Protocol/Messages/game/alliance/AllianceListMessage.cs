using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceListMessage : Message {
        public const ushort Id = 6408;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceFactSheetInformations[] alliances;


        public AllianceListMessage() { }

        public AllianceListMessage(AllianceFactSheetInformations[] alliances) {
            this.alliances = alliances;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.alliances.Length);
            foreach (var entry in this.alliances) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.alliances = new AllianceFactSheetInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.alliances[i] = new AllianceFactSheetInformations();
                this.alliances[i].Deserialize(reader);
            }
        }
    }
}