using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartedWithPodsMessage : ExchangeStartedMessage {
        public const ushort Id = 6129;

        public override ushort MessageId {
            get { return Id; }
        }

        public double firstCharacterId;
        public uint firstCharacterCurrentWeight;
        public uint firstCharacterMaxWeight;
        public double secondCharacterId;
        public uint secondCharacterCurrentWeight;
        public uint secondCharacterMaxWeight;


        public ExchangeStartedWithPodsMessage() { }

        public ExchangeStartedWithPodsMessage(sbyte exchangeType,
                                              double firstCharacterId,
                                              uint firstCharacterCurrentWeight,
                                              uint firstCharacterMaxWeight,
                                              double secondCharacterId,
                                              uint secondCharacterCurrentWeight,
                                              uint secondCharacterMaxWeight)
            : base(exchangeType) {
            this.firstCharacterId = firstCharacterId;
            this.firstCharacterCurrentWeight = firstCharacterCurrentWeight;
            this.firstCharacterMaxWeight = firstCharacterMaxWeight;
            this.secondCharacterId = secondCharacterId;
            this.secondCharacterCurrentWeight = secondCharacterCurrentWeight;
            this.secondCharacterMaxWeight = secondCharacterMaxWeight;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.firstCharacterId);
            writer.WriteVarUhInt(this.firstCharacterCurrentWeight);
            writer.WriteVarUhInt(this.firstCharacterMaxWeight);
            writer.WriteDouble(this.secondCharacterId);
            writer.WriteVarUhInt(this.secondCharacterCurrentWeight);
            writer.WriteVarUhInt(this.secondCharacterMaxWeight);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.firstCharacterId = reader.ReadDouble();

            if (this.firstCharacterId < -9007199254740990 || this.firstCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on firstCharacterId = "
                                    + this.firstCharacterId
                                    + ", it doesn't respect the following condition : firstCharacterId < -9007199254740990 || firstCharacterId > 9007199254740990");
            this.firstCharacterCurrentWeight = reader.ReadVarUhInt();

            if (this.firstCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on firstCharacterCurrentWeight = " + this.firstCharacterCurrentWeight + ", it doesn't respect the following condition : firstCharacterCurrentWeight < 0");
            this.firstCharacterMaxWeight = reader.ReadVarUhInt();

            if (this.firstCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on firstCharacterMaxWeight = " + this.firstCharacterMaxWeight + ", it doesn't respect the following condition : firstCharacterMaxWeight < 0");
            this.secondCharacterId = reader.ReadDouble();

            if (this.secondCharacterId < -9007199254740990 || this.secondCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on secondCharacterId = "
                                    + this.secondCharacterId
                                    + ", it doesn't respect the following condition : secondCharacterId < -9007199254740990 || secondCharacterId > 9007199254740990");
            this.secondCharacterCurrentWeight = reader.ReadVarUhInt();

            if (this.secondCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on secondCharacterCurrentWeight = " + this.secondCharacterCurrentWeight + ", it doesn't respect the following condition : secondCharacterCurrentWeight < 0");
            this.secondCharacterMaxWeight = reader.ReadVarUhInt();

            if (this.secondCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on secondCharacterMaxWeight = " + this.secondCharacterMaxWeight + ", it doesn't respect the following condition : secondCharacterMaxWeight < 0");
        }
    }
}