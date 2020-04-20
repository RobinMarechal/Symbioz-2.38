using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PartyUpdateLightMessage : AbstractPartyEventMessage {
        public const ushort Id = 6054;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong id;
        public uint lifePoints;
        public uint maxLifePoints;
        public ushort prospecting;
        public byte regenRate;


        public PartyUpdateLightMessage() { }

        public PartyUpdateLightMessage(uint partyId, ulong id, uint lifePoints, uint maxLifePoints, ushort prospecting, byte regenRate)
            : base(partyId) {
            this.id = id;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.id);
            writer.WriteVarUhInt(this.lifePoints);
            writer.WriteVarUhInt(this.maxLifePoints);
            writer.WriteVarUhShort(this.prospecting);
            writer.WriteByte(this.regenRate);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.id = reader.ReadVarUhLong();

            if (this.id < 0 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0 || id > 9007199254740990");
            this.lifePoints = reader.ReadVarUhInt();

            if (this.lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + this.lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            this.maxLifePoints = reader.ReadVarUhInt();

            if (this.maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + this.maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            this.prospecting = reader.ReadVarUhShort();

            if (this.prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + this.prospecting + ", it doesn't respect the following condition : prospecting < 0");
            this.regenRate = reader.ReadByte();

            if (this.regenRate < 0 || this.regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + this.regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
        }
    }
}