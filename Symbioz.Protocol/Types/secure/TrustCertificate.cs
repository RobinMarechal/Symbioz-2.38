// Generated on 04/27/2016 01:13:19

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class TrustCertificate {
        public const short Id = 377;

        public virtual short TypeId {
            get { return Id; }
        }

        public int id;
        public string hash;


        public TrustCertificate() { }

        public TrustCertificate(int id, string hash) {
            this.id = id;
            this.hash = hash;
        }


        public virtual void Serialize(ICustomDataOutput writer) {
            writer.WriteInt(this.id);
            writer.WriteUTF(this.hash);
        }

        public virtual void Deserialize(ICustomDataInput reader) {
            this.id = reader.ReadInt();

            if (this.id < 0)
                throw new Exception("Forbidden value on id = " + this.id + ", it doesn't respect the following condition : id < 0");
            this.hash = reader.ReadUTF();
        }
    }
}