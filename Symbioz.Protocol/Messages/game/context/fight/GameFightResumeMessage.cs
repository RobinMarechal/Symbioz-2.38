using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightResumeMessage : GameFightSpectateMessage {
        public const ushort Id = 6067;

        public override ushort MessageId {
            get { return Id; }
        }

        public GameFightSpellCooldown[] spellCooldowns;
        public sbyte summonCount;
        public sbyte bombCount;


        public GameFightResumeMessage() { }

        public GameFightResumeMessage(FightDispellableEffectExtendedInformations[] effects,
                                      GameActionMark[] marks,
                                      ushort gameTurn,
                                      int fightStart,
                                      Idol[] idols,
                                      GameFightSpellCooldown[] spellCooldowns,
                                      sbyte summonCount,
                                      sbyte bombCount)
            : base(effects, marks, gameTurn, fightStart, idols) {
            this.spellCooldowns = spellCooldowns;
            this.summonCount = summonCount;
            this.bombCount = bombCount;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteUShort((ushort) this.spellCooldowns.Length);
            foreach (var entry in this.spellCooldowns) {
                entry.Serialize(writer);
            }

            writer.WriteSByte(this.summonCount);
            writer.WriteSByte(this.bombCount);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.spellCooldowns = new GameFightSpellCooldown[limit];
            for (int i = 0; i < limit; i++) {
                this.spellCooldowns[i] = new GameFightSpellCooldown();
                this.spellCooldowns[i].Deserialize(reader);
            }

            this.summonCount = reader.ReadSByte();

            if (this.summonCount < 0)
                throw new Exception("Forbidden value on summonCount = " + this.summonCount + ", it doesn't respect the following condition : summonCount < 0");
            this.bombCount = reader.ReadSByte();

            if (this.bombCount < 0)
                throw new Exception("Forbidden value on bombCount = " + this.bombCount + ", it doesn't respect the following condition : bombCount < 0");
        }
    }
}