using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameDataPaddockObjectListAddMessage : Message {
        public const ushort Id = 5992;

        public override ushort MessageId {
            get { return Id; }
        }

        public PaddockItem[] paddockItemDescription;


        public GameDataPaddockObjectListAddMessage() { }

        public GameDataPaddockObjectListAddMessage(PaddockItem[] paddockItemDescription) {
            this.paddockItemDescription = paddockItemDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.paddockItemDescription.Length);
            foreach (var entry in this.paddockItemDescription) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.paddockItemDescription = new PaddockItem[limit];
            for (int i = 0; i < limit; i++) {
                this.paddockItemDescription[i] = new PaddockItem();
                this.paddockItemDescription[i].Deserialize(reader);
            }
        }
    }
}