using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class EnabledChannelsMessage : Message {
        public const ushort Id = 892;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte[] channels;
        public sbyte[] disallowed;


        public EnabledChannelsMessage() { }

        public EnabledChannelsMessage(sbyte[] channels, sbyte[] disallowed) {
            this.channels = channels;
            this.disallowed = disallowed;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.channels.Length);
            foreach (var entry in this.channels) {
                writer.WriteSByte(entry);
            }

            writer.WriteUShort((ushort) this.disallowed.Length);
            foreach (var entry in this.disallowed) {
                writer.WriteSByte(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.channels = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.channels[i] = reader.ReadSByte();
            }

            limit = reader.ReadUShort();
            this.disallowed = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.disallowed[i] = reader.ReadSByte();
            }
        }
    }
}