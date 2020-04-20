using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class ServersListMessage : Message {
        public const ushort Id = 30;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameServerInformations[] servers;
        public ushort alreadyConnectedToServerId;
        public bool canCreateNewCharacter;


        public ServersListMessage() { }

        public ServersListMessage(GameServerInformations[] servers, ushort alreadyConnectedToServerId, bool canCreateNewCharacter) {
            this.servers = servers;
            this.alreadyConnectedToServerId = alreadyConnectedToServerId;
            this.canCreateNewCharacter = canCreateNewCharacter;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.servers.Length);
            foreach (var entry in this.servers) {
                entry.Serialize(writer);
            }

            writer.WriteVarUhShort(this.alreadyConnectedToServerId);
            writer.WriteBoolean(this.canCreateNewCharacter);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.servers = new GameServerInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.servers[i] = new GameServerInformations();
                this.servers[i].Deserialize(reader);
            }

            this.alreadyConnectedToServerId = reader.ReadVarUhShort();

            if (this.alreadyConnectedToServerId < 0)
                throw new Exception("Forbidden value on alreadyConnectedToServerId = " + this.alreadyConnectedToServerId + ", it doesn't respect the following condition : alreadyConnectedToServerId < 0");
            this.canCreateNewCharacter = reader.ReadBoolean();
        }
    }
}