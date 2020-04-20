using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterCreationRequestMessage : Message {
        public const ushort Id = 160;

        public override ushort MessageId {
            get { return Id; }
        }

        public string name;
        public sbyte breed;
        public bool sex;
        public int[] colors;
        public ushort cosmeticId;


        public CharacterCreationRequestMessage() { }

        public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, int[] colors, ushort cosmeticId) {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUTF(this.name);
            writer.WriteSByte(this.breed);
            writer.WriteBoolean(this.sex);
            foreach (var entry in this.colors) {
                writer.WriteInt(entry);
            }

            writer.WriteVarUhShort(this.cosmeticId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.name = reader.ReadUTF();
            this.breed = reader.ReadSByte();

            if (this.breed < (byte) Enums.PlayableBreedEnum.Feca || this.breed > (byte) Enums.PlayableBreedEnum.Huppermage)
                throw new Exception("Forbidden value on breed = "
                                    + this.breed
                                    + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Huppermage");
            this.sex = reader.ReadBoolean();

            this.colors = new int[5];
            for (int i = 0; i < 5; i++) {
                this.colors[i] = reader.ReadInt();
            }

            this.cosmeticId = reader.ReadVarUhShort();

            if (this.cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + this.cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
    }
}