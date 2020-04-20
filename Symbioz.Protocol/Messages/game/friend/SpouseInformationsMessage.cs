using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SpouseInformationsMessage : Message {
        public const ushort Id = 6356;

        public override ushort MessageId {
            get { return Id; }
        }

        public FriendSpouseInformations spouse;


        public SpouseInformationsMessage() { }

        public SpouseInformationsMessage(FriendSpouseInformations spouse) {
            this.spouse = spouse;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.spouse.TypeId);
            this.spouse.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.spouse = ProtocolTypeManager.GetInstance<FriendSpouseInformations>(reader.ReadShort());
            this.spouse.Deserialize(reader);
        }
    }
}