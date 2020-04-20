using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class BasicCharactersListMessage : Message {
        public const ushort Id = 6475;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterBaseInformations[] characters;


        public BasicCharactersListMessage() { }

        public BasicCharactersListMessage(CharacterBaseInformations[] characters) {
            this.characters = characters;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.characters.Length);
            foreach (var entry in this.characters) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.characters = new CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.characters[i] = ProtocolTypeManager.GetInstance<CharacterBaseInformations>(reader.ReadShort());
                this.characters[i].Deserialize(reader);
            }
        }
    }
}