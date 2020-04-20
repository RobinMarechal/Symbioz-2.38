using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismFightDefenderLeaveMessage : Message {
        public const ushort Id = 5892;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;
        public ushort fightId;
        public ulong fighterToRemoveId;


        public PrismFightDefenderLeaveMessage() { }

        public PrismFightDefenderLeaveMessage(ushort subAreaId, ushort fightId, ulong fighterToRemoveId) {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteVarUhShort(this.fightId);
            writer.WriteVarUhLong(this.fighterToRemoveId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.fightId = reader.ReadVarUhShort();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.fighterToRemoveId = reader.ReadVarUhLong();

            if (this.fighterToRemoveId < 0 || this.fighterToRemoveId > 9007199254740990)
                throw new Exception("Forbidden value on fighterToRemoveId = " + this.fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0 || fighterToRemoveId > 9007199254740990");
        }
    }
}