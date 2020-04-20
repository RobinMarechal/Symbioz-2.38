using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CinematicMessage : Message {
        public const ushort Id = 6053;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort cinematicId;


        public CinematicMessage() { }

        public CinematicMessage(ushort cinematicId) {
            this.cinematicId = cinematicId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.cinematicId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.cinematicId = reader.ReadVarUhShort();

            if (this.cinematicId < 0)
                throw new Exception("Forbidden value on cinematicId = " + this.cinematicId + ", it doesn't respect the following condition : cinematicId < 0");
        }
    }
}