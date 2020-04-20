using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorAttackedMessage : Message {
        public const ushort Id = 5918;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort firstNameId;
        public ushort lastNameId;
        public short worldX;
        public short worldY;
        public int mapId;
        public ushort subAreaId;
        public BasicGuildInformations guild;


        public TaxCollectorAttackedMessage() { }

        public TaxCollectorAttackedMessage(ushort firstNameId, ushort lastNameId, short worldX, short worldY, int mapId, ushort subAreaId, BasicGuildInformations guild) {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.guild = guild;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.firstNameId);
            writer.WriteVarUhShort(this.lastNameId);
            writer.WriteShort(this.worldX);
            writer.WriteShort(this.worldY);
            writer.WriteInt(this.mapId);
            writer.WriteVarUhShort(this.subAreaId);
            this.guild.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.firstNameId = reader.ReadVarUhShort();

            if (this.firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + this.firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            this.lastNameId = reader.ReadVarUhShort();

            if (this.lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + this.lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            this.worldX = reader.ReadShort();

            if (this.worldX < -255 || this.worldX > 255)
                throw new Exception("Forbidden value on worldX = " + this.worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            this.worldY = reader.ReadShort();

            if (this.worldY < -255 || this.worldY > 255)
                throw new Exception("Forbidden value on worldY = " + this.worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            this.mapId = reader.ReadInt();
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.guild = new BasicGuildInformations();
            this.guild.Deserialize(reader);
        }
    }
}