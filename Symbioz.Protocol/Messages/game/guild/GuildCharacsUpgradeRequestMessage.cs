using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildCharacsUpgradeRequestMessage : Message {
        public const ushort Id = 5706;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte charaTypeTarget;


        public GuildCharacsUpgradeRequestMessage() { }

        public GuildCharacsUpgradeRequestMessage(sbyte charaTypeTarget) {
            this.charaTypeTarget = charaTypeTarget;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.charaTypeTarget);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.charaTypeTarget = reader.ReadSByte();

            if (this.charaTypeTarget < 0)
                throw new Exception("Forbidden value on charaTypeTarget = " + this.charaTypeTarget + ", it doesn't respect the following condition : charaTypeTarget < 0");
        }
    }
}