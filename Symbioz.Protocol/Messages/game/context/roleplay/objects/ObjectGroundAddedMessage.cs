using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ObjectGroundAddedMessage : Message {
        public const ushort Id = 3017;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort cellId;
        public ushort objectGID;


        public ObjectGroundAddedMessage() { }

        public ObjectGroundAddedMessage(ushort cellId, ushort objectGID) {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.cellId);
            writer.WriteVarUhShort(this.objectGID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.cellId = reader.ReadVarUhShort();

            if (this.cellId < 0 || this.cellId > 559)
                throw new Exception("Forbidden value on cellId = " + this.cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            this.objectGID = reader.ReadVarUhShort();

            if (this.objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + this.objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
    }
}