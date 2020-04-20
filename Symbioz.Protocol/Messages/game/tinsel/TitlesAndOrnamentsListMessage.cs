using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class TitlesAndOrnamentsListMessage : Message {
        public const ushort Id = 6367;

        public override ushort MessageId {
            get { return Id; }
        }

        public ushort[] titles;
        public ushort[] ornaments;
        public ushort activeTitle;
        public ushort activeOrnament;


        public TitlesAndOrnamentsListMessage() { }

        public TitlesAndOrnamentsListMessage(ushort[] titles, ushort[] ornaments, ushort activeTitle, ushort activeOrnament) {
            this.titles = titles;
            this.ornaments = ornaments;
            this.activeTitle = activeTitle;
            this.activeOrnament = activeOrnament;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.titles.Length);
            foreach (var entry in this.titles) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.ornaments.Length);
            foreach (var entry in this.ornaments) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteVarUhShort(this.activeTitle);
            writer.WriteVarUhShort(this.activeOrnament);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.titles = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.titles[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.ornaments = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.ornaments[i] = reader.ReadVarUhShort();
            }

            this.activeTitle = reader.ReadVarUhShort();

            if (this.activeTitle < 0)
                throw new Exception("Forbidden value on activeTitle = " + this.activeTitle + ", it doesn't respect the following condition : activeTitle < 0");
            this.activeOrnament = reader.ReadVarUhShort();

            if (this.activeOrnament < 0)
                throw new Exception("Forbidden value on activeOrnament = " + this.activeOrnament + ", it doesn't respect the following condition : activeOrnament < 0");
        }
    }
}