using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AllianceModificationStartedMessage : Message {
        public const ushort Id = 6444;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool canChangeName;
        public bool canChangeTag;
        public bool canChangeEmblem;


        public AllianceModificationStartedMessage() { }

        public AllianceModificationStartedMessage(bool canChangeName, bool canChangeTag, bool canChangeEmblem) {
            this.canChangeName = canChangeName;
            this.canChangeTag = canChangeTag;
            this.canChangeEmblem = canChangeEmblem;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.canChangeName);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.canChangeTag);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, this.canChangeEmblem);
            writer.WriteByte(flag1);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.canChangeName = BooleanByteWrapper.GetFlag(flag1, 0);
            this.canChangeTag = BooleanByteWrapper.GetFlag(flag1, 1);
            this.canChangeEmblem = BooleanByteWrapper.GetFlag(flag1, 2);
        }
    }
}