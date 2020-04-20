using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionBasicMessage {
        public const ushort Id = 5615;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort maxPods;
        public ushort prospecting;
        public ushort wisdom;
        public sbyte taxCollectorsCount;
        public int taxCollectorAttack;
        public uint kamas;
        public ulong experience;
        public uint pods;
        public uint itemsValue;


        public TaxCollectorDialogQuestionExtendedMessage() { }

        public TaxCollectorDialogQuestionExtendedMessage(BasicGuildInformations guildInfo,
                                                         ushort maxPods,
                                                         ushort prospecting,
                                                         ushort wisdom,
                                                         sbyte taxCollectorsCount,
                                                         int taxCollectorAttack,
                                                         uint kamas,
                                                         ulong experience,
                                                         uint pods,
                                                         uint itemsValue)
            : base(guildInfo) {
            this.maxPods = maxPods;
            this.prospecting = prospecting;
            this.wisdom = wisdom;
            this.taxCollectorsCount = taxCollectorsCount;
            this.taxCollectorAttack = taxCollectorAttack;
            this.kamas = kamas;
            this.experience = experience;
            this.pods = pods;
            this.itemsValue = itemsValue;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.maxPods);
            writer.WriteVarUhShort(this.prospecting);
            writer.WriteVarUhShort(this.wisdom);
            writer.WriteSByte(this.taxCollectorsCount);
            writer.WriteInt(this.taxCollectorAttack);
            writer.WriteVarUhInt(this.kamas);
            writer.WriteVarUhLong(this.experience);
            writer.WriteVarUhInt(this.pods);
            writer.WriteVarUhInt(this.itemsValue);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.maxPods = reader.ReadVarUhShort();

            if (this.maxPods < 0)
                throw new Exception("Forbidden value on maxPods = " + this.maxPods + ", it doesn't respect the following condition : maxPods < 0");
            this.prospecting = reader.ReadVarUhShort();

            if (this.prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + this.prospecting + ", it doesn't respect the following condition : prospecting < 0");
            this.wisdom = reader.ReadVarUhShort();

            if (this.wisdom < 0)
                throw new Exception("Forbidden value on wisdom = " + this.wisdom + ", it doesn't respect the following condition : wisdom < 0");
            this.taxCollectorsCount = reader.ReadSByte();

            if (this.taxCollectorsCount < 0)
                throw new Exception("Forbidden value on taxCollectorsCount = " + this.taxCollectorsCount + ", it doesn't respect the following condition : taxCollectorsCount < 0");
            this.taxCollectorAttack = reader.ReadInt();
            this.kamas = reader.ReadVarUhInt();

            if (this.kamas < 0)
                throw new Exception("Forbidden value on kamas = " + this.kamas + ", it doesn't respect the following condition : kamas < 0");
            this.experience = reader.ReadVarUhLong();

            if (this.experience < 0 || this.experience > 9007199254740990)
                throw new Exception("Forbidden value on experience = " + this.experience + ", it doesn't respect the following condition : experience < 0 || experience > 9007199254740990");
            this.pods = reader.ReadVarUhInt();

            if (this.pods < 0)
                throw new Exception("Forbidden value on pods = " + this.pods + ", it doesn't respect the following condition : pods < 0");
            this.itemsValue = reader.ReadVarUhInt();

            if (this.itemsValue < 0)
                throw new Exception("Forbidden value on itemsValue = " + this.itemsValue + ", it doesn't respect the following condition : itemsValue < 0");
        }
    }
}