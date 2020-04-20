using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EmotePlayMessage : EmotePlayAbstractMessage {
        public const ushort Id = 5683;

        public override ushort MessageId {
            get { return Id; }
        }

        public double actorId;
        public int accountId;


        public EmotePlayMessage() { }

        public EmotePlayMessage(byte emoteId, double emoteStartTime, double actorId, int accountId)
            : base(emoteId, emoteStartTime) {
            this.actorId = actorId;
            this.accountId = accountId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.actorId);
            writer.WriteInt(this.accountId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.actorId = reader.ReadDouble();

            if (this.actorId < -9007199254740990 || this.actorId > 9007199254740990)
                throw new Exception("Forbidden value on actorId = " + this.actorId + ", it doesn't respect the following condition : actorId < -9007199254740990 || actorId > 9007199254740990");
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
        }
    }
}