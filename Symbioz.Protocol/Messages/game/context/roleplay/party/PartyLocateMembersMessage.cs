using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyLocateMembersMessage : AbstractPartyMessage {
        public const ushort Id = 5595;

        public override ushort MessageId {
            get { return Id; }
        }

        public PartyMemberGeoPosition[] geopositions;


        public PartyLocateMembersMessage() { }

        public PartyLocateMembersMessage(uint partyId, PartyMemberGeoPosition[] geopositions)
            : base(partyId) {
            this.geopositions = geopositions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.geopositions.Length);
            foreach (var entry in this.geopositions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.geopositions = new PartyMemberGeoPosition[limit];
            for (int i = 0; i < limit; i++) {
                this.geopositions[i] = new PartyMemberGeoPosition();
                this.geopositions[i].Deserialize(reader);
            }
        }
    }
}