using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ServerSettingsMessage : Message {
        public const ushort Id = 6340;

        public override ushort MessageId {
            get { return Id; }
        }

        public string lang;
        public sbyte community;
        public sbyte gameType;
        public ushort arenaLeaveBanTime;


        public ServerSettingsMessage() { }

        public ServerSettingsMessage(string lang, sbyte community, sbyte gameType, ushort arenaLeaveBanTime) {
            this.lang = lang;
            this.community = community;
            this.gameType = gameType;
            this.arenaLeaveBanTime = arenaLeaveBanTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.lang);
            writer.WriteSByte(this.community);
            writer.WriteSByte(this.gameType);
            writer.WriteVarUhShort(this.arenaLeaveBanTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.lang = reader.ReadUTF();
            this.community = reader.ReadSByte();

            if (this.community < 0)
                throw new Exception("Forbidden value on community = " + this.community + ", it doesn't respect the following condition : community < 0");
            this.gameType = reader.ReadSByte();
            this.arenaLeaveBanTime = reader.ReadVarUhShort();

            if (this.arenaLeaveBanTime < 0)
                throw new Exception("Forbidden value on arenaLeaveBanTime = " + this.arenaLeaveBanTime + ", it doesn't respect the following condition : arenaLeaveBanTime < 0");
        }
    }
}