using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameActionFightDispellEffectMessage : GameActionFightDispellMessage {
        public const ushort Id = 6113;

        public override ushort MessageId {
            get { return Id; }
        }

        public int boostUID;


        public GameActionFightDispellEffectMessage() { }

        public GameActionFightDispellEffectMessage(ushort actionId, double sourceId, double targetId, int boostUID)
            : base(actionId, sourceId, targetId) {
            this.boostUID = boostUID;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.boostUID);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.boostUID = reader.ReadInt();

            if (this.boostUID < 0)
                throw new Exception("Forbidden value on boostUID = " + this.boostUID + ", it doesn't respect the following condition : boostUID < 0");
        }
    }
}