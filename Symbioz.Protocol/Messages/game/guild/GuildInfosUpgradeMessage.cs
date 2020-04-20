using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInfosUpgradeMessage : Message {
        public const ushort Id = 5636;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte maxTaxCollectorsCount;
        public sbyte taxCollectorsCount;
        public ushort taxCollectorLifePoints;
        public ushort taxCollectorDamagesBonuses;
        public ushort taxCollectorPods;
        public ushort taxCollectorProspecting;
        public ushort taxCollectorWisdom;
        public ushort boostPoints;
        public ushort[] spellId;
        public sbyte[] spellLevel;


        public GuildInfosUpgradeMessage() { }

        public GuildInfosUpgradeMessage(sbyte maxTaxCollectorsCount,
                                        sbyte taxCollectorsCount,
                                        ushort taxCollectorLifePoints,
                                        ushort taxCollectorDamagesBonuses,
                                        ushort taxCollectorPods,
                                        ushort taxCollectorProspecting,
                                        ushort taxCollectorWisdom,
                                        ushort boostPoints,
                                        ushort[] spellId,
                                        sbyte[] spellLevel) {
            this.maxTaxCollectorsCount = maxTaxCollectorsCount;
            this.taxCollectorsCount = taxCollectorsCount;
            this.taxCollectorLifePoints = taxCollectorLifePoints;
            this.taxCollectorDamagesBonuses = taxCollectorDamagesBonuses;
            this.taxCollectorPods = taxCollectorPods;
            this.taxCollectorProspecting = taxCollectorProspecting;
            this.taxCollectorWisdom = taxCollectorWisdom;
            this.boostPoints = boostPoints;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.maxTaxCollectorsCount);
            writer.WriteSByte(this.taxCollectorsCount);
            writer.WriteVarUhShort(this.taxCollectorLifePoints);
            writer.WriteVarUhShort(this.taxCollectorDamagesBonuses);
            writer.WriteVarUhShort(this.taxCollectorPods);
            writer.WriteVarUhShort(this.taxCollectorProspecting);
            writer.WriteVarUhShort(this.taxCollectorWisdom);
            writer.WriteVarUhShort(this.boostPoints);
            writer.WriteUShort((ushort) this.spellId.Length);
            foreach (var entry in this.spellId) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.spellLevel.Length);
            foreach (var entry in this.spellLevel) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.maxTaxCollectorsCount = reader.ReadSByte();

            if (this.maxTaxCollectorsCount < 0)
                throw new Exception("Forbidden value on maxTaxCollectorsCount = " + this.maxTaxCollectorsCount + ", it doesn't respect the following condition : maxTaxCollectorsCount < 0");
            this.taxCollectorsCount = reader.ReadSByte();

            if (this.taxCollectorsCount < 0)
                throw new Exception("Forbidden value on taxCollectorsCount = " + this.taxCollectorsCount + ", it doesn't respect the following condition : taxCollectorsCount < 0");
            this.taxCollectorLifePoints = reader.ReadVarUhShort();

            if (this.taxCollectorLifePoints < 0)
                throw new Exception("Forbidden value on taxCollectorLifePoints = " + this.taxCollectorLifePoints + ", it doesn't respect the following condition : taxCollectorLifePoints < 0");
            this.taxCollectorDamagesBonuses = reader.ReadVarUhShort();

            if (this.taxCollectorDamagesBonuses < 0)
                throw new Exception("Forbidden value on taxCollectorDamagesBonuses = " + this.taxCollectorDamagesBonuses + ", it doesn't respect the following condition : taxCollectorDamagesBonuses < 0");
            this.taxCollectorPods = reader.ReadVarUhShort();

            if (this.taxCollectorPods < 0)
                throw new Exception("Forbidden value on taxCollectorPods = " + this.taxCollectorPods + ", it doesn't respect the following condition : taxCollectorPods < 0");
            this.taxCollectorProspecting = reader.ReadVarUhShort();

            if (this.taxCollectorProspecting < 0)
                throw new Exception("Forbidden value on taxCollectorProspecting = " + this.taxCollectorProspecting + ", it doesn't respect the following condition : taxCollectorProspecting < 0");
            this.taxCollectorWisdom = reader.ReadVarUhShort();

            if (this.taxCollectorWisdom < 0)
                throw new Exception("Forbidden value on taxCollectorWisdom = " + this.taxCollectorWisdom + ", it doesn't respect the following condition : taxCollectorWisdom < 0");
            this.boostPoints = reader.ReadVarUhShort();

            if (this.boostPoints < 0)
                throw new Exception("Forbidden value on boostPoints = " + this.boostPoints + ", it doesn't respect the following condition : boostPoints < 0");
            var limit = reader.ReadUShort();
            this.spellId = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.spellId[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.spellLevel = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.spellLevel[i] = reader.ReadSByte();
            }
        }
    }
}