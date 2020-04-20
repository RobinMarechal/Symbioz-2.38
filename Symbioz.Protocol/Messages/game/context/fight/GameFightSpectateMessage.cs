using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class GameFightSpectateMessage : Message {
        public const ushort Id = 6069;

        public override ushort MessageId {
            get { return Id; }
        }

        public FightDispellableEffectExtendedInformations[] effects;
        public GameActionMark[] marks;
        public ushort gameTurn;
        public int fightStart;
        public Idol[] idols;


        public GameFightSpectateMessage() { }

        public GameFightSpectateMessage(FightDispellableEffectExtendedInformations[] effects, GameActionMark[] marks, ushort gameTurn, int fightStart, Idol[] idols) {
            this.effects = effects;
            this.marks = marks;
            this.gameTurn = gameTurn;
            this.fightStart = fightStart;
            this.idols = idols;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.effects.Length);
            foreach (var entry in this.effects) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.marks.Length);
            foreach (var entry in this.marks) {
                entry.Serialize(writer);
            }

            writer.WriteVarUhShort(this.gameTurn);
            writer.WriteInt(this.fightStart);
            writer.WriteUShort((ushort) this.idols.Length);
            foreach (var entry in this.idols) {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.effects = new FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.effects[i] = new FightDispellableEffectExtendedInformations();
                this.effects[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.marks = new GameActionMark[limit];
            for (int i = 0; i < limit; i++) {
                this.marks[i] = new GameActionMark();
                this.marks[i].Deserialize(reader);
            }

            this.gameTurn = reader.ReadVarUhShort();

            if (this.gameTurn < 0)
                throw new Exception("Forbidden value on gameTurn = " + this.gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
            this.fightStart = reader.ReadInt();

            if (this.fightStart < 0)
                throw new Exception("Forbidden value on fightStart = " + this.fightStart + ", it doesn't respect the following condition : fightStart < 0");
            limit = reader.ReadUShort();
            this.idols = new Idol[limit];
            for (int i = 0; i < limit; i++) {
                this.idols[i] = new Idol();
                this.idols[i].Deserialize(reader);
            }
        }
    }
}