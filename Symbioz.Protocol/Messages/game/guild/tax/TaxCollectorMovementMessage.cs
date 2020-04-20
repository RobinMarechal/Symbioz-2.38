using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TaxCollectorMovementMessage : Message {
        public const ushort Id = 5633;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte movementType;
        public TaxCollectorBasicInformations basicInfos;
        public ulong playerId;
        public string playerName;


        public TaxCollectorMovementMessage() { }

        public TaxCollectorMovementMessage(sbyte movementType, TaxCollectorBasicInformations basicInfos, ulong playerId, string playerName) {
            this.movementType = movementType;
            this.basicInfos = basicInfos;
            this.playerId = playerId;
            this.playerName = playerName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.movementType);
            this.basicInfos.Serialize(writer);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteUTF(this.playerName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.movementType = reader.ReadSByte();

            if (this.movementType < 0)
                throw new Exception("Forbidden value on movementType = " + this.movementType + ", it doesn't respect the following condition : movementType < 0");
            this.basicInfos = new TaxCollectorBasicInformations();
            this.basicInfos.Deserialize(reader);
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.playerName = reader.ReadUTF();
        }
    }
}