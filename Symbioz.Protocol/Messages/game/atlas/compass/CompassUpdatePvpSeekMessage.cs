using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class CompassUpdatePvpSeekMessage : CompassUpdateMessage {
        public const ushort Id = 6013;

        public override ushort MessageId {
            get { return Id; }
        }

        public ulong memberId;
        public string memberName;


        public CompassUpdatePvpSeekMessage() { }

        public CompassUpdatePvpSeekMessage(sbyte type, MapCoordinates coords, ulong memberId, string memberName)
            : base(type, coords) {
            this.memberId = memberId;
            this.memberName = memberName;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhLong(this.memberId);
            writer.WriteUTF(this.memberName);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.memberId = reader.ReadVarUhLong();

            if (this.memberId < 0 || this.memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + this.memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            this.memberName = reader.ReadUTF();
        }
    }
}