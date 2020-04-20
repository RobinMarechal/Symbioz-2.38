using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameDataPaddockObjectAddMessage : Message {
        public const ushort Id = 5990;

        public override ushort MessageId {
            get { return Id; }
        }

        public PaddockItem paddockItemDescription;


        public GameDataPaddockObjectAddMessage() { }

        public GameDataPaddockObjectAddMessage(PaddockItem paddockItemDescription) {
            this.paddockItemDescription = paddockItemDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.paddockItemDescription.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.paddockItemDescription = new PaddockItem();
            this.paddockItemDescription.Deserialize(reader);
        }
    }
}