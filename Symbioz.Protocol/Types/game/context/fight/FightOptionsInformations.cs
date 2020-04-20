// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightOptionsInformations {
        public const short Id = 20;

        public virtual short TypeId {
            get { return Id; }
        }

        public bool isSecret;
        public bool isRestrictedToPartyOnly;
        public bool isClosed;
        public bool isAskingForHelp;

        public FightOptionsInformations() { }

        public FightOptionsInformations(bool isSecret, bool isRestrictedToPartyOnly, bool isClosed, bool isAskingForHelp) {
            this.isSecret = isSecret;
            this.isRestrictedToPartyOnly = isRestrictedToPartyOnly;
            this.isClosed = isClosed;
            this.isAskingForHelp = isAskingForHelp;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.isSecret);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.isRestrictedToPartyOnly);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, this.isClosed);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, this.isAskingForHelp);
            writer.WriteByte(flag1);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.isSecret = BooleanByteWrapper.GetFlag(flag1, 0);
            this.isRestrictedToPartyOnly = BooleanByteWrapper.GetFlag(flag1, 1);
            this.isClosed = BooleanByteWrapper.GetFlag(flag1, 2);
            this.isAskingForHelp = BooleanByteWrapper.GetFlag(flag1, 3);
        }
    }
}