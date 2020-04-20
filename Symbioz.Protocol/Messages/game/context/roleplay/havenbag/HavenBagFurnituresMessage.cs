using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HavenBagFurnituresMessage : Message {
        public const ushort Id = 6634;

        public override ushort MessageId {
            get { return Id; }
        }

        public HavenBagFurnitureInformation[] furnituresInfos;


        public HavenBagFurnituresMessage() { }

        public HavenBagFurnituresMessage(HavenBagFurnitureInformation[] furnituresInfos) {
            this.furnituresInfos = furnituresInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.furnituresInfos.Length);
            foreach (var entry in this.furnituresInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.furnituresInfos = new HavenBagFurnitureInformation[limit];
            for (int i = 0; i < limit; i++) {
                this.furnituresInfos[i] = new HavenBagFurnitureInformation();
                this.furnituresInfos[i].Deserialize(reader);
            }
        }
    }
}