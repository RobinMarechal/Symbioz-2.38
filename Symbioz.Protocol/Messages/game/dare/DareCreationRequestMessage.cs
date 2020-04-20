using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareCreationRequestMessage : Message {
        public const ushort Id = 6665;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool isPrivate;
        public bool isForGuild;
        public bool isForAlliance;
        public bool needNotifications;
        public int subscriptionFee;
        public int jackpot;
        public ushort maxCountWinners;
        public uint delayBeforeStart;
        public uint duration;
        public DareCriteria[] criterions;


        public DareCreationRequestMessage() { }

        public DareCreationRequestMessage(bool isPrivate,
                                          bool isForGuild,
                                          bool isForAlliance,
                                          bool needNotifications,
                                          int subscriptionFee,
                                          int jackpot,
                                          ushort maxCountWinners,
                                          uint delayBeforeStart,
                                          uint duration,
                                          DareCriteria[] criterions) {
            this.isPrivate = isPrivate;
            this.isForGuild = isForGuild;
            this.isForAlliance = isForAlliance;
            this.needNotifications = needNotifications;
            this.subscriptionFee = subscriptionFee;
            this.jackpot = jackpot;
            this.maxCountWinners = maxCountWinners;
            this.delayBeforeStart = delayBeforeStart;
            this.duration = duration;
            this.criterions = criterions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.isPrivate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.isForGuild);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, this.isForAlliance);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, this.needNotifications);
            writer.WriteByte(flag1);
            writer.WriteInt(this.subscriptionFee);
            writer.WriteInt(this.jackpot);
            writer.WriteUShort(this.maxCountWinners);
            writer.WriteUInt(this.delayBeforeStart);
            writer.WriteUInt(this.duration);
            writer.WriteUShort((ushort) this.criterions.Length);
            foreach (var entry in this.criterions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.isPrivate = BooleanByteWrapper.GetFlag(flag1, 0);
            this.isForGuild = BooleanByteWrapper.GetFlag(flag1, 1);
            this.isForAlliance = BooleanByteWrapper.GetFlag(flag1, 2);
            this.needNotifications = BooleanByteWrapper.GetFlag(flag1, 3);
            this.subscriptionFee = reader.ReadInt();

            if (this.subscriptionFee < 0)
                throw new Exception("Forbidden value on subscriptionFee = " + this.subscriptionFee + ", it doesn't respect the following condition : subscriptionFee < 0");
            this.jackpot = reader.ReadInt();

            if (this.jackpot < 0)
                throw new Exception("Forbidden value on jackpot = " + this.jackpot + ", it doesn't respect the following condition : jackpot < 0");
            this.maxCountWinners = reader.ReadUShort();

            if (this.maxCountWinners < 0 || this.maxCountWinners > 65535)
                throw new Exception("Forbidden value on maxCountWinners = " + this.maxCountWinners + ", it doesn't respect the following condition : maxCountWinners < 0 || maxCountWinners > 65535");
            this.delayBeforeStart = reader.ReadUInt();

            if (this.delayBeforeStart < 0 || this.delayBeforeStart > 4294967295)
                throw new Exception("Forbidden value on delayBeforeStart = " + this.delayBeforeStart + ", it doesn't respect the following condition : delayBeforeStart < 0 || delayBeforeStart > 4294967295");
            this.duration = reader.ReadUInt();

            if (this.duration < 0 || this.duration > 4294967295)
                throw new Exception("Forbidden value on duration = " + this.duration + ", it doesn't respect the following condition : duration < 0 || duration > 4294967295");
            var limit = reader.ReadUShort();
            this.criterions = new DareCriteria[limit];
            for (int i = 0; i < limit; i++) {
                this.criterions[i] = new DareCriteria();
                this.criterions[i].Deserialize(reader);
            }
        }
    }
}