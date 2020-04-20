using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeStartOkNpcShopMessage : Message {
        public const ushort Id = 5761;

        public override ushort MessageId {
            get { return Id; }
        }

        public double npcSellerId;
        public ushort tokenId;
        public ObjectItemToSellInNpcShop[] objectsInfos;


        public ExchangeStartOkNpcShopMessage() { }

        public ExchangeStartOkNpcShopMessage(double npcSellerId, ushort tokenId, ObjectItemToSellInNpcShop[] objectsInfos) {
            this.npcSellerId = npcSellerId;
            this.tokenId = tokenId;
            this.objectsInfos = objectsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.npcSellerId);
            writer.WriteVarUhShort(this.tokenId);
            writer.WriteUShort((ushort) this.objectsInfos.Length);
            foreach (var entry in this.objectsInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.npcSellerId = reader.ReadDouble();

            if (this.npcSellerId < -9007199254740990 || this.npcSellerId > 9007199254740990)
                throw new Exception("Forbidden value on npcSellerId = " + this.npcSellerId + ", it doesn't respect the following condition : npcSellerId < -9007199254740990 || npcSellerId > 9007199254740990");
            this.tokenId = reader.ReadVarUhShort();

            if (this.tokenId < 0)
                throw new Exception("Forbidden value on tokenId = " + this.tokenId + ", it doesn't respect the following condition : tokenId < 0");
            var limit = reader.ReadUShort();
            this.objectsInfos = new ObjectItemToSellInNpcShop[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsInfos[i] = new ObjectItemToSellInNpcShop();
                this.objectsInfos[i].Deserialize(reader);
            }
        }
    }
}