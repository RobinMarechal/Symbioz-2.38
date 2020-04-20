using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildFactsMessage : Message {
        public const ushort Id = 6415;

        public override ushort MessageId {
            get { return Id; }
        }

        public GuildFactSheetInformations infos;
        public int creationDate;
        public ushort nbTaxCollectors;
        public bool enabled;
        public CharacterMinimalInformations[] members;


        public GuildFactsMessage() { }

        public GuildFactsMessage(GuildFactSheetInformations infos, int creationDate, ushort nbTaxCollectors, bool enabled, CharacterMinimalInformations[] members) {
            this.infos = infos;
            this.creationDate = creationDate;
            this.nbTaxCollectors = nbTaxCollectors;
            this.enabled = enabled;
            this.members = members;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteShort(this.infos.TypeId);
            this.infos.Serialize(writer);
            writer.WriteInt(this.creationDate);
            writer.WriteVarUhShort(this.nbTaxCollectors);
            writer.WriteBoolean(this.enabled);
            writer.WriteUShort((ushort) this.members.Length);
            foreach (var entry in this.members) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.infos = ProtocolTypeManager.GetInstance<GuildFactSheetInformations>(reader.ReadShort());
            this.infos.Deserialize(reader);
            this.creationDate = reader.ReadInt();

            if (this.creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + this.creationDate + ", it doesn't respect the following condition : creationDate < 0");
            this.nbTaxCollectors = reader.ReadVarUhShort();

            if (this.nbTaxCollectors < 0)
                throw new Exception("Forbidden value on nbTaxCollectors = " + this.nbTaxCollectors + ", it doesn't respect the following condition : nbTaxCollectors < 0");
            this.enabled = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.members = new CharacterMinimalInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.members[i] = new CharacterMinimalInformations();
                this.members[i].Deserialize(reader);
            }
        }
    }
}