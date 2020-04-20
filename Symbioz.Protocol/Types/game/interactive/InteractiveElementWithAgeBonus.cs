// Generated on 04/27/2016 01:13:17

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class InteractiveElementWithAgeBonus : InteractiveElement {
        public const short Id = 398;

        public override short TypeId {
            get { return Id; }
        }

        public short ageBonus;


        public InteractiveElementWithAgeBonus() { }

        public InteractiveElementWithAgeBonus(int elementId,
                                              int elementTypeId,
                                              InteractiveElementSkill[] enabledSkills,
                                              InteractiveElementSkill[] disabledSkills,
                                              short ageBonus,
                                              bool onCurrentMap)
            : base(elementId, elementTypeId, enabledSkills, disabledSkills, onCurrentMap) {
            this.ageBonus = ageBonus;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteShort(this.ageBonus);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.ageBonus = reader.ReadShort();

            if (this.ageBonus < -1 || this.ageBonus > 1000)
                throw new Exception("Forbidden value on ageBonus = " + this.ageBonus + ", it doesn't respect the following condition : ageBonus < -1 || ageBonus > 1000");
        }
    }
}