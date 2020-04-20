using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage {
        public const ushort Id = 5747;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong playerId;
        public ushort[] skills;


        public JobMultiCraftAvailableSkillsMessage() { }

        public JobMultiCraftAvailableSkillsMessage(bool enabled, ulong playerId, ushort[] skills)
            : base(enabled) {
            this.playerId = playerId;
            this.skills = skills;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.playerId);
            writer.WriteUShort((ushort) this.skills.Length);
            foreach (var entry in this.skills) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            var limit = reader.ReadUShort();
            this.skills = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.skills[i] = reader.ReadVarUhShort();
            }
        }
    }
}