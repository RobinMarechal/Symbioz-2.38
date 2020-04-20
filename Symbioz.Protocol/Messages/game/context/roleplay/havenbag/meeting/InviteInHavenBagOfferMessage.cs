using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class InviteInHavenBagOfferMessage : Message {
        public const ushort Id = 6643;

        public override ushort MessageId {
            get { return Id; }
        }

        public CharacterMinimalInformations hostInformations;
        public int timeLeftBeforeCancel;


        public InviteInHavenBagOfferMessage() { }

        public InviteInHavenBagOfferMessage(CharacterMinimalInformations hostInformations, int timeLeftBeforeCancel) {
            this.hostInformations = hostInformations;
            this.timeLeftBeforeCancel = timeLeftBeforeCancel;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.hostInformations.Serialize(writer);
            writer.WriteVarInt(this.timeLeftBeforeCancel);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.hostInformations = new CharacterMinimalInformations();
            this.hostInformations.Deserialize(reader);
            this.timeLeftBeforeCancel = reader.ReadVarInt();
        }
    }
}