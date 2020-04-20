using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EntityTalkMessage : Message {
        public const ushort Id = 6110;

        public override ushort MessageId {
            get { return Id; }
        }

        public double entityId;
        public ushort textId;
        public string[] parameters;


        public EntityTalkMessage() { }

        public EntityTalkMessage(double entityId, ushort textId, string[] parameters) {
            this.entityId = entityId;
            this.textId = textId;
            this.parameters = parameters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.entityId);
            writer.WriteVarUhShort(this.textId);
            writer.WriteUShort((ushort) this.parameters.Length);
            foreach (var entry in this.parameters) {
                writer.WriteUTF(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.entityId = reader.ReadDouble();

            if (this.entityId < -9007199254740990 || this.entityId > 9007199254740990)
                throw new Exception("Forbidden value on entityId = " + this.entityId + ", it doesn't respect the following condition : entityId < -9007199254740990 || entityId > 9007199254740990");
            this.textId = reader.ReadVarUhShort();

            if (this.textId < 0)
                throw new Exception("Forbidden value on textId = " + this.textId + ", it doesn't respect the following condition : textId < 0");
            var limit = reader.ReadUShort();
            this.parameters = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.parameters[i] = reader.ReadUTF();
            }
        }
    }
}