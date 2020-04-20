using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class DareSubscribedListMessage : Message {
        public const ushort Id = 6658;

        public override ushort MessageId {
            get { return Id; }
        }

        public DareInformations[] daresFixedInfos;
        public DareVersatileInformations[] daresVersatilesInfos;


        public DareSubscribedListMessage() { }

        public DareSubscribedListMessage(DareInformations[] daresFixedInfos, DareVersatileInformations[] daresVersatilesInfos) {
            this.daresFixedInfos = daresFixedInfos;
            this.daresVersatilesInfos = daresVersatilesInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.daresFixedInfos.Length);
            foreach (var entry in this.daresFixedInfos) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.daresVersatilesInfos.Length);
            foreach (var entry in this.daresVersatilesInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.daresFixedInfos = new DareInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.daresFixedInfos[i] = new DareInformations();
                this.daresFixedInfos[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.daresVersatilesInfos = new DareVersatileInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.daresVersatilesInfos[i] = new DareVersatileInformations();
                this.daresVersatilesInfos[i].Deserialize(reader);
            }
        }
    }
}