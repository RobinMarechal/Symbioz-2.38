using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayDelayedObjectUseMessage : GameRolePlayDelayedActionMessage {
        public const ushort Id = 6425;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort objectGID;


        public GameRolePlayDelayedObjectUseMessage() { }

        public GameRolePlayDelayedObjectUseMessage(double delayedCharacterId, sbyte delayTypeId, double delayEndTime, ushort objectGID)
            : base(delayedCharacterId, delayTypeId, delayEndTime) {
            this.objectGID = objectGID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.objectGID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.objectGID = reader.ReadVarUhShort();

            if (this.objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + this.objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
    }
}