using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeObjectTransfertListWithQuantityToInvMessage : Message {
        public const ushort Id = 6470;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint[] ids;
        public uint[] qtys;


        public ExchangeObjectTransfertListWithQuantityToInvMessage() { }

        public ExchangeObjectTransfertListWithQuantityToInvMessage(uint[] ids, uint[] qtys) {
            this.ids = ids;
            this.qtys = qtys;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.ids.Length);
            foreach (var entry in this.ids) {
                writer.WriteVarUhInt(entry);
            }

            writer.WriteUShort((ushort) this.qtys.Length);
            foreach (var entry in this.qtys) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.ids = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.ids[i] = reader.ReadVarUhInt();
            }

            limit = reader.ReadUShort();
            this.qtys = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.qtys[i] = reader.ReadVarUhInt();
            }
        }
    }
}