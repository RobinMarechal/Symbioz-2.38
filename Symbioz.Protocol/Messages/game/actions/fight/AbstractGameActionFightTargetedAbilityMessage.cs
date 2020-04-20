using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class AbstractGameActionFightTargetedAbilityMessage : AbstractGameActionMessage {
        public const ushort Id = 6118;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool silentCast;
        public bool verboseCast;
        public double targetId;
        public short destinationCellId;
        public sbyte critical;


        public AbstractGameActionFightTargetedAbilityMessage() { }

        public AbstractGameActionFightTargetedAbilityMessage(ushort actionId, double sourceId, bool silentCast, bool verboseCast, double targetId, short destinationCellId, sbyte critical)
            : base(actionId, sourceId) {
            this.silentCast = silentCast;
            this.verboseCast = verboseCast;
            this.targetId = targetId;
            this.destinationCellId = destinationCellId;
            this.critical = critical;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.silentCast);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.verboseCast);
            writer.WriteByte(flag1);
            writer.WriteDouble(this.targetId);
            writer.WriteShort(this.destinationCellId);
            writer.WriteSByte(this.critical);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            this.silentCast = BooleanByteWrapper.GetFlag(flag1, 0);
            this.verboseCast = BooleanByteWrapper.GetFlag(flag1, 1);
            this.targetId = reader.ReadDouble();

            if (this.targetId < -9007199254740990 || this.targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + this.targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            this.destinationCellId = reader.ReadShort();

            if (this.destinationCellId < -1 || this.destinationCellId > 559)
                throw new Exception("Forbidden value on destinationCellId = " + this.destinationCellId + ", it doesn't respect the following condition : destinationCellId < -1 || destinationCellId > 559");
            this.critical = reader.ReadSByte();

            if (this.critical < 0)
                throw new Exception("Forbidden value on critical = " + this.critical + ", it doesn't respect the following condition : critical < 0");
        }
    }
}