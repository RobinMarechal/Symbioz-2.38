using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class MapRunningFightListMessage : Message {
        public const ushort Id = 5743;

        public override ushort MessageId {
            get { return Id; }
        }

        public FightExternalInformations[] fights;


        public MapRunningFightListMessage() { }

        public MapRunningFightListMessage(FightExternalInformations[] fights) {
            this.fights = fights;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.fights.Length);
            foreach (var entry in this.fights) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.fights = new FightExternalInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.fights[i] = new FightExternalInformations();
                this.fights[i].Deserialize(reader);
            }
        }
    }
}