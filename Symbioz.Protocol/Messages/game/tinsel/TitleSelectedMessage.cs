using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TitleSelectedMessage : Message {
        public const ushort Id = 6366;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort titleId;


        public TitleSelectedMessage() { }

        public TitleSelectedMessage(ushort titleId) {
            this.titleId = titleId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.titleId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.titleId = reader.ReadVarUhShort();

            if (this.titleId < 0)
                throw new Exception("Forbidden value on titleId = " + this.titleId + ", it doesn't respect the following condition : titleId < 0");
        }
    }
}