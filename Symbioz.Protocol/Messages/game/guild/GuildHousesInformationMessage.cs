using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildHousesInformationMessage : Message {
        public const ushort Id = 5919;

        public override ushort MessageId {
            get { return Id; }
        }

        public HouseInformationsForGuild[] housesInformations;


        public GuildHousesInformationMessage() { }

        public GuildHousesInformationMessage(HouseInformationsForGuild[] housesInformations) {
            this.housesInformations = housesInformations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.housesInformations.Length);
            foreach (var entry in this.housesInformations) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.housesInformations = new HouseInformationsForGuild[limit];
            for (int i = 0; i < limit; i++) {
                this.housesInformations[i] = new HouseInformationsForGuild();
                this.housesInformations[i].Deserialize(reader);
            }
        }
    }
}