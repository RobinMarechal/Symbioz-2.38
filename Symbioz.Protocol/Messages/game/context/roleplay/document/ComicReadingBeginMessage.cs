using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ComicReadingBeginMessage : Message {
        public const ushort Id = 6536;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort comicId;


        public ComicReadingBeginMessage() { }

        public ComicReadingBeginMessage(ushort comicId) {
            this.comicId = comicId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteVarUhShort(this.comicId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.comicId = reader.ReadVarUhShort();

            if (this.comicId < 0)
                throw new Exception("Forbidden value on comicId = " + this.comicId + ", it doesn't respect the following condition : comicId < 0");
        }
    }
}