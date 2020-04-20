using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TreasureHuntMessage : Message {
        public const ushort Id = 6486;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte questType;
        public int startMapId;
        public TreasureHuntStep[] knownStepsList;
        public sbyte totalStepCount;
        public uint checkPointCurrent;
        public uint checkPointTotal;
        public int availableRetryCount;
        public TreasureHuntFlag[] flags;


        public TreasureHuntMessage() { }

        public TreasureHuntMessage(sbyte questType,
                                   int startMapId,
                                   TreasureHuntStep[] knownStepsList,
                                   sbyte totalStepCount,
                                   uint checkPointCurrent,
                                   uint checkPointTotal,
                                   int availableRetryCount,
                                   TreasureHuntFlag[] flags) {
            this.questType = questType;
            this.startMapId = startMapId;
            this.knownStepsList = knownStepsList;
            this.totalStepCount = totalStepCount;
            this.checkPointCurrent = checkPointCurrent;
            this.checkPointTotal = checkPointTotal;
            this.availableRetryCount = availableRetryCount;
            this.flags = flags;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.questType);
            writer.WriteInt(this.startMapId);
            writer.WriteUShort((ushort) this.knownStepsList.Length);
            foreach (var entry in this.knownStepsList) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteSByte(this.totalStepCount);
            writer.WriteVarUhInt(this.checkPointCurrent);
            writer.WriteVarUhInt(this.checkPointTotal);
            writer.WriteInt(this.availableRetryCount);
            writer.WriteUShort((ushort) this.flags.Length);
            foreach (var entry in this.flags) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.questType = reader.ReadSByte();

            if (this.questType < 0)
                throw new Exception("Forbidden value on questType = " + this.questType + ", it doesn't respect the following condition : questType < 0");
            this.startMapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            this.knownStepsList = new TreasureHuntStep[limit];
            for (int i = 0; i < limit; i++) {
                this.knownStepsList[i] = ProtocolTypeManager.GetInstance<TreasureHuntStep>(reader.ReadShort());
                this.knownStepsList[i].Deserialize(reader);
            }

            this.totalStepCount = reader.ReadSByte();

            if (this.totalStepCount < 0)
                throw new Exception("Forbidden value on totalStepCount = " + this.totalStepCount + ", it doesn't respect the following condition : totalStepCount < 0");
            this.checkPointCurrent = reader.ReadVarUhInt();

            if (this.checkPointCurrent < 0)
                throw new Exception("Forbidden value on checkPointCurrent = " + this.checkPointCurrent + ", it doesn't respect the following condition : checkPointCurrent < 0");
            this.checkPointTotal = reader.ReadVarUhInt();

            if (this.checkPointTotal < 0)
                throw new Exception("Forbidden value on checkPointTotal = " + this.checkPointTotal + ", it doesn't respect the following condition : checkPointTotal < 0");
            this.availableRetryCount = reader.ReadInt();
            limit = reader.ReadUShort();
            this.flags = new TreasureHuntFlag[limit];
            for (int i = 0; i < limit; i++) {
                this.flags[i] = new TreasureHuntFlag();
                this.flags[i].Deserialize(reader);
            }
        }
    }
}