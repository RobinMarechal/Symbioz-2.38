using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class SlaveSwitchContextMessage : Message {
        public const ushort Id = 6214;

        public override ushort MessageId {
            get { return Id; }
        }

        public double masterId;
        public double slaveId;
        public SpellItem[] slaveSpells;
        public CharacterCharacteristicsInformations slaveStats;
        public Shortcut[] shortcuts;


        public SlaveSwitchContextMessage() { }

        public SlaveSwitchContextMessage(double masterId, double slaveId, SpellItem[] slaveSpells, CharacterCharacteristicsInformations slaveStats, Shortcut[] shortcuts) {
            this.masterId = masterId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
            this.shortcuts = shortcuts;
        }


        public override void Serialize(ICustomDataOutput writer) {
            writer.WriteDouble(this.masterId);
            writer.WriteDouble(this.slaveId);
            writer.WriteUShort((ushort) this.slaveSpells.Length);
            foreach (var entry in this.slaveSpells) {
                entry.Serialize(writer);
            }

            this.slaveStats.Serialize(writer);
            writer.WriteUShort((ushort) this.shortcuts.Length);
            foreach (var entry in this.shortcuts) {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.masterId = reader.ReadDouble();

            if (this.masterId < -9007199254740990 || this.masterId > 9007199254740990)
                throw new Exception("Forbidden value on masterId = " + this.masterId + ", it doesn't respect the following condition : masterId < -9007199254740990 || masterId > 9007199254740990");
            this.slaveId = reader.ReadDouble();

            if (this.slaveId < -9007199254740990 || this.slaveId > 9007199254740990)
                throw new Exception("Forbidden value on slaveId = " + this.slaveId + ", it doesn't respect the following condition : slaveId < -9007199254740990 || slaveId > 9007199254740990");
            var limit = reader.ReadUShort();
            this.slaveSpells = new SpellItem[limit];
            for (int i = 0; i < limit; i++) {
                this.slaveSpells[i] = new SpellItem();
                this.slaveSpells[i].Deserialize(reader);
            }

            this.slaveStats = new CharacterCharacteristicsInformations();
            this.slaveStats.Deserialize(reader);
            limit = reader.ReadUShort();
            this.shortcuts = new Shortcut[limit];
            for (int i = 0; i < limit; i++) {
                this.shortcuts[i] = ProtocolTypeManager.GetInstance<Shortcut>(reader.ReadShort());
                this.shortcuts[i].Deserialize(reader);
            }
        }
    }
}