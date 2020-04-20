using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SelectedServerRefusedMessage : Message {
        public const ushort Id = 41;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort serverId;
        public sbyte error;
        public sbyte serverStatus;


        public SelectedServerRefusedMessage() { }

        public SelectedServerRefusedMessage(ushort serverId, sbyte error, sbyte serverStatus) {
            this.serverId = serverId;
            this.error = error;
            this.serverStatus = serverStatus;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.serverId);
            writer.WriteSByte(this.error);
            writer.WriteSByte(this.serverStatus);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.serverId = reader.ReadVarUhShort();

            if (this.serverId < 0)
                throw new Exception("Forbidden value on serverId = " + this.serverId + ", it doesn't respect the following condition : serverId < 0");
            this.error = reader.ReadSByte();

            if (this.error < 0)
                throw new Exception("Forbidden value on error = " + this.error + ", it doesn't respect the following condition : error < 0");
            this.serverStatus = reader.ReadSByte();

            if (this.serverStatus < 0)
                throw new Exception("Forbidden value on serverStatus = " + this.serverStatus + ", it doesn't respect the following condition : serverStatus < 0");
        }
    }
}