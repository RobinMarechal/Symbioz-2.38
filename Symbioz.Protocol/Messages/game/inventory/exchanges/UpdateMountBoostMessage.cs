using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class UpdateMountBoostMessage : Message {
        public const ushort Id = 6179;

        public override ushort MessageId {
            get { return Id; }
        }

        public int rideId;
        public UpdateMountBoost[] boostToUpdateList;


        public UpdateMountBoostMessage() { }

        public UpdateMountBoostMessage(int rideId, UpdateMountBoost[] boostToUpdateList) {
            this.rideId = rideId;
            this.boostToUpdateList = boostToUpdateList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarInt(this.rideId);
            writer.WriteUShort((ushort) this.boostToUpdateList.Length);
            foreach (var entry in this.boostToUpdateList) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.rideId = reader.ReadVarInt();
            var limit = reader.ReadUShort();
            this.boostToUpdateList = new UpdateMountBoost[limit];
            for (int i = 0; i < limit; i++) {
                this.boostToUpdateList[i] = ProtocolTypeManager.GetInstance<UpdateMountBoost>(reader.ReadShort());
                this.boostToUpdateList[i].Deserialize(reader);
            }
        }
    }
}