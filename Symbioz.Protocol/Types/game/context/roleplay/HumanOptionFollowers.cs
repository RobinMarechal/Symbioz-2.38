// Generated on 04/27/2016 01:13:14

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class HumanOptionFollowers : HumanOption {
        public const short Id = 410;

        public override short TypeId {
            get { return Id; }
        }

        public IndexedEntityLook[] followingCharactersLook;


        public HumanOptionFollowers() { }

        public HumanOptionFollowers(IndexedEntityLook[] followingCharactersLook) {
            this.followingCharactersLook = followingCharactersLook;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.followingCharactersLook.Length);
            foreach (var entry in this.followingCharactersLook) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.followingCharactersLook = new IndexedEntityLook[limit];
            for (int i = 0; i < limit; i++) {
                this.followingCharactersLook[i] = new IndexedEntityLook();
                this.followingCharactersLook[i].Deserialize(reader);
            }
        }
    }
}