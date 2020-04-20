using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildLevelUpMessage : Message {
        public const ushort Id = 6062;

        public override ushort MessageId {
            get { return Id; }
        }

        public byte newLevel;


        public GuildLevelUpMessage() { }

        public GuildLevelUpMessage(byte newLevel) {
            this.newLevel = newLevel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteByte(this.newLevel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.newLevel = reader.ReadByte();

            if (this.newLevel < 2 || this.newLevel > 200)
                throw new Exception("Forbidden value on newLevel = " + this.newLevel + ", it doesn't respect the following condition : newLevel < 2 || newLevel > 200");
        }
    }
}