using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class HousePropertiesMessage : Message {
        public const ushort Id = 5734;

        public override ushort MessageId {
            get { return Id; }
        }

        public HouseInformations properties;


        public HousePropertiesMessage() { }

        public HousePropertiesMessage(HouseInformations properties) {
            this.properties = properties;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.properties.TypeId);
            this.properties.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.properties = ProtocolTypeManager.GetInstance<HouseInformations>(reader.ReadShort());
            this.properties.Deserialize(reader);
        }
    }
}