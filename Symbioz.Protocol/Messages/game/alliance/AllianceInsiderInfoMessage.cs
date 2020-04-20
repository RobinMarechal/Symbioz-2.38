using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceInsiderInfoMessage : Message {
        public const ushort Id = 6403;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceFactSheetInformations allianceInfos;
        public GuildInsiderFactSheetInformations[] guilds;
        public PrismSubareaEmptyInfo[] prisms;


        public AllianceInsiderInfoMessage() { }

        public AllianceInsiderInfoMessage(AllianceFactSheetInformations allianceInfos, GuildInsiderFactSheetInformations[] guilds, PrismSubareaEmptyInfo[] prisms) {
            this.allianceInfos = allianceInfos;
            this.guilds = guilds;
            this.prisms = prisms;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.allianceInfos.Serialize(writer);
            writer.WriteUShort((ushort) this.guilds.Length);
            foreach (var entry in this.guilds) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.prisms.Length);
            foreach (var entry in this.prisms) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.allianceInfos = new AllianceFactSheetInformations();
            this.allianceInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.guilds = new GuildInsiderFactSheetInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.guilds[i] = new GuildInsiderFactSheetInformations();
                this.guilds[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.prisms = new PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.prisms[i] = ProtocolTypeManager.GetInstance<PrismSubareaEmptyInfo>(reader.ReadShort());
                this.prisms[i].Deserialize(reader);
            }
        }
    }
}