using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceVersatileInfoListMessage : Message {
        public const ushort Id = 6436;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceVersatileInformations[] alliances;


        public AllianceVersatileInfoListMessage() { }

        public AllianceVersatileInfoListMessage(AllianceVersatileInformations[] alliances) {
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
            this.alliances = new AllianceVersatileInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.alliances[i] = new AllianceVersatileInformations();
                this.alliances[i].Deserialize(reader);
            }
        }
    }
}