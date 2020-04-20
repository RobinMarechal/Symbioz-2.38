using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildChangeMemberParametersMessage : Message {
        public const ushort Id = 5549;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong memberId;
        public ushort rank;
        public sbyte experienceGivenPercent;
        public uint rights;


        public GuildChangeMemberParametersMessage() { }

        public GuildChangeMemberParametersMessage(ulong memberId, ushort rank, sbyte experienceGivenPercent, uint rights) {
            this.memberId = memberId;
            this.rank = rank;
            this.experienceGivenPercent = experienceGivenPercent;
            this.rights = rights;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.memberId);
            writer.WriteVarUhShort(this.rank);
            writer.WriteSByte(this.experienceGivenPercent);
            writer.WriteVarUhInt(this.rights);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.memberId = reader.ReadVarUhLong();

            if (this.memberId < 0 || this.memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + this.memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            this.rank = reader.ReadVarUhShort();

            if (this.rank < 0)
                throw new Exception("Forbidden value on rank = " + this.rank + ", it doesn't respect the following condition : rank < 0");
            this.experienceGivenPercent = reader.ReadSByte();

            if (this.experienceGivenPercent < 0 || this.experienceGivenPercent > 100)
                throw new Exception("Forbidden value on experienceGivenPercent = " + this.experienceGivenPercent + ", it doesn't respect the following condition : experienceGivenPercent < 0 || experienceGivenPercent > 100");
            this.rights = reader.ReadVarUhInt();

            if (this.rights < 0)
                throw new Exception("Forbidden value on rights = " + this.rights + ", it doesn't respect the following condition : rights < 0");
        }
    }
}