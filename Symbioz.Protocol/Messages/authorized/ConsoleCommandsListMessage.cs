using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ConsoleCommandsListMessage : Message {
        public const ushort Id = 6127;

        public override ushort MessageId {
            get { return Id; }
        }

        public string[] aliases;
        public string[] args;
        public string[] descriptions;


        public ConsoleCommandsListMessage() { }

        public ConsoleCommandsListMessage(string[] aliases, string[] args, string[] descriptions) {
            this.aliases = aliases;
            this.args = args;
            this.descriptions = descriptions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.aliases.Length);
            foreach (var entry in this.aliases) {
                writer.WriteUTF(entry);
            }

            writer.WriteUShort((ushort) this.args.Length);
            foreach (var entry in this.args) {
                writer.WriteUTF(entry);
            }

            writer.WriteUShort((ushort) this.descriptions.Length);
            foreach (var entry in this.descriptions) {
                writer.WriteUTF(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.aliases = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.aliases[i] = reader.ReadUTF();
            }

            limit = reader.ReadUShort();
            this.args = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.args[i] = reader.ReadUTF();
            }

            limit = reader.ReadUShort();
            this.descriptions = new string[limit];
            for (int i = 0; i < limit; i++) {
                this.descriptions[i] = reader.ReadUTF();
            }
        }
    }
}