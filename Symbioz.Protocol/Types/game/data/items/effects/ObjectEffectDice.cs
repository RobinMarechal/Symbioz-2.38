// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class ObjectEffectDice : ObjectEffect {
        public const short Id = 73;

        public override short TypeId {
            get { return Id; }
        }

        public ushort diceNum;
        public ushort diceSide;
        public ushort diceConst;


        public ObjectEffectDice() { }

        public ObjectEffectDice(ushort actionId, ushort diceNum, ushort diceSide, ushort diceConst)
            : base(actionId) {
            this.diceNum = diceNum;
            this.diceSide = diceSide;
            this.diceConst = diceConst;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteVarUhShort(this.diceNum);
            writer.WriteVarUhShort(this.diceSide);
            writer.WriteVarUhShort(this.diceConst);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.diceNum = reader.ReadVarUhShort();

            if (this.diceNum < 0)
                throw new Exception("Forbidden value on diceNum = " + this.diceNum + ", it doesn't respect the following condition : diceNum < 0");
            this.diceSide = reader.ReadVarUhShort();

            if (this.diceSide < 0)
                throw new Exception("Forbidden value on diceSide = " + this.diceSide + ", it doesn't respect the following condition : diceSide < 0");
            this.diceConst = reader.ReadVarUhShort();

            if (this.diceConst < 0)
                throw new Exception("Forbidden value on diceConst = " + this.diceConst + ", it doesn't respect the following condition : diceConst < 0");
        }
    }
}