using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextCreateMessage : Message {
        public const ushort Id = 200;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte context;


        public GameContextCreateMessage() { }

        public GameContextCreateMessage(sbyte context) {
            this.context = context;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.context);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.context = reader.ReadSByte();

            if (this.context < 0)
                throw new Exception("Forbidden value on context = " + this.context + ", it doesn't respect the following condition : context < 0");
        }
    }
}