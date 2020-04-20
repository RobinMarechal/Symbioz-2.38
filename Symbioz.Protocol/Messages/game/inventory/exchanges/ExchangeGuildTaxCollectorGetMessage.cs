using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ExchangeGuildTaxCollectorGetMessage : Message {
        public const ushort Id = 5762;

        public override ushort MessageId {
            get { return Id; }
        }

        public string collectorName;
        public short worldX;
        public short worldY;
        public int mapId;
        public ushort subAreaId;
        public string userName;
        public ulong callerId;
        public string callerName;
        public double experience;
        public ushort pods;
        public ObjectItemGenericQuantity[] objectsInfos;


        public ExchangeGuildTaxCollectorGetMessage() { }

        public ExchangeGuildTaxCollectorGetMessage(string collectorName,
                                                   short worldX,
                                                   short worldY,
                                                   int mapId,
                                                   ushort subAreaId,
                                                   string userName,
                                                   ulong callerId,
                                                   string callerName,
                                                   double experience,
                                                   ushort pods,
                                                   ObjectItemGenericQuantity[] objectsInfos) {
            this.collectorName = collectorName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.userName = userName;
            this.callerId = callerId;
            this.callerName = callerName;
            this.experience = experience;
            this.pods = pods;
            this.objectsInfos = objectsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.collectorName);
            writer.WriteShort(this.worldX);
            writer.WriteShort(this.worldY);
            writer.WriteInt(this.mapId);
            writer.WriteVarUhShort(this.subAreaId);
            writer.WriteUTF(this.userName);
            writer.WriteVarUhLong(this.callerId);
            writer.WriteUTF(this.callerName);
            writer.WriteDouble(this.experience);
            writer.WriteVarUhShort(this.pods);
            writer.WriteUShort((ushort) this.objectsInfos.Length);
            foreach (var entry in this.objectsInfos) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.collectorName = reader.ReadUTF();
            this.worldX = reader.ReadShort();

            if (this.worldX < -255 || this.worldX > 255)
                throw new Exception("Forbidden value on worldX = " + this.worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            this.worldY = reader.ReadShort();

            if (this.worldY < -255 || this.worldY > 255)
                throw new Exception("Forbidden value on worldY = " + this.worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            this.mapId = reader.ReadInt();
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            this.userName = reader.ReadUTF();
            this.callerId = reader.ReadVarUhLong();

            if (this.callerId < 0 || this.callerId > 9007199254740990)
                throw new Exception("Forbidden value on callerId = " + this.callerId + ", it doesn't respect the following condition : callerId < 0 || callerId > 9007199254740990");
            this.callerName = reader.ReadUTF();
            this.experience = reader.ReadDouble();

            if (this.experience < -9007199254740990 || this.experience > 9007199254740990)
                throw new Exception("Forbidden value on experience = " + this.experience + ", it doesn't respect the following condition : experience < -9007199254740990 || experience > 9007199254740990");
            this.pods = reader.ReadVarUhShort();

            if (this.pods < 0)
                throw new Exception("Forbidden value on pods = " + this.pods + ", it doesn't respect the following condition : pods < 0");
            var limit = reader.ReadUShort();
            this.objectsInfos = new ObjectItemGenericQuantity[limit];
            for (int i = 0; i < limit; i++) {
                this.objectsInfos[i] = new ObjectItemGenericQuantity();
                this.objectsInfos[i].Deserialize(reader);
            }
        }
    }
}