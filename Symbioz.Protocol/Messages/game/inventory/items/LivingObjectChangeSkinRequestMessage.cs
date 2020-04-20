using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LivingObjectChangeSkinRequestMessage : Message {
        public const ushort Id = 5725;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint livingUID;
        public byte livingPosition;
        public uint skinId;


        public LivingObjectChangeSkinRequestMessage() { }

        public LivingObjectChangeSkinRequestMessage(uint livingUID, byte livingPosition, uint skinId) {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
            this.skinId = skinId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.livingUID);
            writer.WriteByte(this.livingPosition);
            writer.WriteVarUhInt(this.skinId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.livingUID = reader.ReadVarUhInt();

            if (this.livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + this.livingUID + ", it doesn't respect the following condition : livingUID < 0");
            this.livingPosition = reader.ReadByte();

            if (this.livingPosition < 0 || this.livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + this.livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
            this.skinId = reader.ReadVarUhInt();

            if (this.skinId < 0)
                throw new Exception("Forbidden value on skinId = " + this.skinId + ", it doesn't respect the following condition : skinId < 0");
        }
    }
}