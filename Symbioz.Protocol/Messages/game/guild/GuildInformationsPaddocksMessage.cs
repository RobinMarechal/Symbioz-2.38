using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GuildInformationsPaddocksMessage : Message {
        public const ushort Id = 5959;

        public override ushort MessageId {
            get { return Id; }
        }

        public sbyte nbPaddockMax;
        public PaddockContentInformations[] paddocksInformations;


        public GuildInformationsPaddocksMessage() { }

        public GuildInformationsPaddocksMessage(sbyte nbPaddockMax, PaddockContentInformations[] paddocksInformations) {
            this.nbPaddockMax = nbPaddockMax;
            this.paddocksInformations = paddocksInformations;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteSByte(this.nbPaddockMax);
            writer.WriteUShort((ushort) this.paddocksInformations.Length);
            foreach (var entry in this.paddocksInformations) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.nbPaddockMax = reader.ReadSByte();

            if (this.nbPaddockMax < 0)
                throw new Exception("Forbidden value on nbPaddockMax = " + this.nbPaddockMax + ", it doesn't respect the following condition : nbPaddockMax < 0");
            var limit = reader.ReadUShort();
            this.paddocksInformations = new PaddockContentInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.paddocksInformations[i] = new PaddockContentInformations();
                this.paddocksInformations[i].Deserialize(reader);
            }
        }
    }
}