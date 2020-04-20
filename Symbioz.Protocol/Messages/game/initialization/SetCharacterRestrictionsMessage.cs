using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SetCharacterRestrictionsMessage : Message {
        public const ushort Id = 170;

        public override ushort MessageId {
            get { return Id; }
        }

        public double actorId;
        public ActorRestrictionsInformations restrictions;


        public SetCharacterRestrictionsMessage() { }

        public SetCharacterRestrictionsMessage(double actorId, ActorRestrictionsInformations restrictions) {
            this.actorId = actorId;
            this.restrictions = restrictions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.actorId);
            this.restrictions.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.actorId = reader.ReadDouble();

            if (this.actorId < -9007199254740990 || this.actorId > 9007199254740990)
                throw new Exception("Forbidden value on actorId = " + this.actorId + ", it doesn't respect the following condition : actorId < -9007199254740990 || actorId > 9007199254740990");
            this.restrictions = new ActorRestrictionsInformations();
            this.restrictions.Deserialize(reader);
        }
    }
}