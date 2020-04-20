using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities.Look;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YAXLib;

namespace Symbioz.World.Models.Entities.HumanOptions {
    public class CharacterHumanOptionFollowers : CharacterHumanOption {
        public CharacterHumanOptionFollowers() {
            this.Looks = new Dictionary<sbyte, ContextActorLook>();
        }

        public CharacterHumanOptionFollowers(ContextActorLook follower) {
            this.Looks = new Dictionary<sbyte, ContextActorLook>();
            this.AddFollower(follower);
        }

        public Dictionary<sbyte, ContextActorLook> Looks { get; set; }

        public void AddFollower(ContextActorLook look) {
            if (this.Looks.Count > 0)
                this.Looks.Add((sbyte) (this.Looks.Keys.Last() + 1), look);
            else
                this.Looks.Add(1, look);
        }

        public void RemoveFollower(ContextActorLook look) {
            var data = this.Looks.FirstOrDefault(x => x.Value.Equals(look));

            if (data.Value != null) this.Looks.Remove(data.Key);
        }

        public override HumanOption GetHumanOption() {
            return new HumanOptionFollowers(this.Looks.ToList().ConvertAll<IndexedEntityLook>(x => new IndexedEntityLook(x.Value.ToEntityLook(), x.Key)).ToArray());
        }
    }
}