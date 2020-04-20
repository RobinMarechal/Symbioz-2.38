using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameRolePlayShowChallengeMessage : Message {
        public const ushort Id = 301;

        public override ushort MessageId {
            get { return Id; }
        }

        public FightCommonInformations commonsInfos;


        public GameRolePlayShowChallengeMessage() { }

        public GameRolePlayShowChallengeMessage(FightCommonInformations commonsInfos) {
            this.commonsInfos = commonsInfos;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.commonsInfos.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.commonsInfos = new FightCommonInformations();
            this.commonsInfos.Deserialize(reader);
        }
    }
}