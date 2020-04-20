using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightResumeWithSlavesMessage : GameFightResumeMessage {
        public const ushort Id = 6215;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameFightResumeSlaveInfo[] slavesInfo;


        public GameFightResumeWithSlavesMessage() { }

        public GameFightResumeWithSlavesMessage(FightDispellableEffectExtendedInformations[] effects,
                                                GameActionMark[] marks,
                                                ushort gameTurn,
                                                int fightStart,
                                                Idol[] idols,
                                                GameFightSpellCooldown[] spellCooldowns,
                                                sbyte summonCount,
                                                sbyte bombCount,
                                                GameFightResumeSlaveInfo[] slavesInfo)
            : base(effects, marks, gameTurn, fightStart, idols, spellCooldowns, summonCount, bombCount) {
            this.slavesInfo = slavesInfo;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.slavesInfo.Length);
            foreach (var entry in this.slavesInfo) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.slavesInfo = new GameFightResumeSlaveInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.slavesInfo[i] = new GameFightResumeSlaveInfo();
                this.slavesInfo[i].Deserialize(reader);
            }
        }
    }
}