using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class KohUpdateMessage : Message {
        public const ushort Id = 6439;

        public override ushort MessageId {
            get { return Id; }
        }

        public AllianceInformations[] alliances;
        public ushort[] allianceNbMembers;
        public uint[] allianceRoundWeigth;
        public sbyte[] allianceMatchScore;
        public BasicAllianceInformations allianceMapWinner;
        public uint allianceMapWinnerScore;
        public uint allianceMapMyAllianceScore;
        public double nextTickTime;


        public KohUpdateMessage() { }

        public KohUpdateMessage(AllianceInformations[] alliances,
                                ushort[] allianceNbMembers,
                                uint[] allianceRoundWeigth,
                                sbyte[] allianceMatchScore,
                                BasicAllianceInformations allianceMapWinner,
                                uint allianceMapWinnerScore,
                                uint allianceMapMyAllianceScore,
                                double nextTickTime) {
            this.alliances = alliances;
            this.allianceNbMembers = allianceNbMembers;
            this.allianceRoundWeigth = allianceRoundWeigth;
            this.allianceMatchScore = allianceMatchScore;
            this.allianceMapWinner = allianceMapWinner;
            this.allianceMapWinnerScore = allianceMapWinnerScore;
            this.allianceMapMyAllianceScore = allianceMapMyAllianceScore;
            this.nextTickTime = nextTickTime;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteUShort((ushort) this.alliances.Length);
            foreach (var entry in this.alliances) {
                entry.Serialize(writer);
            }

            writer.WriteUShort((ushort) this.allianceNbMembers.Length);
            foreach (var entry in this.allianceNbMembers) {
                writer.WriteVarUhShort(entry);
            }

            writer.WriteUShort((ushort) this.allianceRoundWeigth.Length);
            foreach (var entry in this.allianceRoundWeigth) {
                writer.WriteVarUhInt(entry);
            }

            writer.WriteUShort((ushort) this.allianceMatchScore.Length);
            foreach (var entry in this.allianceMatchScore) {
                writer.WriteSByte(entry);
            }

            this.allianceMapWinner.Serialize(writer);
            writer.WriteVarUhInt(this.allianceMapWinnerScore);
            writer.WriteVarUhInt(this.allianceMapMyAllianceScore);
            writer.WriteDouble(this.nextTickTime);
        }

        public override void Deserialize(ICustomDataInput reader) {
            var limit = reader.ReadUShort();
            this.alliances = new AllianceInformations[limit];
            for (int i = 0; i < limit; i++) {
                this.alliances[i] = new AllianceInformations();
                this.alliances[i].Deserialize(reader);
            }

            limit = reader.ReadUShort();
            this.allianceNbMembers = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.allianceNbMembers[i] = reader.ReadVarUhShort();
            }

            limit = reader.ReadUShort();
            this.allianceRoundWeigth = new uint[limit];
            for (int i = 0; i < limit; i++) {
                this.allianceRoundWeigth[i] = reader.ReadVarUhInt();
            }

            limit = reader.ReadUShort();
            this.allianceMatchScore = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.allianceMatchScore[i] = reader.ReadSByte();
            }

            this.allianceMapWinner = new BasicAllianceInformations();
            this.allianceMapWinner.Deserialize(reader);
            this.allianceMapWinnerScore = reader.ReadVarUhInt();

            if (this.allianceMapWinnerScore < 0)
                throw new Exception("Forbidden value on allianceMapWinnerScore = " + this.allianceMapWinnerScore + ", it doesn't respect the following condition : allianceMapWinnerScore < 0");
            this.allianceMapMyAllianceScore = reader.ReadVarUhInt();

            if (this.allianceMapMyAllianceScore < 0)
                throw new Exception("Forbidden value on allianceMapMyAllianceScore = " + this.allianceMapMyAllianceScore + ", it doesn't respect the following condition : allianceMapMyAllianceScore < 0");
            this.nextTickTime = reader.ReadDouble();

            if (this.nextTickTime < 0 || this.nextTickTime > 9007199254740990)
                throw new Exception("Forbidden value on nextTickTime = " + this.nextTickTime + ", it doesn't respect the following condition : nextTickTime < 0 || nextTickTime > 9007199254740990");
        }
    }
}