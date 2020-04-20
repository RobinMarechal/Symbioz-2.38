using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionAcknowledgementMessage : Message {
        public const ushort Id = 957;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool valid;
        public sbyte actionId;


        public GameActionAcknowledgementMessage() { }

        public GameActionAcknowledgementMessage(bool valid, sbyte actionId) {
            this.valid = valid;
            this.actionId = actionId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.valid);
            writer.WriteSByte(this.actionId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.valid = reader.ReadBoolean();
            this.actionId = reader.ReadSByte();
        }
    }
}