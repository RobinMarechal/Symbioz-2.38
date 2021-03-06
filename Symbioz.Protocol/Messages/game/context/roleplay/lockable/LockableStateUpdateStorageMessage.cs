using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class LockableStateUpdateStorageMessage : LockableStateUpdateAbstractMessage {
        public const ushort Id = 5669;

        public override ushort MessageId {
            get { return Id; }
        }

        public int mapId;
        public uint elementId;


        public LockableStateUpdateStorageMessage() { }

        public LockableStateUpdateStorageMessage(bool locked, int mapId, uint elementId)
            : base(locked) {
            this.mapId = mapId;
            this.elementId = elementId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.mapId);
            writer.WriteVarUhInt(this.elementId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.mapId = reader.ReadInt();
            this.elementId = reader.ReadVarUhInt();

            if (this.elementId < 0)
                throw new Exception("Forbidden value on elementId = " + this.elementId + ", it doesn't respect the following condition : elementId < 0");
        }
    }
}