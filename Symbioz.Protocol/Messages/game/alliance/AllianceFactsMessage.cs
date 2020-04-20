using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceFactsMessage : Message {
        public const ushort Id = 6414;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceFactSheetInformations infos;
        public GuildInAllianceInformations[] guilds;
        public ushort[] controlledSubareaIds;
        public ulong leaderCharacterId;
        public string leaderCharacterName;


        public AllianceFactsMessage() { }

        public AllianceFactsMessage(AllianceFactSheetInformations infos, GuildInAllianceInformations[] guilds, ushort[] controlledSubareaIds, ulong leaderCharacterId, string leaderCharacterName) {
            this.infos = infos;
            this.guilds = guilds;
            this.controlledSubareaIds = controlledSubareaIds;
            this.leaderCharacterId = leaderCharacterId;
            this.leaderCharacterName = leaderCharacterName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.infos.TypeId);
            this.infos.Serialize(writer);
            writer.WriteUShort((ushort) this.guilds.Length);
            foreach (var entry in this.guilds) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.controlledSubareaIds.Length);
            foreach (var entry in this.controlledSubareaIds) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteVarUhLong(this.leaderCharacterId);
            writer.WriteUTF(this.leaderCharacterName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.infos = ProtocolTypeManager.GetInstance<AllianceFactSheetInformations>(reader.ReadShort());
            this.infos.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.guilds = new GuildInAllianceInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guilds[i] = new GuildInAllianceInformations();
                this.guilds[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.controlledSubareaIds = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.controlledSubareaIds[i] = reader.ReadVarUhShort();
            }

            this.leaderCharacterId = reader.ReadVarUhLong();

            if (this.leaderCharacterId < 0 || this.leaderCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on leaderCharacterId = " + this.leaderCharacterId + ", it doesn't respect the following condition : leaderCharacterId < 0 || leaderCharacterId > 9007199254740990");
            this.leaderCharacterName = reader.ReadUTF();
        }
    }
}