using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismSettingsRequestMessage : Message {
        public const ushort Id = 6437;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;
        public sbyte startDefenseTime;


        public PrismSettingsRequestMessage() { }

        public PrismSettingsRequestMessage(ushort subAreaId, sbyte startDefenseTime) {
            this.subAreaId = subAreaId;
            this.startDefenseTime = startDefenseTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteSByte(this.startDefenseTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.startDefenseTime = reader.ReadSByte();

            if (this.startDefenseTime < 0)
                throw new Exception("Forbidden value on startDefenseTime = " + this.startDefenseTime + ", it doesn't respect the following condition : startDefenseTime < 0");
        }
    }
}