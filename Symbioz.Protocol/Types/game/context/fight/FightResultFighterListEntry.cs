// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightResultFighterListEntry : FightResultListEntry {
        public const short Id = 189;

        public override short TypeId {
            get { return Id; }
        }

        public double id;
        public bool alive;


        public FightResultFighterListEntry() { }

        public FightResultFighterListEntry(ushort outcome, sbyte wave, FightLoot rewards, double id, bool alive)
            : base(outcome, wave, rewards) {
            this.id = id;
            this.alive = alive;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteDouble(this.id);
            writer.WriteBoolean(this.alive);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            this.alive = reader.ReadBoolean();
        }
    }
}