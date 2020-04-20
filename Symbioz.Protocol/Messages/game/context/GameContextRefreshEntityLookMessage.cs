using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameContextRefreshEntityLookMessage : Message {
        public const ushort Id = 5637;

        public override ushort MessageId {
            get { return Id; }
        }

        public double id;
        public EntityLook look;


        public GameContextRefreshEntityLookMessage() { }

        public GameContextRefreshEntityLookMessage(double id, EntityLook look) {
            this.id = id;
            this.look = look;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
            this.look.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            this.look = new EntityLook();
            this.look.Deserialize(reader);
        }
    }
}