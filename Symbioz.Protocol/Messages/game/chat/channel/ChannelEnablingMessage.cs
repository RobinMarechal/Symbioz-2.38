using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ChannelEnablingMessage : Message {
        public const ushort Id = 890;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte channel;
        public bool enable;


        public ChannelEnablingMessage() { }

        public ChannelEnablingMessage(sbyte channel, bool enable) {
            this.channel = channel;
            this.enable = enable;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.channel);
            writer.WriteBoolean(this.enable);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.channel = reader.ReadSByte();

            if (this.channel < 0)
                throw new Exception("Forbidden value on channel = " + this.channel + ", it doesn't respect the following condition : channel < 0");
            this.enable = reader.ReadBoolean();
        }
    }
}