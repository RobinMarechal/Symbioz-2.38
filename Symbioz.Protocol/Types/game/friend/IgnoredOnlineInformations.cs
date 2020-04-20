// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class IgnoredOnlineInformations : IgnoredInformations {
        public const short Id = 105;

        public override short TypeId {
            get { return Id; }
        }

        public ulong playerId;
        public string playerName;
        public sbyte breed;
        public bool sex;


        public IgnoredOnlineInformations() { }

        public IgnoredOnlineInformations(int accountId, string accountName, ulong playerId, string playerName, sbyte breed, bool sex)
            : base(accountId, accountName) {
            this.playerId = playerId;
            this.playerName = playerName;
            this.breed = breed;
            this.sex = sex;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteUTF(this.playerName);
            writer.WriteSByte(this.breed);
            writer.WriteBoolean(this.sex);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            this.playerName = reader.ReadUTF();
            this.breed = reader.ReadSByte();

            if (this.breed < (byte) Enums.PlayableBreedEnum.Feca || this.breed > (byte) Enums.PlayableBreedEnum.Huppermage)
                throw new Exception("Forbidden value on breed = "
                                    + this.breed
                                    + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Huppermage");
            this.sex = reader.ReadBoolean();
        }
    }
}