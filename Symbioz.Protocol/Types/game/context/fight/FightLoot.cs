// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightLoot {
        public const short Id = 41;

        public virtual short TypeId {
            get { return Id; }
        }

        public ushort[] objects;
        public uint kamas;


        public FightLoot() { }

        public FightLoot(ushort[] objects, uint kamas) {
            this.objects = objects;
            this.kamas = kamas;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.objects.Length);
            foreach (var entry in this.objects) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteVarUhInt(this.kamas);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.objects = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.objects[i] = reader.ReadVarUhShort();
            }

            this.kamas = reader.ReadVarUhInt();

            if (this.kamas < 0)
                throw new Exception("Forbidden value on kamas = " + this.kamas + ", it doesn't respect the following condition : kamas < 0");
        }
    }
}