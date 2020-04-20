using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayTaxCollectorFightRequestMessage : Message {
        public const ushort Id = 5954;

        public override ushort MessageId {
            get { return Id; }
        }

        public int taxCollectorId;


        public GameRolePlayTaxCollectorFightRequestMessage() { }

        public GameRolePlayTaxCollectorFightRequestMessage(int taxCollectorId) {
            this.taxCollectorId = taxCollectorId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.taxCollectorId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.taxCollectorId = reader.ReadInt();
        }
    }
}