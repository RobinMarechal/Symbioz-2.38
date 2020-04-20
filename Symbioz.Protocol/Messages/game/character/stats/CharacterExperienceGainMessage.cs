using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterExperienceGainMessage : Message {
        public const ushort Id = 6321;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong experienceCharacter;
        public ulong experienceMount;
        public ulong experienceGuild;
        public ulong experienceIncarnation;


        public CharacterExperienceGainMessage() { }

        public CharacterExperienceGainMessage(ulong experienceCharacter, ulong experienceMount, ulong experienceGuild, ulong experienceIncarnation) {
            this.experienceCharacter = experienceCharacter;
            this.experienceMount = experienceMount;
            this.experienceGuild = experienceGuild;
            this.experienceIncarnation = experienceIncarnation;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhLong(this.experienceCharacter);
            writer.WriteVarUhLong(this.experienceMount);
            writer.WriteVarUhLong(this.experienceGuild);
            writer.WriteVarUhLong(this.experienceIncarnation);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.experienceCharacter = reader.ReadVarUhLong();

            if (this.experienceCharacter < 0 || this.experienceCharacter > 9007199254740990)
                throw new Exception("Forbidden value on experienceCharacter = " + this.experienceCharacter + ", it doesn't respect the following condition : experienceCharacter < 0 || experienceCharacter > 9007199254740990");
            this.experienceMount = reader.ReadVarUhLong();

            if (this.experienceMount < 0 || this.experienceMount > 9007199254740990)
                throw new Exception("Forbidden value on experienceMount = " + this.experienceMount + ", it doesn't respect the following condition : experienceMount < 0 || experienceMount > 9007199254740990");
            this.experienceGuild = reader.ReadVarUhLong();

            if (this.experienceGuild < 0 || this.experienceGuild > 9007199254740990)
                throw new Exception("Forbidden value on experienceGuild = " + this.experienceGuild + ", it doesn't respect the following condition : experienceGuild < 0 || experienceGuild > 9007199254740990");
            this.experienceIncarnation = reader.ReadVarUhLong();

            if (this.experienceIncarnation < 0 || this.experienceIncarnation > 9007199254740990)
                throw new Exception("Forbidden value on experienceIncarnation = "
                                    + this.experienceIncarnation
                                    + ", it doesn't respect the following condition : experienceIncarnation < 0 || experienceIncarnation > 9007199254740990");
        }
    }
}