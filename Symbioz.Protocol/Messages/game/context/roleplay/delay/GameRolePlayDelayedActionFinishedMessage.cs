using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayDelayedActionFinishedMessage : Message {
        public const ushort Id = 6150;

        public override ushort MessageId {
            get { return Id; }
        }

        public double delayedCharacterId;
        public sbyte delayTypeId;


        public GameRolePlayDelayedActionFinishedMessage() { }

        public GameRolePlayDelayedActionFinishedMessage(double delayedCharacterId, sbyte delayTypeId) {
            this.delayedCharacterId = delayedCharacterId;
            this.delayTypeId = delayTypeId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.delayedCharacterId);
            writer.WriteSByte(this.delayTypeId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.delayedCharacterId = reader.ReadDouble();

            if (this.delayedCharacterId < -9007199254740990 || this.delayedCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on delayedCharacterId = "
                                    + this.delayedCharacterId
                                    + ", it doesn't respect the following condition : delayedCharacterId < -9007199254740990 || delayedCharacterId > 9007199254740990");
            this.delayTypeId = reader.ReadSByte();

            if (this.delayTypeId < 0)
                throw new Exception("Forbidden value on delayTypeId = " + this.delayTypeId + ", it doesn't respect the following condition : delayTypeId < 0");
        }
    }
}