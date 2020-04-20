using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolListMessage : Message {
        public const ushort Id = 6585;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] chosenIdols;
        public ushort[] partyChosenIdols;
        public PartyIdol[] partyIdols;


        public IdolListMessage() { }

        public IdolListMessage(ushort[] chosenIdols, ushort[] partyChosenIdols, PartyIdol[] partyIdols) {
            this.chosenIdols = chosenIdols;
            this.partyChosenIdols = partyChosenIdols;
            this.partyIdols = partyIdols;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.chosenIdols.Length);
            foreach (var entry in this.chosenIdols) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.partyChosenIdols.Length);
            foreach (var entry in this.partyChosenIdols) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.partyIdols.Length);
            foreach (var entry in this.partyIdols) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.chosenIdols = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.chosenIdols[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.partyChosenIdols = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.partyChosenIdols[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.partyIdols = new PartyIdol[limit];
            for (int i = 0; i < limit; i++) {
                this.partyIdols[i] = ProtocolTypeManager.GetInstance<PartyIdol>(reader.ReadShort());
                this.partyIdols[i].Deserialize(reader);
            }
        }
    }
}