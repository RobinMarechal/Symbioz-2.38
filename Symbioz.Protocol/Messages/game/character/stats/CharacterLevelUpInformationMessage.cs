using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CharacterLevelUpInformationMessage : CharacterLevelUpMessage {
        public const ushort Id = 6076;

        public override ushort MessageId {
            get { return Id; }
        }

        public string name;
        public ulong id;


        public CharacterLevelUpInformationMessage() { }

        public CharacterLevelUpInformationMessage(byte newLevel, string name, ulong id)
            : base(newLevel) {
            this.name = name;
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUTF(this.name);
            writer.WriteVarUhLong(this.id);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.name = reader.ReadUTF();
            this.id = reader.ReadVarUhLong();

            if (this.id < 0 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0 || id > 9007199254740990");
        }
    }
}