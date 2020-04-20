using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkMountWithOutPaddockMessage : Message {
        public const ushort Id = 5991;

        public override ushort MessageId {
            get { return Id; }
        }

        public MountClientData[] stabledMountsDescription;


        public ExchangeStartOkMountWithOutPaddockMessage() { }

        public ExchangeStartOkMountWithOutPaddockMessage(MountClientData[] stabledMountsDescription) {
            this.stabledMountsDescription = stabledMountsDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.stabledMountsDescription.Length);
            foreach (var entry in this.stabledMountsDescription) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.stabledMountsDescription = new MountClientData[limit];
            for (int i = 0; i < limit; i++) {
                this.stabledMountsDescription[i] = new MountClientData();
                this.stabledMountsDescription[i].Deserialize(reader);
            }
        }
    }
}