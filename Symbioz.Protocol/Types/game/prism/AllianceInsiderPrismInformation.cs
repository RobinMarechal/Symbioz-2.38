// Generated on 04/27/2016 01:13:18

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class AllianceInsiderPrismInformation : PrismInformation {
        public const short Id = 431;

        public override short TypeId {
            get { return Id; }
        }

        public int lastTimeSlotModificationDate;
        public uint lastTimeSlotModificationAuthorGuildId;
        public ulong lastTimeSlotModificationAuthorId;
        public string lastTimeSlotModificationAuthorName;
        public ObjectItem[] modulesObjects;


        public AllianceInsiderPrismInformation() { }

        public AllianceInsiderPrismInformation(sbyte typeId,
                                               sbyte state,
                                               int nextVulnerabilityDate,
                                               int placementDate,
                                               uint rewardTokenCount,
                                               int lastTimeSlotModificationDate,
                                               uint lastTimeSlotModificationAuthorGuildId,
                                               ulong lastTimeSlotModificationAuthorId,
                                               string lastTimeSlotModificationAuthorName,
                                               ObjectItem[] modulesObjects)
            : base(typeId, state, nextVulnerabilityDate, placementDate, rewardTokenCount) {
            this.lastTimeSlotModificationDate = lastTimeSlotModificationDate;
            this.lastTimeSlotModificationAuthorGuildId = lastTimeSlotModificationAuthorGuildId;
            this.lastTimeSlotModificationAuthorId = lastTimeSlotModificationAuthorId;
            this.lastTimeSlotModificationAuthorName = lastTimeSlotModificationAuthorName;
            this.modulesObjects = modulesObjects;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.lastTimeSlotModificationDate);
            writer.WriteVarUhInt(this.lastTimeSlotModificationAuthorGuildId);
            writer.WriteVarUhLong(this.lastTimeSlotModificationAuthorId);
            writer.WriteUTF(this.lastTimeSlotModificationAuthorName);
            writer.WriteUShort((ushort) this.modulesObjects.Length);
            foreach (var entry in this.modulesObjects) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.lastTimeSlotModificationDate = reader.ReadInt();

            if (this.lastTimeSlotModificationDate < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationDate = " + this.lastTimeSlotModificationDate + ", it doesn't respect the following condition : lastTimeSlotModificationDate < 0");
            this.lastTimeSlotModificationAuthorGuildId = reader.ReadVarUhInt();

            if (this.lastTimeSlotModificationAuthorGuildId < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorGuildId = "
                                    + this.lastTimeSlotModificationAuthorGuildId
                                    + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorGuildId < 0");
            this.lastTimeSlotModificationAuthorId = reader.ReadVarUhLong();

            if (this.lastTimeSlotModificationAuthorId < 0 || this.lastTimeSlotModificationAuthorId > 9007199254740990)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorId = "
                                    + this.lastTimeSlotModificationAuthorId
                                    + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorId < 0 || lastTimeSlotModificationAuthorId > 9007199254740990");
            this.lastTimeSlotModificationAuthorName = reader.ReadUTF();
            var limit = reader.ReadUShort();
            this.modulesObjects = new ObjectItem[limit];
            for (int i = 0; i < limit; i++) {
                this.modulesObjects[i] = new ObjectItem();
                this.modulesObjects[i].Deserialize(reader);
            }
        }
    }
}