using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectFoundWhileRecoltingMessage : Message {
        public const ushort Id = 6017;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort genericId;
        public uint quantity;
        public uint resourceGenericId;


        public ObjectFoundWhileRecoltingMessage() { }

        public ObjectFoundWhileRecoltingMessage(ushort genericId, uint quantity, uint resourceGenericId) {
            this.genericId = genericId;
            this.quantity = quantity;
            this.resourceGenericId = resourceGenericId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.genericId);
            writer.WriteVarUhInt(this.quantity);
            writer.WriteVarUhInt(this.resourceGenericId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.genericId = reader.ReadVarUhShort();

            if (this.genericId < 0)
                throw new Exception("Forbidden value on genericId = " + this.genericId + ", it doesn't respect the following condition : genericId < 0");
            this.quantity = reader.ReadVarUhInt();

            if (this.quantity < 0)
                throw new Exception("Forbidden value on quantity = " + this.quantity + ", it doesn't respect the following condition : quantity < 0");
            this.resourceGenericId = reader.ReadVarUhInt();

            if (this.resourceGenericId < 0)
                throw new Exception("Forbidden value on resourceGenericId = " + this.resourceGenericId + ", it doesn't respect the following condition : resourceGenericId < 0");
        }
    }
}