using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class PrismFightDefenderAddMessage : Message {
        public const ushort Id = 5895;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort subAreaId;
        public ushort fightId;
        public CharacterMinimalPlusLookInformations defender;


        public PrismFightDefenderAddMessage() { }

        public PrismFightDefenderAddMessage(ushort subAreaId, ushort fightId, CharacterMinimalPlusLookInformations defender) {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.defender = defender;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteVarUhShort(this.fightId);
            writer.WriteShort(this.defender.TypeId);
            this.defender.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.fightId = reader.ReadVarUhShort();

            if (this.fightId < 0)
                throw new Exception("Forbidden value on fightId = " + this.fightId + ", it doesn't respect the following condition : fightId < 0");
            this.defender = ProtocolTypeManager.GetInstance<CharacterMinimalPlusLookInformations>(reader.ReadShort());
            this.defender.Deserialize(reader);
        }
    }
}