using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class NotificationByServerMessage : Message {
        public const ushort Id = 6103;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort id;
        public string[] parameters;
        public bool forceOpen;


        public NotificationByServerMessage() { }

        public NotificationByServerMessage(ushort id, string[] parameters, bool forceOpen) {
            this.id = id;
            this.parameters = parameters;
            this.forceOpen = forceOpen;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.id);
            writer.WriteUShort((ushort) this.parameters.Length);
            foreach (var entry in this.parameters) {
                writer.WriteUTF(entry);
            }

            writer.WriteBoolean(this.forceOpen);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadVarUhShort();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            this.parameters = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.parameters[i] = reader.ReadUTF();
            }

            this.forceOpen = reader.ReadBoolean();
        }
    }
}