using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeMountFreeFromPaddockMessage : Message {
        public const ushort Id = 6055;

        public override ushort MessageId {
            get { return Id; }
        }

        public string name;
        public short worldX;
        public short worldY;
        public string liberator;


        public ExchangeMountFreeFromPaddockMessage() { }

        public ExchangeMountFreeFromPaddockMessage(string name, short worldX, short worldY, string liberator) {
            this.name = name;
            this.worldX = worldX;
            this.worldY = worldY;
            this.liberator = liberator;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.name);
            writer.WriteShort(this.worldX);
            writer.WriteShort(this.worldY);
            writer.WriteUTF(this.liberator);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.name = reader.ReadUTF();
            this.worldX = reader.ReadShort();

            if (this.worldX < -255 || this.worldX > 255)
                throw new Exception("Forbidden value on worldX = " + this.worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            this.worldY = reader.ReadShort();

            if (this.worldY < -255 || this.worldY > 255)
                throw new Exception("Forbidden value on worldY = " + this.worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            this.liberator = reader.ReadUTF();
        }
    }
}