// Generated on 04/27/2016 01:13:11

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FightTeamMemberInformations {
        public const short Id = 44;

        public virtual short TypeId {
            get { return Id; }
        }

        public double id;


        public FightTeamMemberInformations() { }

        public FightTeamMemberInformations(double id) {
            this.id = id;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.id);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadDouble();

            if (this.id < -9007199254740990 || this.id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
        }
    }
}