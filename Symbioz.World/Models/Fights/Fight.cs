using SSync.Messages;
using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Models.Fights.Results;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Maps.Shapes;
using Symbioz.World.Providers.Fights;
using Symbioz.World.Providers.Fights.Challenges;
using Symbioz.World.Providers.Fights.Effects;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights {
    public abstract class Fight {
        Logger logger = new Logger();

        /// <summary>
        /// en secondes
        /// </summary>
        public const uint TurnTime = 30;

        public abstract FightTypeEnum FightType { get; }

        public abstract bool SpawnJoin { get; }

        public abstract bool PvP { get; }

        public abstract bool ShowBlades { get; }

        public int Id { get; set; }

        public MapRecord Map { get; set; }

        public short CellId { get; set; }

        public FightTeam BlueTeam { get; set; }

        public FightTeam RedTeam { get; set; }
        public FightTeam Winners { get; set; }
        public FightTeam Losers { get; set; }
        public DateTime CreationTime { get; private set; }
        public DateTime StartTime { get; private set; }
        protected ActionTimer m_placementTimer { get; set; }
        protected ActionTimer m_turnTimer { get; set; }
        public TimeLine TimeLine { get; private set; }

        public Fighter FighterPlaying {
            get { return this.TimeLine.Current; }
        }

        public SequenceManager SequencesManager { get; private set; }
        public Synchronizer Synchronizer { get; protected set; }

        /// <summary>
        /// Action<bool>Started = true, Placement = false</bool>
        /// </summary>
        public event Action<Fight, bool> OnFightEndedEvt;

        public event Action<Fight> FightStartEvt;

        private ReversedUniqueIdProvider m_contextualIdPopper = new ReversedUniqueIdProvider(0);

        private UniqueIdProvider m_markIdPopper = new UniqueIdProvider();

        public bool Started { get; set; }
        public short AgeBonus { get; set; }
        public List<Mark> Marks { get; private set; }
        public abstract bool MinationAllowed { get; }

        public bool Ended = false;

        public Fight(MapRecord map, FightTeam blueTeam, FightTeam redTeam, short cellId) {
            this.Id = FightProvider.Instance.PopId();
            this.Map = map;
            this.BlueTeam = blueTeam;
            this.RedTeam = redTeam;
            this.BlueTeam.Fight = this;
            this.RedTeam.Fight = this;
            this.TimeLine = new TimeLine(this);
            this.SequencesManager = new SequenceManager(this);
            this.CellId = cellId;
            this.Started = false;
            this.CreationTime = DateTime.Now;
            this.Marks = new List<Mark>();
        }

        #region FightPreparation

        public virtual void StartPlacement() {
            //  Logger.Write<Fight>("Démarage du placement", ConsoleColor.DarkMagenta);

            if (this.GetPreparationDelay() > 0) {
                this.m_placementTimer = new ActionTimer(this.GetPreparationDelay() * 1000, this.StartFighting, false);
                this.m_placementTimer.Start();
            }

            if (this.ShowBlades) {
                this.ShowBladesOnMap();
                this.Send(this.GetIdolFightPreparationUpdateMessage());
            }
        }

        public virtual IdolFightPreparationUpdateMessage GetIdolFightPreparationUpdateMessage() {
            return new IdolFightPreparationUpdateMessage(0, new Idol[0]);
        }

        public virtual void OnSetReady(Fighter fighter, bool isReady) {
            this.Send(new GameFightHumanReadyStateMessage((ulong) fighter.Id, isReady));
            this.CheckFightStart();
        }

        public void CheckFightStart() {
            if (this.RedTeam.AreAllReady() && this.BlueTeam.AreAllReady()) {
                this.StartFighting();
            }
        }

        public virtual void StartFighting() {
            //   Logger.Write<Fight>("Lancement du combat!", ConsoleColor.DarkMagenta);

            if (this.GetPreparationDelay() > 0) {
                this.m_placementTimer.Stop();
            }

            this.StartTime = DateTime.Now;

            this.Started = true;

            this.UpdateEntitiesPositions();

            this.Map.Instance.RemoveFightSword(this);

            this.TimeLine.OrderLine();

            this.Send(this.GetGameFightStartMessage());

            this.UpdateTimeLine();

            this.OnFightStarted();
        }

        public virtual GameFightStartMessage GetGameFightStartMessage() {
            return new GameFightStartMessage(new Idol[0]);
        }

        public void Synchronize() {
            this.Send(new GameFightSynchronizeMessage(this.GetAllFighters().ConvertAll<GameFightFighterInformations>(x => x.GetFightFighterInformations()).ToArray()));
        }

        public void UpdateRound() {
            this.Send(new GameFightNewRoundMessage((uint) this.TimeLine.RoundNumber));
        }

        public virtual void OnFightStarted() {
            this.Synchronize();
            this.UpdateRound();

            foreach (var fighter in this.GetAllFighters()) {
                fighter.OnFightStarted();
            }

            if (this.FightStartEvt != null) this.FightStartEvt(this);

            this.StartTurn();
        }

        public short GetPlacementTimeLeft() {
            double num = (double) this.GetPreparationDelay() - (DateTime.Now - this.CreationTime).TotalSeconds;
            if (num < 0.0) {
                num = 0.0;
            }

            return (short) ((double) num * (double) 10);
        }

        public virtual int GetPreparationDelay() {
            return 0;
        }

        public void StartTurn() {
            // Logger.Write<Fight>("Nouveau tour!", ConsoleColor.DarkMagenta);
            if (this.Started && !this.Ended) {
                this.OnTurnStarted();
            }
        }

        public virtual void OnTurnStarted() {
            this.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START);
            //     this.FighterPlaying.TriggerBuffs(BuffTriggerType.TURN_BEGIN, null);
            this.FighterPlaying.DecrementAllCastedBuffsDuration();
            this.FighterPlaying.DecrementBuffsDelay();
            this.FighterPlaying.DecrementMarkDurations();
            this.FighterPlaying.TriggerMarks(this.FighterPlaying, MarkTriggerTypeEnum.ON_TURN_STARTED);
            // this.TriggerMarks(this.FighterPlaying.Cell, this.FighterPlaying, TriggerType.TURN_BEGIN);

            this.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_TURN_START);

            this.CheckDeads();

            if (!this.CheckFightEnd()) {
                if (this.TimeLine.NewRound) {
                    this.UpdateRound();
                }

                if (!this.FighterPlaying.Alive) // || this.FighterPlaying.MustSkipTurn()) (buff => corruption enutrof)
                {
                    this.StopTurn();
                }
                else {
                    this.Send(new GameFightTurnStartMessage(this.FighterPlaying.Id, (uint) TurnTime * 10));

                    //  Synchronize(); a voir :/


                    //foreach (var fighter in GetFighters<CharacterFighter>())
                    //{
                    //    fighter.Character.RefreshStats();
                    //}

                    this.FighterPlaying.TurnStartCell = this.FighterPlaying.CellId;
                    this.FighterPlaying.TurnStartTime = DateTime.Now;

                    if (this.FighterPlaying.TriggerBuffs(TriggerType.TURN_BEGIN)) {
                        this.FighterPlaying.PassTurn();
                    }
                    else {
                        this.m_turnTimer = new ActionTimer((int) TurnTime * 1000, this.FighterPlaying.PassTurn, false);
                        this.m_turnTimer.Start();
                        this.FighterPlaying.OnTurnStarted();
                    }
                }
            }
        }

        public void PointsVariation(int sourceId, int targetId, ActionsEnum action, short delta) {
            this.Send(new GameActionFightPointsVariationMessage((ushort) action, sourceId, targetId, delta));
        }

        public void StopTurn() {
            if (!this.Ended) {
                if (this.m_turnTimer != null) {
                    this.m_turnTimer.Stop();
                }

                if (this.Synchronizer != null) {
                    //   this.logger.Alert("Last ReadyChecker was not disposed. (Stop Turn)");
                    this.Synchronizer.Cancel();
                    this.Synchronizer = null;
                }

                if (!this.CheckFightEnd()) {
                    this.OnTurnStopped();
                    this.Synchronizer = Synchronizer.RequestCheck(this, new Action(this.PassTurn), new Action<PlayableFighter[]>(this.LagAndPassTurn));
                }
            }
        }

        protected void LagAndPassTurn(PlayableFighter[] laggers) {
            //  this.OnLaggersSpotted(laggers);
            this.PassTurn();
        }

        protected virtual void OnLaggersSpotted(PlayableFighter[] laggers) {
            if (laggers.Length == 1) {
                this.Send(new TextInformationMessage((sbyte) TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                                                     28,
                                                     new string[] {
                                                         laggers[0].Name
                                                     }));
            }
            else {
                if (laggers.Length > 1) {
                    string[] array = new string[1];
                    array[0] = string.Join(",",
                                           from entry in laggers
                                           select entry.Name);
                    this.Send(new TextInformationMessage((sbyte) TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                                                         29,
                                                         array));
                }
            }
        }

        protected virtual void OnTurnStopped() {
            //Logger.Write<Fight>("Fin d'un tour!", ConsoleColor.DarkMagenta);

            //if (this.SequencesManager.SequencesCount > 1)
            //{
            //    logger.Error("Sequencing Error, (" + SequencesManager.SequencesCount + ") left...");
            //    OnFighters<CharacterFighter>(x => x.Character.ReplyError("Sequencing Error, (" + SequencesManager.SequencesCount + ") left..."));
            //}


            if (!this.CheckFightEnd()) {
                this.Send(new GameFightTurnEndMessage(this.FighterPlaying.Id));
            }

            this.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END);
            this.FighterPlaying.TriggerMarks(this.FighterPlaying, MarkTriggerTypeEnum.ON_TURN_ENDED);
            this.FighterPlaying.TriggerBuffs(TriggerType.TURN_END);
            this.FighterPlaying.OnTurnEnded();
            this.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_TURN_END);
        }

        private void PassTurn() {
            if (!this.Ended) {
                this.Synchronizer = null;


                this.SequencesManager.EndAllSequences();
                if (!this.CheckFightEnd()) {
                    if (!this.TimeLine.SelectNextFighter()) {
                        if (!this.CheckFightEnd()) {
                            this.logger.Error("Something goes wrong : no more actors are available to play but the fight is not ended");
                        }
                    }
                    else {
                        this.OnTurnPassed();
                        this.StartTurn();
                    }
                }
            }
        }

        public void UpdateTeams() {
            if (!this.Started) {
                this.BlueTeam.Update();
                this.RedTeam.Update();
            }
        }

        protected virtual void OnTurnPassed() { }

        public void UpdateTimeLine() {
            double[] ids = this.TimeLine.GetIds();
            this.Send(new GameFightTurnListMessage(ids, this.GetDeads()));
        }

        public Fighter[] GetSummons(bool aliveOnly = true) {
            return this.GetFighters<IOwnable>(aliveOnly).Cast<Fighter>().ToArray();
        }

        private double[] GetDeads() {
            Fighter[] deads = this.GetAllFighters(false).FindAll(x => !(x is IOwnable) && !x.Alive).ToArray();
            return Array.ConvertAll(deads, x => (double) x.Id);
        }

        public void ShowFighters(CharacterFighter fighter) {
            this.GetAllFighters().ForEach(x => x.ShowFighter(fighter));
        }

        public void UpdateFightersPlacementDirection() {
            foreach (var fighter in this.GetAllFighters()) {
                fighter.Direction = fighter.Team.FindPlacementDirection(fighter);
            }
        }

        public void UpdateEntitiesPositions() {
            List<IdentifiedEntityDispositionInformations> positions = new List<IdentifiedEntityDispositionInformations>();

            foreach (var fighter in this.GetAllFighters()) {
                positions.Add(fighter.GetIdentifiedEntityDispositionInformations());
            }

            this.Send(new GameEntitiesDispositionMessage(positions.ToArray()));
        }

        private void ShowBladesOnMap() {
            this.FindBladesPlacement();
            this.Map.Instance.AddFight(this);
        }

        private void FindBladesPlacement() {
            if (this.RedTeam.Leader.MapCellId != this.BlueTeam.Leader.MapCellId) {
                this.RedTeam.BladesCellId = this.RedTeam.Leader.MapCellId;
                this.BlueTeam.BladesCellId = this.BlueTeam.Leader.MapCellId;
            }
            else {
                var randomAdjacentFreeCell = this.Map.CloseCellWithoutEntitiesPositions(this.RedTeam.Leader.MapCellId);

                if (randomAdjacentFreeCell == 0) {
                    this.RedTeam.BladesCellId = this.RedTeam.Leader.MapCellId;
                }
                else {
                    this.RedTeam.BladesCellId = randomAdjacentFreeCell;
                }

                this.BlueTeam.BladesCellId = this.BlueTeam.Leader.MapCellId;
            }
        }

        public virtual void TryJoin(Character character, int leaderId) {
            FightTeam joinedTeam;

            if (this.BlueTeam.Leader.Id == leaderId)
                joinedTeam = this.BlueTeam;
            else if (this.RedTeam.Leader.Id == leaderId)
                joinedTeam = this.RedTeam;
            else {
                character.ReplyError("Unable to find a team to join...");
                return;
            }

            if (joinedTeam.Options.CanJoin(character)) {
                joinedTeam.AddFighter(character.CreateFighter(joinedTeam));
            }
        }

        public FightTeam GetTeam(TeamTypeEnum teamType) {
            if (this.BlueTeam.Type == teamType)
                return this.BlueTeam;
            if (this.RedTeam.Type == teamType)
                return this.RedTeam;

            throw new Exception("Unable to find team (" + teamType + ")");
        }

        public abstract void SendGameFightJoinMessage(CharacterFighter fighter);

        #endregion

        public virtual void EndFight() {
            //logger.Color2("Fin du combat...");

            if (this.FighterPlaying != null)
                this.FighterPlaying.OnFightEndedWhilePlaying();

            if (this.Started)
                this.DeterminsWinners();

            if (this.OnFightEndedEvt != null) this.OnFightEndedEvt(this, this.Started);

            if (this.Started) {
                this.Synchronizer = null;
                List<IFightResult> list = this.GenerateResults().ToList<IFightResult>();

                this.ApplyResults(list);
                this.Send(new GameFightEndMessage(this.GetFightDuration(),
                                                  this.AgeBonus,
                                                  0,
                                                  (from entry in list
                                                   select entry.GetFightResultListEntry()).ToArray(),
                                                  new NamedPartyTeamWithOutcome[0]));
            }

            foreach (CharacterFighter current in this.GetFighters<CharacterFighter>(false)) {
                bool winner = current.Team == this.Winners ? true : false;
                current.Character.Record.Stats.LifePoints = current.Stats.CurrentLifePoints;
                current.Character.RejoinMap(this.FightType, winner, this.SpawnJoin, current.Fight.FighterPlaying.Stats);

                current.Character.BeginHealthRegeneration();
            }


            this.Dispose();
        }

        protected void ApplyResults(IEnumerable<IFightResult> results) {
            foreach (IFightResult current in results) {
                current.Apply();
            }
        }

        protected abstract IEnumerable<IFightResult> GenerateResults();

        public int GetFightDuration() {
            return (!this.Started) ? 0 : ((int) (DateTime.Now - this.StartTime).TotalMilliseconds);
        }

        protected virtual void DeterminsWinners() {
            if (this.BlueTeam.Alives == 0) {
                this.Winners = this.RedTeam;
                this.Losers = this.BlueTeam;
            }
            else if (this.RedTeam.Alives == 0) {
                this.Winners = this.BlueTeam;
                this.Losers = this.RedTeam;
            }
        }

        public virtual void Dispose() {
            // Logger.Write<Fight>("Combat disposé!", ConsoleColor.DarkMagenta);
            if (this.m_turnTimer != null) this.m_turnTimer.Stop();

            if (this.m_placementTimer != null) this.m_placementTimer.Stop();

            if (this.Synchronizer != null) {
                this.Synchronizer.Cancel();
                this.Synchronizer = null;
            }

            this.RedTeam = null;
            this.BlueTeam = null;
            this.Map.Instance.RemoveFight(this);
            FightProvider.Instance.RemoveFight(this);
        }

        public FightTeam GetWinner() {
            if (this.BlueTeam.Alives == 0)
                return this.BlueTeam;
            else if (this.RedTeam.Alives == 0)
                return this.RedTeam;
            else {
                this.logger.Error("Try to determine winners while fight running...");
                return null;
            }
        }

        public short PopNextMarkId() {
            return (short) this.m_markIdPopper.Pop();
        }

        public int PopNextContextualId() {
            return this.m_contextualIdPopper.Pop();
        }

        public void AddMark(Mark mark) {
            bool seq = this.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            this.Marks.Add(mark);

            foreach (var fighter in this.GetFighters<CharacterFighter>(false)) {
                GameActionMark gameActionMark = null;
                if (mark.IsVisibleFor(fighter)) {
                    gameActionMark = mark.GetGameActionMark();
                }
                else {
                    gameActionMark = mark.GetHiddenGameActionMark();
                }

                fighter.Send(new GameActionFightMarkCellsMessage(0, mark.Source.Id, gameActionMark));
            }

            foreach (var fighter in this.GetAllFighters()) // dont work
            {
                if (mark.ContainsCell(fighter.CellId))
                    fighter.TriggerMarks(fighter, MarkTriggerTypeEnum.ON_CAST);
            }

            if (seq)
                this.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        public void RemoveMark(Fighter source, Mark mark) {
            this.Marks.Remove(mark);
            this.Send(new GameActionFightUnmarkCellsMessage(0, source.Id, mark.Id));
        }

        public T[] GetMarks<T>(Predicate<T> predicate) where T : Mark {
            return Array.FindAll(this.Marks.OfType<T>().ToArray(), predicate);
        }

        public T[] GetMarks<T>() where T : Mark {
            return this.Marks.OfType<T>().ToArray();
        }

        /// <summary>
        ///  this.TimeLine.RemoveFighter(fighter);
        ///  this.TimeLine.InsertFighter(fighter, TimeLine.Index + 1);
        /// changer l'index d'un fighter ? pas de fonction? 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fighter"></param>
        /// <param name="cellId"></param>
        /// <param name="lifePercent"></param>
        public void ReviveFighter(Fighter source, Fighter fighter, short cellId, short lifePercent) {
            fighter.Alive = true;
            fighter.DeathTime = null;
            fighter.CellId = cellId;
            fighter.Direction = source.Point.OrientationTo(fighter.Point);
            fighter.Stats.CurrentLifePoints = fighter.Stats.CurrentMaxLifePoints.GetPercentageOf(lifePercent);
            this.TimeLine.RemoveFighter(fighter);
            this.TimeLine.InsertFighter(fighter, this.TimeLine.Index + 1);
            this.Send(new GameActionFightSummonMessage(0, source.Id, new GameFightFighterInformations[] { fighter.GetFightFighterInformations() }));
            this.UpdateTimeLine();
        }

        public void AddSummon<T>(T fighter) where T : Fighter, IOwnable {
            this.AddSummons(new T[] { fighter });
        }

        public void AddSummons<T>(T[] fighters) where T : Fighter, IOwnable {
            foreach (var fighter in fighters) {
                ((ISummon<Fighter>) (fighter)).Owner.Team.AddFighter(fighter);

                if (fighter.InsertInTimeline())
                    this.TimeLine.InsertFighter(fighter, this.TimeLine.Index + 1);

                fighter.Initialize();
            }

            this.Send(new GameActionFightSummonMessage(0, ((ISummon<Fighter>) (fighters.First())).Owner.Id, Array.ConvertAll(fighters, x => x.GetFightFighterInformations())));
            this.UpdateTimeLine();
        }

        public void AddBomb<T>(T fighter, Fighter source) where T : BombFighter, ISummon<Fighter> {
            source.Team.AddFighter(fighter);
            fighter.Initialize();
            this.Send(new GameActionFightSummonMessage(0, source.Id, new GameFightFighterInformations[] { fighter.GetFightFighterInformations() }));
            BombProvider.Instance.UpdateWalls(fighter);
        }

        public void HighlightCell(Color color, short cellId) {
            this.Send(new DebugHighlightCellsMessage(color.ToArgb(),
                                                     new ushort[] {
                                                         (ushort) cellId
                                                     }));
        }

        public Wall AddWall(Fighter source, SpellLevelRecord level, EffectInstance effect, BombFighter firstBomb, BombFighter secondBomb, byte delta) {
            var direction = firstBomb.Point.OrientationTo(secondBomb.Point, true);
            Wall wall = new Wall((short) this.m_markIdPopper.Pop(),
                                 source,
                                 level,
                                 effect,
                                 firstBomb.Point.GetCellInDirection(direction, 1),
                                 Color.FromArgb(firstBomb.SpellBombRecord.WallColor),
                                 firstBomb,
                                 secondBomb,
                                 delta,
                                 direction);
            this.AddMark(wall);
            return wall;
        }

        public void AddSummon<T>(T fighter, CharacterFighter source) where T : Fighter, ISummon<CharacterFighter> {
            source.Team.AddFighter(fighter);
            if (fighter.InsertInTimeline()) {
                this.TimeLine.InsertFighter(fighter, this.TimeLine.Index + 1);
            }

            fighter.Initialize();
            this.Send(new GameActionFightSummonMessage(0, source.Id, new GameFightFighterInformations[] { fighter.GetFightFighterInformations() }));
            if (fighter.InsertInTimeline()) {
                this.UpdateTimeLine();
            }
        }

        public void ShowCell(Fighter source, ushort cellId) {
            this.Send(new ShowCellMessage(source.Id, cellId));
        }

        public bool ShouldTriggerOnMove(short cellId) {
            return this.Marks.Find(x => x.BreakMove && x.ContainsCell(cellId)) != null;
        }

        public void CheckDeads() {
            List<Fighter> fighters = this.GetAllFighters().FindAll(x => x.Alive == true && x.Stats.CurrentLifePoints <= 0);

            if (fighters.Count > 0) {
                Fighter playingFighter = null;

                bool seq = this.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

                foreach (var fighter in fighters) {
                    if (fighter.IsFighterTurn) {
                        playingFighter = fighter;
                    }

                    fighter.Die(fighter);
                }

                if (seq) this.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

                if (playingFighter != null) // && !Ended)
                    playingFighter.PassTurn();
            }
        }

        public virtual bool CheckFightEnd() {
            //  logger.Color2("Le combat se termine t-il?");
            if (this.BlueTeam.Alives == 0 || this.RedTeam.Alives == 0 && !this.Ended) {
                if (this.Started) {
                    if (this.Synchronizer != null) this.Synchronizer.Cancel();

                    this.Synchronizer = Synchronizer.RequestCheck(this, this.EndFight, delegate(PlayableFighter[] actors) { this.EndFight(); });
                }
                else
                    this.EndFight();

                this.Ended = true;
            }

            return this.Ended;
        }

        public bool IsCellFree(short cellId) {
            return this.Map.WalkableDuringFight((ushort) cellId) && this.GetFighter(cellId) == null;
        }

        public bool IsCellsFree(short[] cells) {
            foreach (var cell in cells) {
                if (!this.IsCellFree(cell))
                    return false;
            }

            return true;
        }

        public virtual Challenge GetChallenge(ushort id) {
            return null;
        }

        public void Send(Message message) {
            this.GetFighters<CharacterFighter>(false).ForEach(x => x.Character.Client.Send(message));
        }

        public void Reply(string message) {
            this.GetFighters<CharacterFighter>(false).ForEach(x => x.Character.Reply(message));
        }

        public T GetFirstFighter<T>(Func<T, bool> predicate, bool aliveOnly = true) where T : Fighter {
            return this.GetFighters<T>(aliveOnly).FirstOrDefault(predicate);
        }

        public Fighter GetFighter(MapPoint point) {
            return this.GetFighter(point.CellId);
        }

        public Fighter GetFighter(short cellid) {
            return this.GetAllFighters().Find(x => x.CellId == cellid);
        }

        public short RandomFreeCell() {
            return (short) Array.FindAll(this.Map.WalkableCells, x => this.IsCellFree((short) x)).Random();
        }

        public Fighter GetFighter(int contextualid) {
            return this.GetAllFighters().Find(x => x.Id == contextualid);
        }

        public List<T> GetFighters<T>(bool aliveOnly = true) {
            return this.GetAllFighters(aliveOnly).OfType<T>().ToList();
        }

        public List<Fighter> GetAllFighters(bool aliveOnly = true) {
            List<Fighter> fighters = new List<Fighter>();
            fighters.AddRange(this.RedTeam.GetFighters(aliveOnly));
            fighters.AddRange(this.BlueTeam.GetFighters(aliveOnly));
            return fighters;
        }

        public List<Fighter> GetAllFightersWithLeavers() {
            var fighters = this.GetAllFighters(false);
            fighters.AddRange(this.RedTeam.GetLeavers());
            fighters.AddRange(this.BlueTeam.GetLeavers());
            return fighters;
        }

        public void OnFighters<T1>(Action<T1> action) where T1 : Fighter {
            foreach (var fighter in this.GetAllFighters().OfType<T1>()) {
                action(fighter);
            }
        }

        public void TeleportFightersToInitialPosition(Fighter source) {
            bool sequence = this.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            foreach (var fighter in this.GetAllFighters()) {
                if (fighter.CellId != fighter.FightStartCell)
                    fighter.Teleport(source, new MapPoint(fighter.FightStartCell));
            }

            if (sequence) this.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
        }

        public bool CanBeSeen(short from, short to, bool throughEntities = false) {
            if (from == to)
                return true;

            var occupiedCells = new short[0];
            if (!throughEntities)
                occupiedCells = this.GetAllFighters(true).FindAll(x => x.BlockSight).Select(x => x.CellId).ToArray();

            var line = new LineSet(new MapPoint(from), new MapPoint(to));

            var list = line.EnumerateValidPoints().Skip(1);
            return !(from point in list
                     where to != point.CellId
                     let cell = point.CellId
                     where !this.Map.LineOfSight(cell) || !throughEntities && Array.IndexOf(occupiedCells, point.CellId) != -1
                     select point).Any();
        }

        public bool CanBeSeenOld(short from, short to, bool throughEntities = false) {
            bool result;

            if (from == to) {
                result = true;
            }
            else {
                short[] array = new short[0];
                if (!throughEntities) {
                    array = (
                                from x in this.GetAllFighters(true)
                                select x.CellId).ToArray<short>();
                }

                IEnumerable<MapPoint> cellsInLine = MapPoint.GetPoint(from).GetCellsInLine(MapPoint.GetPoint(to));
                foreach (MapPoint current in cellsInLine.Skip(1)) {
                    if (to != current.CellId) {
                        short cell = current.CellId;
                        if (!this.Map.LineOfSight(cell) || (!throughEntities && Array.IndexOf<short>(array, current.CellId) != -1)) {
                            result = false;
                            return result;
                        }
                    }
                }

                result = true;
            }

            return result;
        }


        public abstract FightCommonInformations GetFightCommonInformations();

        public virtual FightExternalInformations GetExternalInformations() {
            return new FightExternalInformations(this.Id,
                                                 (sbyte) this.FightType,
                                                 0,
                                                 false,
                                                 this.GetFightTeamLightInformations(),
                                                 new FightOptionsInformations[2] {
                                                     this.RedTeam.Options.GetFightOptionsInformations(), this.BlueTeam.Options.GetFightOptionsInformations()
                                                 });
        }

        public FightTeamLightInformations[] GetFightTeamLightInformations() {
            return new FightTeamLightInformations[2] {
                this.RedTeam.GetFightTeamLightInformations(), this.BlueTeam.GetFightTeamLightInformations()
            };
        }

        public FightTeamInformations[] GetFightTeamInformations() {
            return new FightTeamInformations[2] {
                this.RedTeam.GetFightTeamInformations(), this.BlueTeam.GetFightTeamInformations()
            };
        }

        public FightOptionsInformations[] GetFightOptionsInformations() {
            return new FightOptionsInformations[] {
                this.RedTeam.Options.GetFightOptionsInformations(), this.BlueTeam.Options.GetFightOptionsInformations()
            };
        }
    }
}