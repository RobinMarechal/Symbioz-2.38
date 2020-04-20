using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdolSelectErrorMessage : Message {
        public const ushort Id = 6584;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool activate;
        public bool party;
        public sbyte reason;
        public ushort idolId;


        public IdolSelectErrorMessage() { }

        public IdolSelectErrorMessage(bool activate, bool party, sbyte reason, ushort idolId) {
            this.activate = activate;
            this.party = party;
            this.reason = reason;
            this.idolId = idolId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.activate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.party);
            writer.WriteByte(flag1);
            writer.WriteSByte(this.reason);
            writer.WriteVarUhShort(this.idolId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.activate = BooleanByteWrapper.GetFlag(flag1, 0);
            this.party = BooleanByteWrapper.GetFlag(flag1, 1);
            this.reason = reader.ReadSByte();

            if (this.reason < 0)
                throw new Exception("Forbidden value on reason = " + this.reason + ", it doesn't respect the following condition : reason < 0");
            this.idolId = reader.ReadVarUhShort();

            if (this.idolId < 0)
                throw new Exception("Forbidden value on idolId = " + this.idolId + ", it doesn't respect the following condition : idolId < 0");
        }
    }
}