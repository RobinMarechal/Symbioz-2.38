using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CompassUpdateMessage : Message {
        public const ushort Id = 5591;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte type;
        public MapCoordinates coords;


        public CompassUpdateMessage() { }

        public CompassUpdateMessage(sbyte type, MapCoordinates coords) {
            this.type = type;
            this.coords = coords;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.type);
            writer.WriteShort(this.coords.TypeId);
            this.coords.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.type = reader.ReadSByte();

            if (this.type < 0)
                throw new Exception("Forbidden value on type = " + this.type + ", it doesn't respect the following condition : type < 0");
            this.coords = ProtocolTypeManager.GetInstance<MapCoordinates>(reader.ReadShort());
            this.coords.Deserialize(reader);
        }
    }
}