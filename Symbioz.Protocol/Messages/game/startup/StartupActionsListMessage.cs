using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class StartupActionsListMessage : Message {
        public const ushort Id = 1301;

        public override ushort MessageId {
            get { return Id; }
        }

        public StartupActionAddObject[] actions;


        public StartupActionsListMessage() { }

        public StartupActionsListMessage(StartupActionAddObject[] actions) {
            this.actions = actions;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.actions.Length);
            foreach (var entry in this.actions) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.actions = new StartupActionAddObject[limit];
            for (int i = 0; i < limit; i++) {
                this.actions[i] = new StartupActionAddObject();
                this.actions[i].Deserialize(reader);
            }
        }
    }
}