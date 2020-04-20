using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TextInformationMessage : Message {
        public const ushort Id = 780;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte msgType;
        public ushort msgId;
        public string[] parameters;


        public TextInformationMessage() { }

        public TextInformationMessage(sbyte msgType, ushort msgId, string[] parameters) {
            this.msgType = msgType;
            this.msgId = msgId;
            this.parameters = parameters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.msgType);
            writer.WriteVarUhShort(this.msgId);
            writer.WriteUShort((ushort) this.parameters.Length);
            foreach (var entry in this.parameters) {
                writer.WriteUTF(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.msgType = reader.ReadSByte();

            if (this.msgType < 0)
                throw new Exception("Forbidden value on msgType = " + this.msgType + ", it doesn't respect the following condition : msgType < 0");
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