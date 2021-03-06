using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TeleportRequestMessage : Message {
        public const ushort Id = 5961;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte teleporterType;
        public int mapId;


        public TeleportRequestMessage() { }

        public TeleportRequestMessage(sbyte teleporterType, int mapId) {
            this.teleporterType = teleporterType;
            this.mapId = mapId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.teleporterType);
            writer.WriteInt(this.mapId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.teleporterType = reader.ReadSByte();

            if (this.teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + this.teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            this.mapId = reader.ReadInt();

            if (this.mapId < 0)
                throw new Exception("Forbidden value on mapId = " + this.mapId + ", it doesn't respect the following condition : mapId < 0");
        }
    }
}