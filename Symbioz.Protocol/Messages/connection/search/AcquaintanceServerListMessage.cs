using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AcquaintanceServerListMessage : Message {
        public const ushort Id = 6142;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] servers;


        public AcquaintanceServerListMessage() { }

        public AcquaintanceServerListMessage(ushort[] servers) {
            this.servers = servers;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.servers.Length);
            foreach (var entry in this.servers) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.servers = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.servers[i] = reader.ReadVarUhShort();
            }
        }
    }
}