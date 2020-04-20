using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AccountHouseMessage : Message {
        public const ushort Id = 6315;

        public override ushort MessageId {
            get { return Id; }
        }

        public AccountHouseInformations[] houses;


        public AccountHouseMessage() { }

        public AccountHouseMessage(AccountHouseInformations[] houses) {
            this.houses = houses;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.houses.Length);
            foreach (var entry in this.houses) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.houses = new AccountHouseInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.houses[i] = new AccountHouseInformations();
                this.houses[i].Deserialize(reader);
            }
        }
    }
}