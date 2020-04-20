using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage {
        public const ushort Id = 5979;

        public override ushort MessageId {
            get { return Id; }
        }

        public MountClientData[] paddockedMountsDescription;


        public ExchangeStartOkMountMessage() { }

        public ExchangeStartOkMountMessage(MountClientData[] stabledMountsDescription, MountClientData[] paddockedMountsDescription)
            : base(stabledMountsDescription) {
            this.paddockedMountsDescription = paddockedMountsDescription;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.paddockedMountsDescription.Length);
            foreach (var entry in this.paddockedMountsDescription) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.paddockedMountsDescription = new MountClientData[limit];
            for (int i = 0; i < limit; i++) {
                this.paddockedMountsDescription[i] = new MountClientData();
                this.paddockedMountsDescription[i].Deserialize(reader);
            }
        }
    }
}