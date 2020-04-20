using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeBidHouseInListAddedMessage : Message {
        public const ushort Id = 5949;

        public override ushort MessageId {
            get { return Id; }
        }

        public int itemUID;
        public int objGenericId;
        public ObjectEffect[] effects;
        public uint[] prices;


        public ExchangeBidHouseInListAddedMessage() { }

        public ExchangeBidHouseInListAddedMessage(int itemUID, int objGenericId, ObjectEffect[] effects, uint[] prices) {
            this.itemUID = itemUID;
            this.objGenericId = objGenericId;
            this.effects = effects;
            this.prices = prices;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.itemUID);
            writer.WriteInt(this.objGenericId);
            writer.WriteUShort((ushort) this.effects.Length);
            foreach (var entry in this.effects) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.prices.Length);
            foreach (var entry in this.prices) {
                writer.WriteVarUhInt(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.itemUID = reader.ReadInt();
            this.objGenericId = reader.ReadInt();
            var limit = reader.ReadUShort();
            this.effects = new ObjectEffect[limit];
            for (int i = 0; i < limit; i++) {
                this.effects[i] = ProtocolTypeManager.GetInstance<ObjectEffect>(reader.ReadShort());
                this.effects[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.prices = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.prices[i] = reader.ReadVarUhInt();
            }
        }
    }
}