using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectoryRemoveMessage : Message {
        public const ushort Id = 5653;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte jobId;
        public ulong playerId;


        public JobCrafterDirectoryRemoveMessage() { }

        public JobCrafterDirectoryRemoveMessage(sbyte jobId, ulong playerId) {
            this.jobId = jobId;
            this.playerId = playerId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.jobId);
            writer.WriteVarUhLong(this.playerId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.jobId = reader.ReadSByte();

            if (this.jobId < 0)
                throw new Exception("Forbidden value on jobId = " + this.jobId + ", it doesn't respect the following condition : jobId < 0");
            this.playerId = reader.ReadVarUhLong();

            if (this.playerId < 0 || this.playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + this.playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
        }
    }
}