using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SystemMessageDisplayMessage : Message {
        public const ushort Id = 189;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool hangUp;
        public ushort msgId;
        public string[] parameters;


        public SystemMessageDisplayMessage() { }

        public SystemMessageDisplayMessage(bool hangUp, ushort msgId, string[] parameters) {
            this.hangUp = hangUp;
            this.msgId = msgId;
            this.parameters = parameters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.hangUp);
            writer.WriteVarUhShort(this.msgId);
            writer.WriteUShort((ushort) this.parameters.Length);
            foreach (var entry in this.parameters) {
                writer.WriteUTF(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.hangUp = reader.ReadBoolean();
            this.msgId = reader.ReadVarUhShort();

            if (this.msgId < 0)
                throw new Exception("Forbidden value on msgId = " + this.msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            this.parameters = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.parameters[i] = reader.ReadUTF();
            }
        }
    }
}