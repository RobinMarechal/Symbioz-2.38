using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameDataPlayFarmObjectAnimationMessage : Message {
        public const ushort Id = 6026;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] cellId;


        public GameDataPlayFarmObjectAnimationMessage() { }

        public GameDataPlayFarmObjectAnimationMessage(ushort[] cellId) {
            this.cellId = cellId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.cellId.Length);
            foreach (var entry in this.cellId) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.cellId = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.cellId[i] = reader.ReadVarUhShort();
            }
        }
    }
}