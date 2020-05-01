using Symbioz.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Symbioz.World.Models.Entities.Stats {
    public class Characteristic {
        public virtual short Base { get; set; }

        public virtual short Additional { get; set; }

        public virtual short Objects { get; set; }

        [YAXDontSerialize]
        public virtual short Context { get; set; }

        public virtual Characteristic Clone() {
            return new Characteristic() {
                Additional = this.Additional,
                Base = this.Base,
                Context = this.Context,
                Objects = this.Objects
            };
        }

        public static Characteristic Zero() {
            return New(0);
        }

        public static Characteristic New(short @base) {
            return new Characteristic() {
                Base = @base,
                Additional = 0,
                Context = 0,
                Objects = 0
            };
        }

        public CharacterBaseCharacteristic GetBaseCharacteristic() {
            return new CharacterBaseCharacteristic(this.Base, this.Additional, this.Objects, 0, this.Context);
        }

        public virtual short Total() {
            return (short) (this.Base + this.Additional + this.Objects);
        }

        public virtual short TotalInContext() {
            return (short) (this.Total() + this.Context);
        }
    }
}