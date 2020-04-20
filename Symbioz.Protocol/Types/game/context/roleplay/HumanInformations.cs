// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HumanInformations {
        public const short Id = 157;

        public virtual short TypeId {
            get { return Id; }
        }

        public ActorRestrictionsInformations restrictions;
        public bool sex;
        public HumanOption[] options;


        public HumanInformations() { }

        public HumanInformations(ActorRestrictionsInformations restrictions, bool sex, HumanOption[] options) {
            this.restrictions = restrictions;
            this.sex = sex;
            this.options = options;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            this.restrictions.Serialize(writer);
            writer.WriteBoolean(this.sex);
            writer.WriteUShort((ushort) this.options.Length);
            foreach (var entry in this.options) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.restrictions = new ActorRestrictionsInformations();
            this.restrictions.Deserialize(reader);
            this.sex = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            this.options = new HumanOption[limit];
            for (int i = 0; i < limit; i++) {
                this.options[i] = ProtocolTypeManager.GetInstance<HumanOption>(reader.ReadShort());
                this.options[i].Deserialize(reader);
            }
        }
    }
}