using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayArenaRegistrationStatusMessage : Message {
        public const ushort Id = 6284;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool registered;
        public sbyte step;
        public int battleMode;


        public GameRolePlayArenaRegistrationStatusMessage() { }

        public GameRolePlayArenaRegistrationStatusMessage(bool registered, sbyte step, int battleMode) {
            this.registered = registered;
            this.step = step;
            this.battleMode = battleMode;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteBoolean(this.registered);
            writer.WriteSByte(this.step);
            writer.WriteInt(this.battleMode);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.registered = reader.ReadBoolean();
            this.step = reader.ReadSByte();

            if (this.step < 0)
                throw new Exception("Forbidden value on step = " + this.step + ", it doesn't respect the following condition : step < 0");
            this.battleMode = reader.ReadInt();

            if (this.battleMode < 0)
                throw new Exception("Forbidden value on battleMode = " + this.battleMode + ", it doesn't respect the following condition : battleMode < 0");
        }
    }
}