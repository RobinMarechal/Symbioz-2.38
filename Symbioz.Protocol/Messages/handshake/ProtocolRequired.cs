using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ProtocolRequired : Message {
        public const ushort Id = 1;

        public override ushort MessageId {
            get { return Id; }
        }

        public int requiredVersion;
        public int currentVersion;


        public ProtocolRequired() { }

        public ProtocolRequired(int requiredVersion, int currentVersion) {
            this.requiredVersion = requiredVersion;
            this.currentVersion = currentVersion;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.requiredVersion);
            writer.WriteInt(this.currentVersion);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.requiredVersion = reader.ReadInt();

            if (this.requiredVersion < 0)
                throw new Exception("Forbidden value on requiredVersion = " + this.requiredVersion + ", it doesn't respect the following condition : requiredVersion < 0");
            this.currentVersion = reader.ReadInt();

            if (this.currentVersion < 0)
                throw new Exception("Forbidden value on currentVersion = " + this.currentVersion + ", it doesn't respect the following condition : currentVersion < 0");
        }
    }
}