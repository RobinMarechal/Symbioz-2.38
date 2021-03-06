using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkJobIndexMessage : Message {
        public const ushort Id = 5819;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint[] jobs;


        public ExchangeStartOkJobIndexMessage() { }

        public ExchangeStartOkJobIndexMessage(uint[] jobs) {
            this.jobs = jobs;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.jobs.Length);
            foreach (var entry in this.jobs) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.jobs = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.jobs[i] = reader.ReadVarUhInt();
            }
        }
    }
}