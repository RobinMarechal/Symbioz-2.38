using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LivingObjectMessageRequestMessage : Message {
        public const ushort Id = 6066;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort msgId;
        public string[] parameters;
        public uint livingObject;


        public LivingObjectMessageRequestMessage() { }

        public LivingObjectMessageRequestMessage(ushort msgId, string[] parameters, uint livingObject) {
            this.msgId = msgId;
            this.parameters = parameters;
            this.livingObject = livingObject;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.msgId);
            writer.WriteUShort((ushort) this.parameters.Length);
            foreach (var entry in this.parameters) {
                writer.WriteUTF(entry);
            }

            writer.WriteVarUhInt(this.livingObject);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.msgId = reader.ReadVarUhShort();

            if (this.msgId < 0)
                throw new Exception("Forbidden value on msgId = " + this.msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            this.parameters = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.parameters[i] = reader.ReadUTF();
            }

            this.livingObject = reader.ReadVarUhInt();

            if (this.livingObject < 0)
                throw new Exception("Forbidden value on livingObject = " + this.livingObject + ", it doesn't respect the following condition : livingObject < 0");
        }
    }
}