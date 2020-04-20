using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class FriendsListMessage : Message {
        public const ushort Id = 4002;

        public override ushort MessageId {
            get { return Id; }
        }

        public FriendInformations[] friendsList;


        public FriendsListMessage() { }

        public FriendsListMessage(FriendInformations[] friendsList) {
            this.friendsList = friendsList;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.friendsList.Length);
            foreach (var entry in this.friendsList) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.friendsList = new FriendInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.friendsList[i] = ProtocolTypeManager.GetInstance<FriendInformations>(reader.ReadShort());
                this.friendsList[i].Deserialize(reader);
            }
        }
    }
}