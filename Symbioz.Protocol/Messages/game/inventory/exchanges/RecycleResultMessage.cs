using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class RecycleResultMessage : Message {
        public const ushort Id = 6601;

        public override ushort MessageId {
            get { return Id; }
        }

        public uint nuggetsForPrism;
        public uint nuggetsForPlayer;


        public RecycleResultMessage() { }

        public RecycleResultMessage(uint nuggetsForPrism, uint nuggetsForPlayer) {
            this.nuggetsForPrism = nuggetsForPrism;
            this.nuggetsForPlayer = nuggetsForPlayer;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhInt(this.nuggetsForPrism);
            writer.WriteVarUhInt(this.nuggetsForPlayer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.nuggetsForPrism = reader.ReadVarUhInt();

            if (this.nuggetsForPrism < 0)
                throw new Exception("Forbidden value on nuggetsForPrism = " + this.nuggetsForPrism + ", it doesn't respect the following condition : nuggetsForPrism < 0");
            this.nuggetsForPlayer = reader.ReadVarUhInt();

            if (this.nuggetsForPlayer < 0)
                throw new Exception("Forbidden value on nuggetsForPlayer = " + this.nuggetsForPlayer + ", it doesn't respect the following condition : nuggetsForPlayer < 0");
        }
    }
}