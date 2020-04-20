using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharactersListWithModificationsMessage : CharactersListMessage {
        public const ushort Id = 6120;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterToRecolorInformation[] charactersToRecolor;
        public int[] charactersToRename;
        public int[] unusableCharacters;
        public CharacterToRelookInformation[] charactersToRelook;


        public CharactersListWithModificationsMessage() { }

        public CharactersListWithModificationsMessage(CharacterBaseInformations[] characters,
                                                      bool hasStartupActions,
                                                      CharacterToRecolorInformation[] charactersToRecolor,
                                                      int[] charactersToRename,
                                                      int[] unusableCharacters,
                                                      CharacterToRelookInformation[] charactersToRelook)
            : base(characters, hasStartupActions) {
            this.charactersToRecolor = charactersToRecolor;
            this.charactersToRename = charactersToRename;
            this.unusableCharacters = unusableCharacters;
            this.charactersToRelook = charactersToRelook;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.charactersToRecolor.Length);
            foreach (var entry in this.charactersToRecolor) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.charactersToRename.Length);
            foreach (var entry in this.charactersToRename) {
                writer.WriteInt(entry);
            }

            writer.WriteUShort((ushort) this.unusableCharacters.Length);
            foreach (var entry in this.unusableCharacters) {
                writer.WriteInt(entry);
            }

            writer.WriteUShort((ushort) this.charactersToRelook.Length);
            foreach (var entry in this.charactersToRelook) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.charactersToRecolor = new CharacterToRecolorInformation[limit];
            for (int i = 0; i < limit; i++) {
                this.charactersToRecolor[i] = new CharacterToRecolorInformation();
                this.charactersToRecolor[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.charactersToRename = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.charactersToRename[i] = reader.ReadInt();
            }

            limit = reader.ReadUShort();
            this.unusableCharacters = new int[limit];
            for (int i = 0; i < limit; i++) {
                this.unusableCharacters[i] = reader.ReadInt();
            }

            limit = reader.ReadUShort();
            this.charactersToRelook = new CharacterToRelookInformation[limit];
            for (int i = 0; i < limit; i++) {
                this.charactersToRelook[i] = new CharacterToRelookInformation();
                this.charactersToRelook[i].Deserialize(reader);
            }
        }
    }
}