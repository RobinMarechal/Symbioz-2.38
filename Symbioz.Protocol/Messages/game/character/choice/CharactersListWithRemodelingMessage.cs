using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharactersListWithRemodelingMessage : CharactersListMessage {
        public const ushort Id = 6550;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterToRemodelInformations[] charactersToRemodel;


        public CharactersListWithRemodelingMessage() { }

        public CharactersListWithRemodelingMessage(CharacterBaseInformations[] characters, bool hasStartupActions, CharacterToRemodelInformations[] charactersToRemodel)
            : base(characters, hasStartupActions) {
            this.charactersToRemodel = charactersToRemodel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.charactersToRemodel.Length);
            foreach (var entry in this.charactersToRemodel) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.charactersToRemodel = new CharacterToRemodelInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.charactersToRemodel[i] = new CharacterToRemodelInformations();
                this.charactersToRemodel[i].Deserialize(reader);
            }
        }
    }
}