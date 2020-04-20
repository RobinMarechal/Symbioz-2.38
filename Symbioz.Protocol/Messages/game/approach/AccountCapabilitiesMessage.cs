using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AccountCapabilitiesMessage : Message {
        public const ushort Id = 6216;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool tutorialAvailable;
        public bool canCreateNewCharacter;
        public int accountId;
        public uint breedsVisible;
        public uint breedsAvailable;
        public sbyte status;


        public AccountCapabilitiesMessage() { }

        public AccountCapabilitiesMessage(bool tutorialAvailable, bool canCreateNewCharacter, int accountId, uint breedsVisible, uint breedsAvailable, sbyte status) {
            this.tutorialAvailable = tutorialAvailable;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.accountId = accountId;
            this.breedsVisible = breedsVisible;
            this.breedsAvailable = breedsAvailable;
            this.status = status;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.tutorialAvailable);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.canCreateNewCharacter);
            writer.WriteByte(flag1);
            writer.WriteInt(this.accountId);
            writer.WriteVarUhInt(this.breedsVisible);
            writer.WriteVarUhInt(this.breedsAvailable);
            writer.WriteSByte(this.status);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.tutorialAvailable = BooleanByteWrapper.GetFlag(flag1, 0);
            this.canCreateNewCharacter = BooleanByteWrapper.GetFlag(flag1, 1);
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.breedsVisible = reader.ReadVarUhInt();

            if (this.breedsVisible < 0)
                throw new Exception("Forbidden value on breedsVisible = " + this.breedsVisible + ", it doesn't respect the following condition : breedsVisible < 0");
            this.breedsAvailable = reader.ReadVarUhInt();

            if (this.breedsAvailable < 0)
                throw new Exception("Forbidden value on breedsAvailable = " + this.breedsAvailable + ", it doesn't respect the following condition : breedsAvailable < 0");
            this.status = reader.ReadSByte();
        }
    }
}