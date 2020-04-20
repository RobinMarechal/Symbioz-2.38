using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SetUpdateMessage : Message {
        public const ushort Id = 5503;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort setId;
        public ushort[] setObjects;
        public ObjectEffect[] setEffects;


        public SetUpdateMessage() { }

        public SetUpdateMessage(ushort setId, ushort[] setObjects, ObjectEffect[] setEffects) {
            this.setId = setId;
            this.setObjects = setObjects;
            this.setEffects = setEffects;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.setId);
            writer.WriteUShort((ushort) this.setObjects.Length);
            foreach (var entry in this.setObjects) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.setEffects.Length);
            foreach (var entry in this.setEffects) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.setId = reader.ReadVarUhShort();

            if (this.setId < 0)
                throw new Exception("Forbidden value on setId = " + this.setId + ", it doesn't respect the following condition : setId < 0");
            var limit = reader.ReadUShort();
            this.setObjects = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.setObjects[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.setEffects = new ObjectEffect[limit];
            for (int i = 0; i < limit; i++) {
                this.setEffects[i] = ProtocolTypeManager.GetInstance<ObjectEffect>(reader.ReadShort());
                this.setEffects[i].Deserialize(reader);
            }
        }
    }
}