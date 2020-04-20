using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobBookSubscribeRequestMessage : Message {
        public const ushort Id = 6592;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte[] jobIds;


        public JobBookSubscribeRequestMessage() { }

        public JobBookSubscribeRequestMessage(sbyte[] jobIds) {
            this.jobIds = jobIds;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.jobIds.Length);
            foreach (var entry in this.jobIds) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.jobIds = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.jobIds[i] = reader.ReadSByte();
            }
        }
    }
}