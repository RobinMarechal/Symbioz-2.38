using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayFightRequestCanceledMessage : Message {
        public const ushort Id = 5822;

        public override ushort MessageId {
            get { return Id; }
        }

        public int fightId;
        public double sourceId;
        public double targetId;


        public GameRolePlayFightRequestCanceledMessage() { }

        public GameRolePlayFightRequestCanceledMessage(int fightId, double sourceId, double targetId) {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.fightId);
            writer.WriteDouble(this.sourceId);
            writer.WriteDouble(this.targetId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.fightId = reader.ReadInt();
            this.sourceId = reader.ReadDouble();

            if (this.sourceId < -9007199254740990 || this.sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + this.sourceId + ", it doesn't respect the following condition : sourceId < -9007199254740990 || sourceId > 9007199254740990");
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
        }
    }
}