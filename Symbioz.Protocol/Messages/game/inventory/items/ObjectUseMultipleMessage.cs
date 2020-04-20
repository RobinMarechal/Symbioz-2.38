using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectUseMultipleMessage : ObjectUseMessage {
        public const ushort Id = 6234;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint quantity;


        public ObjectUseMultipleMessage() { }

        public ObjectUseMultipleMessage(uint objectUID, uint quantity)
            : base(objectUID) {
            this.quantity = quantity;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhInt(this.quantity);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
        }
    }
}