using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayPlayerLifeStatusMessage : Message {
        public const ushort Id = 5996;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte state;
        public int phenixMapId;


        public GameRolePlayPlayerLifeStatusMessage() { }

        public GameRolePlayPlayerLifeStatusMessage(sbyte state, int phenixMapId) {
            this.state = state;
            this.phenixMapId = phenixMapId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.state);
            writer.WriteInt(this.phenixMapId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.state = reader.ReadSByte();

            if (this.state < 0)
                throw new Exception("Forbidden value on state = " + this.state + ", it doesn't respect the following condition : state < 0");
            this.phenixMapId = reader.ReadInt();

            if (this.phenixMapId < 0)
                throw new Exception("Forbidden value on phenixMapId = " + this.phenixMapId + ", it doesn't respect the following condition : phenixMapId < 0");
        }
    }
}