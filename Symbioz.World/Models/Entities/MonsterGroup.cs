using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities.Look;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Maps.Shapes;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Providers.Maps.Path;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;

namespace Symbioz.World.Models.Entities {
    public class MonsterGroup : Entity {
        public const short ClientStarsBonusLimit = 200;

        public static int StarsBonusInterval = 300;

        public static short StarsBonusIncrementation = 2;

        public static short StarsBonusLimit = 300;

        public const uint AgeBonusRate = 0;

        private List<Monster> Monsters = new List<Monster>();

        public int MonsterCount {
            get { return this.Monsters.Count; }
        }

        public MonsterGroup(MapRecord map) {
            this.Map = map;
            this.CellId = this.Map.RandomNoBorderFightCell();
            this.m_UId = this.Map.Instance.PopNextNPEntityId();
            this.CreationDate = DateTime.Now;
        }

        public override string Name {
            get { return this.Leader.Template.Name; }
        }

        private long m_UId;

        public override long Id {
            get { return this.m_UId; }
        }

        public override ushort CellId { get; set; }

        public DateTime CreationDate { get; set; }

        public short AgeBonus {
            get {
                return 0;
                // return (short)Math.Min(200d, (DateTime.Now.DateTimeToUnixTimestamp() - CreationDate.DateTimeToUnixTimestamp()) / 60000);
            }
        }

        public override DirectionsEnum Direction { get; set; }

        public override ContextActorLook Look {
            get { return this.Leader.Look; }
            set { return; }
        }

        public Monster Leader { get; private set; }

        public void AddMonster(Monster monster) {
            this.Monsters.Add(monster);

            if (this.Monsters.Count == 1)
                this.Leader = monster;
        }

        public Monster[] GetMonsters() {
            return this.Monsters.ToArray();
        }

        private void Move(List<short> keys) {
            this.Map.Instance.Send(new GameMapMovementMessage(keys.ToArray(), this.Id));
            this.CellId = (ushort) keys.Last();
        }

        public void RandomMapMove() {
            Lozenge lozenge = new Lozenge(1, 4);
            short cellId = lozenge.GetCells((short) this.CellId, this.Map).Where(entry => this.Map.Walkable((ushort) entry)).Random();

            if (cellId != 0) {
                Pathfinder pathfinder = new Pathfinder(this.Map, (short) this.CellId, cellId);
                var cells = pathfinder.FindPath();

                if (cells != null && cells.Count > 0) {
                    cells.Insert(0, (short) this.CellId);
                    this.Move(cells);
                }
            }
        }

        public IEnumerable<Fighter> CreateFighters(FightTeam team) {
            foreach (var monster in this.Monsters) {
                yield return monster.CreateFighter(team);
            }
        }

        public static MonsterGroup FromTemplates(MapRecord map, MonsterRecord[] templates) {
            MonsterGroup group = new MonsterGroup(map);

            foreach (var template in templates) {
                group.AddMonster(new Monster(template, group));
            }

            return group;
        }

        public MonsterInGroupInformations[] GetMonsterInGroupInformations() {
            return this.Monsters.FindAll(x => x != this.Leader).ConvertAll(x => x.GetMonsterInGroupInformations()).ToArray();
        }

        public GroupMonsterStaticInformations GetGroupMonsterStaticInformations() {
            return new GroupMonsterStaticInformations(this.Leader.GetMonsterInGroupLightInformations(), this.GetMonsterInGroupInformations());
        }

        public override GameRolePlayActorInformations GetActorInformations() {
            return new GameRolePlayGroupMonsterInformations((int) this.Id,
                                                            this.Look.ToEntityLook(),
                                                            new EntityDispositionInformations((short) this.CellId, (sbyte) this.Direction),
                                                            false,
                                                            false,
                                                            false,
                                                            this.GetGroupMonsterStaticInformations(),
                                                            this.CreationDate.DateTimeToUnixTimestamp(),
                                                            0,
                                                            0,
                                                            0);
        }

        public override string ToString() {
            return "Monsters (" + this.Name + "' group)";
        }
    }
}