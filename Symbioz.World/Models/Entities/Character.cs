using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Handlers.Approach;
using Symbioz.World.Models.Dialogs;
using Symbioz.World.Models.Dialogs.DialogBox;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities.HumanOptions;
using Symbioz.World.Models.Entities.Jobs;
using Symbioz.World.Models.Entities.Look;
using Symbioz.World.Models.Entities.Shortcuts;
using Symbioz.World.Models.Entities.Stats;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Models.Items;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Parties;
using Symbioz.World.Network;
using Symbioz.World.Providers.Arena;
using Symbioz.World.Providers.Guilds;
using Symbioz.World.Providers.Items;
using Symbioz.World.Providers.Maps.Interactives;
using Symbioz.World.Providers.Maps.Path;
using Symbioz.World.Providers.Parties;
using Symbioz.World.Records;
using Symbioz.World.Records.Almanach;
using Symbioz.World.Records.Breeds;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Maps;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Npcs;

namespace Symbioz.World.Models.Entities {
    public class Character : Entity {
        public WorldClient Client { get; set; }

        public CharacterRecord Record { get; private set; }

        public ushort ExpMultiplicator {
            get { return (ushort) this.Client.Characters.Count(x => ExperienceRecord.GetCharacterLevel(x.Exp) > this.Level); }
        }


        public BreedRecord Breed {
            get { return BreedRecord.GetBreed(this.Record.BreedId); }
        }

        public bool Riding {
            get { return this.Look.IsRiding; }
        }

        public bool ChangeMap { get; set; }
        public SpellShortcutBar SpellShortcutBar { get; private set; }
        public GeneralShortcutBar GeneralShortcutBar { get; private set; }
        public bool Collecting { get; set; }
        public PlayableFighter Fighter { get; private set; }
        public CharacterFighter FighterMaster { get; private set; }

        public bool Fighting {
            get { return this.Fighter != null; }
        }

        public int FighterCount {
            get {
                return this.MinationCount() + 1; // + companion?
            }
        }

        public bool InArena {
            get { return this.ArenaMember != null; }
        }

        public ArenaMember ArenaMember { get; private set; }

        /// <summary>
        /// Last Map before enter arena
        /// </summary>
        public int? PreviousRoleplayMapId { get; set; }

        public bool CanRegisterArena {
            get { return this.InRoleplay && !this.InArena; }
        }

        public bool HasGuild {
            get { return this.Record.GuildId != 0; }
        }

        public GuildInstance Guild { get; set; }

        public GuildMemberInstance GuildMember {
            get { return this.Guild.GetMember(this.Id); }
        }

        public Inventory Inventory { get; private set; }
        public Dialog Dialog { get; set; }
        public RequestBox RequestBox { get; set; }
        public ushort[] SkillsAllowed { get; private set; }

        private Timer HealthRegenerationTimer = null;

        private CharacterHumanOptionOrnament ActiveOrnament {
            get { return this.GetFirstHumanOption<CharacterHumanOptionOrnament>(); }
        }

        private CharacterHumanOptionTitle ActiveTitle {
            get { return this.GetFirstHumanOption<CharacterHumanOptionTitle>(); }
        }

        public AbstractParty Party { get; set; }

        public bool HasParty() {
            if (this.Party != null && this.Party.Members.Contains(this))
                return true;
            return false;
        }

        public bool HadBlockOtherPartiesInvitations { get; set; }

        public List<AbstractParty> GuestedParties { get; set; }

        public bool IsMute { get; set; }

        public T GetRequestBox<T>() where T : RequestBox {
            return (T) this.RequestBox;
        }

        public T GetDialog<T>() where T : Dialog {
            return (T) this.Dialog;
        }

        public void OpenDialog(Dialog dialog, bool force = false) {
            if (!this.Busy || force) {
                try {
                    this.Dialog = dialog;
                    this.Dialog.Open();
                }
                catch {
                    this.ReplyError("Impossible d'éxecuter l'action.");
                    this.LeaveDialog();
                }
            }
            else {
                this.ReplyError("Unable to open dialog while busy...");
            }
        }

        public void SwapFighter(PlayableFighter newFighter) {
            this.Fighter = newFighter;
        }

        public void SwapFighterToMaster() {
            this.Fighter = this.FighterMaster;
        }

        public PlayableFighter CreateFighter(FightTeam team) {
            if (this.Look.RemoveAura()) this.RefreshActorOnMap();

            this.StopHealthRegeneration();

            this.MovementKeys = null;
            this.IsMoving = false;
            this.Map.Instance.RemoveEntity(this);
            this.DestroyContext();
            this.CreateContext(GameContextEnum.FIGHT);
            // this.RefreshStats();
            this.Client.Send(new GameFightStartingMessage((sbyte) team.Fight.FightType, team.Fight.BlueTeam.Id, team.Fight.RedTeam.Id));
            this.FighterMaster = new CharacterFighter(this, team, this.CellId);
            this.Fighter = this.FighterMaster;

            if (team.Fight.MinationAllowed)
                this.ApplyMination(this.FighterMaster, team);

            return this.Fighter;
        }

        private void ApplyMination(CharacterFighter master, FightTeam team) {
            CharacterItemRecord[] items = this.Inventory.GetEquipedMinationItems();

            foreach (var item in items) {
                EffectMination effect = item.FirstEffect<EffectMination>();
                EffectMinationLevel effectLevel = item.FirstEffect<EffectMinationLevel>();

                if (effectLevel == null) // to remove (axiom)
                {
                    effectLevel = new EffectMinationLevel(1, 0, 0);
                    item.AddEffect(effectLevel);
                }

                var fighter = new MinationMonsterFighter(team,
                                                         MonsterRecord.GetMonster(effect.MonsterId),
                                                         effect.GradeId,
                                                         effectLevel.Level,
                                                         master,
                                                         team.GetPlacementCell());

                team.AddFighter(fighter);
                fighter.SetLife(effectLevel.Level * 20, true);
            }
        }

        private int MinationCount() {
            return Array.FindAll(this.Inventory.GetEquipedItems(), x => x.HasEffect<EffectMination>()).Count();
        }

        public FighterRefusedReasonEnum CanRequestFight(Character target) {
            FighterRefusedReasonEnum result;
            if (target.Fighting || target.Busy) {
                result = FighterRefusedReasonEnum.OPPONENT_OCCUPIED;
            }
            else {
                if (this.Fighting || this.Busy) {
                    result = FighterRefusedReasonEnum.IM_OCCUPIED;
                }
                else {
                    if (target == this) {
                        result = FighterRefusedReasonEnum.FIGHT_MYSELF;
                    }
                    else {
                        if (this.ChangeMap || target.ChangeMap || target.Map != this.Map || !this.Map.Position.AllowFightChallenges || !this.Map.ValidForFight) {
                            result = FighterRefusedReasonEnum.WRONG_MAP;
                        }
                        else {
                            result = FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
                        }
                    }
                }
            }

            return result;
        }

        public FighterRefusedReasonEnum CanAgress(Character target) {
            if (target.Client.Ip == this.Client.Ip && this.Client.Account.Role <= ServerRoleEnum.Animator) {
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;
            }

            if (target.Busy) {
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;
            }

            if (target == this) {
                return FighterRefusedReasonEnum.FIGHT_MYSELF;
            }

            if (this.Level - target.Level > 20) {
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;
            }

            if (!this.Map.Position.AllowAggression) {
                return FighterRefusedReasonEnum.WRONG_MAP;
            }

            if (target.Record.Alignment.Side == this.Record.Alignment.Side) {
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;
            }

            if (this.Busy) {
                return FighterRefusedReasonEnum.IM_OCCUPIED;
            }

            if (!this.InRoleplay) {
                return FighterRefusedReasonEnum.TOO_LATE;
            }

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public long GetRewardExperienceFromPercentage(int percentage) {
            long result = (long) (this.UpperBoundExperience * (percentage / 100d));
            result = result / this.Level;
            return result;
        }

        public bool CanAlmanach(AlmanachRecord almanach) {
            return this.Record.LastAlmanachDay != almanach.Id;
        }

        public bool DoAlmanach(AlmanachRecord almanach) {
            if (this.Inventory.Exist(almanach.ItemGId, almanach.Quantity)) {
                this.Inventory.RemoveItem(this.Inventory.GetFirstItem(almanach.ItemGId, almanach.Quantity), almanach.Quantity);
                this.OnItemLost(almanach.ItemGId, almanach.Quantity);

                this.Inventory.AddItem((ushort) almanach.RewardItemGId, (uint) almanach.RewardItemQuantity);
                this.OnItemGained((ushort) almanach.RewardItemGId, (uint) almanach.RewardItemQuantity);
                long xp = this.GetRewardExperienceFromPercentage(almanach.XpRewardPercentage);

                if (this.Level < 200) {
                    this.AddExperience((ulong) xp);
                    this.OnExperienceGained(xp);
                    this.RefreshStats();
                }

                this.Record.LastAlmanachDay = almanach.Id;
                return true;
            }
            else {
                return false;
            }
        }

        public bool IsInDialog(DialogTypeEnum type) {
            if (this.Dialog == null)
                return false;
            return this.Dialog.DialogType == type;
        }

        public bool IsInExchange(ExchangeTypeEnum type) {
            var exchange = this.GetDialog<Exchange>();
            if (exchange != null)
                return exchange.ExchangeType == type;
            else
                return false;
        }

        public void AcceptRequest() {
            if (this.IsInRequest() && this.RequestBox.Target == this) {
                this.RequestBox.Accept();
            }
        }

        public void DenyRequest() {
            if (this.IsInRequest() && this.RequestBox.Target == this) {
                this.RequestBox.Deny();
            }
        }

        public void CancelRequest() {
            if (!this.IsInRequest()) return;

            if (this.IsRequestSource()) {
                this.RequestBox.Cancel();
            }
            else if (this.IsRequestTarget()) {
                this.DenyRequest();
            }
        }

        public bool IsInRequest() {
            return this.RequestBox != null;
        }

        public bool IsRequestSource() {
            return this.IsInRequest() && this.RequestBox.Source == this;
        }

        public bool IsRequestTarget() {
            return this.IsInRequest() && this.RequestBox.Target == this;
        }

        public bool Busy {
            get { return this.Dialog != null || this.RequestBox != null || this.ChangeMap || !this.CanInteract; }
        }

        public bool CanInteract { get; set; }

        public override long Id {
            get { return this.Record.Id; }
        }

        public override string Name {
            get { return this.Record.Name; }
        }

        public override ContextActorLook Look {
            get { return this.Record.Look; }
            set { this.Record.Look = value; }
        }

        private ushort m_level;


        public ushort Level {
            get { return this.m_level; }

            private set {
                this.m_level = value;
                this.LowerBoundExperience = ExperienceRecord.GetExperienceForLevel(this.Level).Player;
                this.UpperBoundExperience = ExperienceRecord.GetExperienceForNextLevel(this.Level).Player;
            }
        }

        public ulong Experience {
            get { return this.Record.Exp; }
            private set {
                this.Record.Exp = value;

                if (value >= this.UpperBoundExperience && this.Level < ExperienceRecord.MaxCharacterLevel || value < this.LowerBoundExperience) {
                    ushort level = this.Level;
                    this.Level = ExperienceRecord.GetCharacterLevel(this.Record.Exp);
                    int difference = (int) (this.Level - level);
                    this.OnLevelChanged(level, difference, true);
                }
            }
        }

        public void RefreshStats() {
            this.Client.Send(new CharacterStatsListMessage(this.Record.Stats.GetCharacterCharacteristics(this)));
        }

        public void Restat(bool addStatPoints = true) {
            this.Record.Restat(addStatPoints);
            this.RefreshStats();
        }

        public void SetDirection(DirectionsEnum direction) {
            this.Record.Direction = (sbyte) direction;
            this.SendMap(new GameMapChangeOrientationMessage(new ActorOrientation(this.Id, (sbyte) direction)));
        }

        public void AddFollower(ContextActorLook look) {
            CharacterHumanOptionFollowers followers = this.GetFirstHumanOption<CharacterHumanOptionFollowers>();

            if (followers != null) {
                followers.AddFollower(look);
            }
            else {
                this.AddHumanOption(new CharacterHumanOptionFollowers(look));
            }
        }

        public void RemoveFollower(ContextActorLook look) {
            CharacterHumanOptionFollowers followers = this.GetFirstHumanOption<CharacterHumanOptionFollowers>();

            if (followers == null) {
                new Logger().Error("Error while removing follower!");
                return;
            }

            followers.RemoveFollower(look);

            if (followers.Looks.Count == 0) this.RemoveHumanOption<CharacterHumanOptionFollowers>();
        }

        public void AddExperience(ulong amount) {
            this.Experience += amount;
        }

        public void SetLevel(ushort newLevel) {
            if (newLevel > ExperienceRecord.MaxCharacterLevel) {
                this.Reply("New level must be < " + ExperienceRecord.MaxCharacterLevel);
            }
            else {
                this.Experience = ExperienceRecord.GetExperienceForLevel(newLevel).Player;
            }
        }

        private void OnLevelChanged(ushort oldLevel, int amount, bool send) {
            if (send && this.Level > oldLevel) {
                this.SendMap(new CharacterLevelUpInformationMessage((byte) this.Level, this.Record.Name, (uint) this.Id));
                this.Client.Send(new CharacterLevelUpMessage((byte) this.Level));
            }

            this.CheckSpells();

            if (this.Level > oldLevel) {
                this.Record.Stats.LifePoints += (5 * amount);
                this.Record.Stats.MaxLifePoints += (5 * amount);
                this.Record.SpellPoints += (ushort) (amount);
                this.Record.StatsPoints += (ushort) (5 * amount);
            }
            else if (this.Level < oldLevel) {
                this.Record.Stats.LifePoints += (5 * amount);
                this.Record.Stats.MaxLifePoints += (5 * amount);
                this.Record.StatsPoints = (ushort) (this.Level * 5 - 5);
                this.CheckRemovedSpells();
                this.Inventory.UnequipAll();
            }

            if (oldLevel < 100 && this.Level >= 100) {
                this.LearnEmote((byte) EmotesEnum.PowerAura);
                this.Record.Stats.ActionPoints.Base += 1;
                this.LearnOrnament((ushort) OrnamentsEnum.Hundred, send);
            }

            if (oldLevel < 160 && this.Level >= 160) {
                this.LearnOrnament((ushort) OrnamentsEnum.HundredSixty, send);
            }

            if (oldLevel < 200 && this.Level == 200) {
                this.LearnOrnament((ushort) OrnamentsEnum.TwoHundred, send);
            }

            if (this.HasParty()) {
                this.Party.UpdateMember(this);
            }

            if (send) {
                this.Record.Stats.LifePoints = this.Record.Stats.MaxLifePoints;
                this.RefreshActorOnMap();
                this.RefreshStats();
            }
        }

        public ulong LowerBoundExperience { get; private set; }
        public ulong UpperBoundExperience { get; private set; }

        private bool New { get; set; }

        private GameContextEnum? m_context { get; set; }

        public GameContextEnum? Context {
            get { return this.m_context; }
        }

        public bool InRoleplay {
            get { return this.Context.HasValue && this.Context.Value == GameContextEnum.ROLE_PLAY; }
        }

        public ushort MovedCell { get; set; }

        public ushort SubareaId {
            get {
                if (this.Map != null) {
                    return this.Map.SubAreaId;
                }
                else {
                    return 0;
                }
            }
        }

        public override ushort CellId {
            get { return this.Record.CellId; }
            set { this.Record.CellId = value; }
        }

        public override DirectionsEnum Direction {
            get { return (DirectionsEnum) this.Record.Direction; }
            set { this.Record.Direction = (sbyte) value; }
        }

        public bool IsMoving { get; private set; }

        public short[] MovementKeys { get; private set; }

        public Character(WorldClient client, CharacterRecord record, bool isNew) {
            this.Record = record;
            this.Client = client;
            this.New = isNew;
            this.GuestedParties = new List<AbstractParty>();
            this.Level = ExperienceRecord.GetCharacterLevel(record.Exp);
            this.Inventory = new Inventory(this);
            this.SpellShortcutBar = new SpellShortcutBar(this);
            this.GeneralShortcutBar = new GeneralShortcutBar(this);
            this.SkillsAllowed = SkillsProvider.Instance.GetAllowedSkills(this).ToArray();
            this.Party = null;
            this.CanInteract = true;

            if (isNew) {
                this.OnLevelChanged(1, this.Level - 1, false);
            }
            
            this.BeginHealthRegeneration();
            
            this.Client.Send(new LifePointsRegenEndMessage((uint) this.Record.Stats.LifePoints, (uint) this.Record.Stats.MaxLifePoints));
            this.Client.Send(new LifePointsRegenBeginMessage((byte) (WorldConfiguration.Instance.HealthRegenPerSecond*10)));
        }

        public void Deconstruct() {
            this.StopHealthRegeneration();
        }

        public void RefreshActorOnMap() {
            this.SendMap(new GameRolePlayShowActorMessage(this.GetActorInformations()));
        }

        public void RefreshActor() {
            this.Client.Send(new GameRolePlayShowActorMessage(this.GetActorInformations()));
        }

        public void CheckSpells() {
            foreach (var spell in this.Breed.GetSpellsForLevel(this.Level, this.Record.Spells)) {
                this.LearnSpell(spell);
            }

            this.RefreshSpells();
        }

        public void CheckRemovedSpells() {
            var spells2 = this.Breed.GetSpellsForLevel(200, new List<CharacterSpell>());
            var spells = this.Breed.GetSpellsForLevel(this.Level, new List<CharacterSpell>());


            foreach (var spell in this.Record.Spells.ToArray()) {
                var enumerable = spells as ushort[] ?? spells.ToArray();
                if (!enumerable.Contains(spell.SpellId) && spells2.Contains(spell.SpellId)) {
                    this.RemoveSpell(spell.SpellId);
                }
            }
        }

        public bool HasSpell(ushort spellId) {
            return this.Record.Spells.Find(x => x.SpellId == spellId) != null;
        }

        public void LearnSpell(ushort spellId) {
            if (!this.HasSpell(spellId)) {
                this.Record.Spells.Add(new CharacterSpell(spellId, 1));
                if (this.SpellShortcutBar.CanAdd()) {
                    this.SpellShortcutBar.Add(spellId);
                    this.RefreshShortcuts();
                }

                this.RefreshSpells();

                this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 3, spellId);
            }
        }

        public void RemoveSpell(ushort spellId) {
            if (this.HasSpell(spellId)) {
                this.Record.Spells.RemoveAll(x => x.SpellId == spellId);
                this.SpellShortcutBar.Remove(spellId);
                this.RefreshSpells();
                this.RefreshShortcuts();
            }
        }

        public CharacterSpell GetSpell(ushort spellId) {
            return this.Record.Spells.Find(x => x.SpellId == spellId);
        }

        public void ModifySpell(ushort spellId, sbyte gradeId) {
            if (!this.Fighting) {
                CharacterSpell actualSpell = this.GetSpell(spellId);

                if (actualSpell == null) {
                    this.Client.Send(new SpellModifyFailureMessage());
                    return;
                }

                int cost = actualSpell.Grade < gradeId
                               ? CharacterSpell.GetBoostCost(actualSpell.Grade, gradeId)
                               : CharacterSpell.GetBoostCost(gradeId, actualSpell.Grade);

                if (actualSpell.Grade < gradeId) {
                    if (cost <= this.Record.SpellPoints) {
                        this.Record.SpellPoints -= (ushort) cost;
                    }
                    else {
                        this.Client.Send(new SpellModifyFailureMessage());
                        return;
                    }
                }
                else {
                    if (actualSpell.Grade > gradeId) {
                        this.Record.SpellPoints += (ushort) cost;
                    }
                    else {
                        this.Client.Send(new SpellModifyFailureMessage());
                    }
                }

                actualSpell.SetGrade(gradeId);
                this.RefreshStats();
                this.Client.Send(new SpellModifySuccessMessage(spellId, gradeId));
            }
            else {
                this.Client.Send(new SpellModifyFailureMessage());
            }
        }

        public ShortcutBar GetShortcutBar(ShortcutBarEnum barEnum) {
            switch (barEnum) {
                case ShortcutBarEnum.GENERAL_SHORTCUT_BAR:
                    return this.GeneralShortcutBar;
                case ShortcutBarEnum.SPELL_SHORTCUT_BAR:
                    return this.SpellShortcutBar;
            }

            throw new Exception("Unknown shortcut bar, " + barEnum);
        }

        public void AddSpellPoints(ushort amount) {
            this.Record.SpellPoints += amount;
            this.RefreshStats();
        }

        public void RefreshShortcuts() {
            this.SpellShortcutBar.Refresh();
            this.GeneralShortcutBar.Refresh();
        }

        public void RefreshSpells() {
            this.Client.Send(new SpellListMessage(true, this.Record.Spells.ConvertAll(x => x.GetSpellItem()).ToArray()));
        }

        public void OnItemGained(ushort gid, uint quantity) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, new object[] { quantity, gid });
        }

        public void OnExperienceGained(long experience) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 8, new object[] { experience });
        }

        public void OnItemLost(ushort gid, uint quantity) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, new object[] { quantity, gid });
        }

        public void OnItemSelled(ushort gid, uint quantity, uint price) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 65, new object[] { price, string.Empty, gid, quantity });
        }

        public void OnKamasGained(int amount) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 45, new object[] { amount });
        }

        public void OnKamasLost(int amount) {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 46, new object[] { amount });
        }

        public void PlayEmote(byte id) {
            EmoteRecord template = EmoteRecord.GetEmote(id);

            if (!this.Collecting && !this.ChangeMap) {
                if (this.Look.RemoveAura()) this.RefreshActorOnMap();

                if (template.IsAura) {
                    ushort bonesId = template.AuraBones;

                    if (template.Id == (byte) EmotesEnum.PowerAura)
                        bonesId = (ushort) (this.Level >= 100 && this.Level != 200 ? 169 : 170);

                    this.Look.AddAura(bonesId);
                    this.RefreshActorOnMap();
                }
                else {
                    this.SendMap(new EmotePlayMessage(id, 0, this.Id, this.Client.Account.Id));
                }
            }
        }

        public void RandomTalkEmote() {
            byte[] talkEmotes = new byte[] { 49, 66, 9, 2, 10, 88 };
            this.PlayEmote(talkEmotes.Random());
        }

        public bool LearnEmote(byte id) {
            if (!this.Record.KnownEmotes.Contains(id)) {
                this.Record.KnownEmotes.Add(id);
                this.Client.Send(new EmoteAddMessage(id));
                return true;
            }
            else {
                this.Reply("Vous connaissez déja cette émote.");
                return false;
            }
        }

        public bool HasReachObjective(short id) {
            return this.Record.DoneObjectives.Contains(id);
        }

        public void ReachObjective(short id) {
            if (!this.Record.DoneObjectives.Contains(id)) {
                this.Record.DoneObjectives.Add(id);
                this.Reply("Nouvel objectif atteint.");
            }
        }

        public bool OnFightEnded(bool winner, FightTypeEnum type) {
            this.Inventory.DecrementEtherals();

            if (type == FightTypeEnum.FIGHT_TYPE_PVP_ARENA) {
                this.Teleport(this.PreviousRoleplayMapId.Value);
                this.PreviousRoleplayMapId = null;
                return true;
            }
            else if (winner && type == FightTypeEnum.FIGHT_TYPE_PvM) {
                EndFightActionRecord endFightAction = EndFightActionRecord.GetEndFightAction(this.Map.Id);
                if (endFightAction != null) {
                    this.Teleport(endFightAction.TeleportMapId, endFightAction.TeleportCellId);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveEmote(byte id) {
            if (this.Record.KnownEmotes.Contains(id)) {
                this.Record.KnownEmotes.Remove(id);
                this.Client.Send(new EmoteRemoveMessage(id));
                return true;
            }
            else {
                this.Reply("Impossible de retirer l'émote.");
                return false;
            }
        }

        public void RefreshEmotes() {
            this.Client.Send(new EmoteListMessage(this.Record.KnownEmotes.ToArray()));
        }

        public void OnEnterMap() {
            this.ChangeMap = false;
            this.UpdateServerExperience(this.Map.SubArea.ExperienceRate);

            if (this.Busy)
                this.LeaveDialog();

            if (!this.Fighting) // Teleport + Fight
            {
                lock (this.Map.Instance) {
                    this.Map.Instance.AddEntity(this);

                    this.Map.Instance.MapComplementary(this.Client);
                    this.Map.Instance.MapFightCount(this.Client);

                    foreach (Character current in this.Map.Instance.GetEntities<Character>()) {
                        if (current.IsMoving) {
                            this.Client.Send(new GameMapMovementMessage(current.MovementKeys, current.Id));
                            this.Client.Send(new BasicNoOperationMessage());
                        }
                    }

                    this.Client.Send(new BasicNoOperationMessage());
                    this.Client.Send(new BasicTimeMessage(DateTime.Now.DateTimeToUnixTimestamp(), 1));
                }
            }

            if (this.HasParty()) {
                this.Party.UpdateMember(this);
            }
        }

        public void RefreshGuild() {
            if (this.HasGuild) {
                this.Guild = GuildProvider.Instance.GetGuild(this.Record.GuildId);

                if (this.GuildMember == null || this.Guild == null) {
                    this.RemoveHumanOption<CharacterHumanOptionGuild>();
                    this.Record.GuildId = 0;
                    return;
                }

                this.GuildMember.OnConnected(this);
                this.SendGuildMembership();

                if (this.Guild.Record.Motd != null && this.Guild.Record.Motd.Content != null) {
                    this.Client.Send(new GuildMotdMessage(this.Guild.Record.Motd.Content,
                                                          this.Guild.Record.Motd.Timestamp,
                                                          this.Guild.Record.Motd.MemberId,
                                                          this.Guild.Record.Motd.MemberName));
                }
            }
        }

        public void SendGuildMembership() {
            this.Client.Send(new GuildMembershipMessage(this.Guild.Record.GetGuildInformations(), this.GuildMember.Record.Rights, true));
        }

        public void CreateContext(GameContextEnum context) {
            if (this.Context.HasValue) {
                this.DestroyContext();
            }

            this.Client.Send(new GameContextCreateMessage((sbyte) context));
            this.m_context = context;
        }

        public void DestroyContext() {
            this.Client.Send(new GameContextDestroyMessage());
            this.m_context = null;
        }

        public void UpdateServerExperience(int rate) {
            ushort percent = (ushort) (100 * (rate + this.ExpMultiplicator));
            this.Client.Send(new ServerExperienceModificatorMessage(percent));
        }

        public void TextInformation(TextInformationTypeEnum msgType, ushort msgId, params object[] parameters) {
            this.Client.Send(new TextInformationMessage((sbyte) msgType,
                                                        msgId,
                                                        (from entry in parameters
                                                         select entry.ToString()).ToArray()));
        }

        /// <summary>
        /// This is a wtf part, im not able to fix this weird bug (Module stop loading, ContextCreateMessage not received)
        /// </summary>
        public void SafeConnection() {
            ActionTimer timer = new ActionTimer(10000, this.CreateContextForced, false);
            timer.Start();
        }

        void CreateContextForced() {
            if (!this.Context.HasValue) {
                Logger.Write<Character>("Context Creation is Forced for " + this.Name, ConsoleColor.Green);
                ContextHandler.HandleCreateContextRequest(null, this.Client);
            }
        }

        public void OnConnected() {
            this.Client.Send(new AlmanachCalendarDateMessage(1));
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 89, new string[0]);
            this.Reply(WorldConfiguration.Instance.WelcomeMessage, Color.BlueViolet);

            foreach (var notifRecord in NotificationRecord.GetConnectionNotifications(this.Client.Account.Id)) {
                this.Reply(notifRecord.Notification);
                notifRecord.RemoveElement();
            }

            if (this.Inventory.HasMountEquiped) {
                this.Client.Send(new MountSetMessage(this.Inventory.Mount.GetMountClientData()));
            }
        }

        public void OpenPopup(byte lockDuration, string author, string content) {
            this.Client.Send(new PopupWarningMessage(lockDuration, author, content));
        }

        public void SpawnPoint(bool needComplementary = false) {
            this.TeleportZaap(this.Record.SpawnPointMapId, needComplementary);
        }

        /// <summary>
        /// Teleporte a une carte possédant un zaap
        /// </summary>
        /// <param name="mapId"></param>
        public void TeleportZaap(int mapId, bool needComplementary = false) {
            if (mapId != -1) {
                MapRecord destinationMap = MapRecord.GetMap(mapId);
                if (destinationMap.HasZaap())
                    this.Teleport(mapId, TeleporterDialog.GetTeleporterCell(destinationMap, destinationMap.Zaap), needComplementary);
                else {
                    this.ReplyError("No zaap at here, aborting teleportation.");
                    return;
                }
            }
            else {
                this.Teleport(WorldConfiguration.Instance.StartMapId,
                              (WorldConfiguration.Instance.StartCellId),
                              needComplementary);
            }

            this.Reply("Vous avez été téléporté.");
        }

        public void Reply(object value, Color color, bool bold = false, bool underline = false) {
            value = this.ApplyPolice(value, bold, underline);
            this.Client.Send(new TextInformationMessage(0, 0, new string[] { string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), value) }));
        }

        object ApplyPolice(object value, bool bold, bool underline) {
            if (bold)
                value = "<b>" + value + "</b>";
            if (underline)
                value = "<u>" + value + "</u>";
            return value;
        }

        public void Reply(object value, bool bold = false, bool underline = false) {
            value = this.ApplyPolice(value, bold, underline);
            this.Client.Send(new TextInformationMessage(0, 0, new string[] { value.ToString() }));
        }

        public void ReplyError(object value) {
            this.Reply(value, Color.DarkRed, false, false);
        }

        public void Notification(string message) {
            this.Client.Send(new NotificationByServerMessage(24, new string[] { message }, true));
        }

        public void Teleport(int mapId, ushort? cellid = null, bool needComplementary = false, bool force = false) {
            if (this.Fighting)
                return;
            if (this.ChangeMap)
                return;
            if (this.Busy && !force)
                return;

            if (this.Record.MapId != mapId) this.ChangeMap = true;

            var teleportMap = MapRecord.GetMap(mapId);

            if (teleportMap != null) {
                if (cellid < 0 || cellid > 560)
                    cellid = teleportMap.RandomWalkableCell();

                if (cellid.HasValue) {
                    if (!teleportMap.Walkable(cellid.Value)) {
                        cellid = teleportMap.RandomWalkableCell();
                    }
                }
                else {
                    if (!teleportMap.Walkable(this.CellId)) {
                        this.CellId = teleportMap.RandomWalkableCell();
                    }
                }

                if (this.Map != null && this.Map.Id == mapId && !needComplementary) {
                    if (cellid != null) {
                        this.SendMap(new TeleportOnSameMapMessage(this.Id, (ushort) cellid));
                        this.Record.CellId = cellid.Value;
                    }
                    else
                        this.SendMap(new TeleportOnSameMapMessage(this.Id, (ushort) this.Record.CellId));

                    this.MovementKeys = null;
                    this.IsMoving = false;
                    return;
                }

                if (this.Map != null) this.Map.Instance.RemoveEntity(this);

                this.MovementKeys = null;
                this.IsMoving = false;
                this.CurrentMapMessage(mapId);


                if (cellid != null)
                    this.Record.CellId = cellid.Value;
                this.Record.MapId = mapId;
            }
            else {
                this.Client.Character.ReplyError("The map does not exist...");
                this.ChangeMap = false;
            }
        }

        public void AddMinationExperience(ulong experienceFightDelta) {
            foreach (var item in this.Inventory.GetEquipedMinationItems()) {
                var effect = item.FirstEffect<EffectMinationLevel>();

                if (effect != null) {
                    var level = effect.Level;
                    effect.AddExperience(experienceFightDelta);

                    if (level != effect.Level) {
                        this.OnMinationLevelUp(item.FirstEffect<EffectMination>(), effect.Level);
                    }

                    this.Inventory.OnItemModified(item);
                }
            }
        }

        public void CurrentMapMessage(int mapId) {
            this.Client.Send(new CurrentMapMessage(mapId, WorldConfiguration.Instance.MapKey));
        }

        public void MoveOnMap(short[] cells) {
            if (!this.Busy) {
                ushort clientCellId = (ushort) PathParser.ReadCell(cells.First());

                if (clientCellId == this.CellId) {
                    if (this.Look.RemoveAura()) this.RefreshActorOnMap();
                    sbyte direction = PathParser.GetDirection(cells.Last());
                    ushort cellid = (ushort) PathParser.ReadCell(cells.Last());

                    this.Record.Direction = direction;
                    this.MovedCell = cellid;
                    this.IsMoving = true;
                    this.MovementKeys = cells;
                    this.SendMap(new GameMapMovementMessage(cells, this.Id));
                }
                else {
                    this.NoMove();
                }
            }
            else {
                this.NoMove();
            }
        }

        public void NoMove() {
            this.Client.Send(new GameMapNoMovementMessage((short) this.Point.X, (short) this.Point.Y));
        }

        public void OpenUIByObject(sbyte id, uint itemUid) {
            this.Client.Send(new ClientUIOpenedByObjectMessage(id, itemUid));
        }

        public void EndMove() {
            this.Record.CellId = this.MovedCell;
            this.MovedCell = 0;
            this.IsMoving = false;
            this.MovementKeys = null;

            DropItem item = this.Map.Instance.GetDroppedItem(this.Record.CellId);

            if (item != null) {
                item.OnPickUp(this);
            }
        }

        public void LeaveDialog() {
            if (this.Dialog == null && !this.IsInRequest()) {
                this.ReplyError("Unknown dialog...");
                return;
            }
            else {
                if (this.IsInRequest()) {
                    this.CancelRequest();
                }

                if (this.Dialog != null)
                    this.Dialog.Close();
            }
        }

        public void OpenPaddock() {
            this.OpenDialog(new MountStableExchange(this));
        }

        public void OpenBank() {
            this.OpenDialog(new BankExchange(this, BankItemRecord.GetBankItems(this.Client.Account.Id)));
        }

        public void OpenGuildCreationPanel() {
            this.OpenDialog(new GuildCreationDialog(this));
        }

        public void OpenNpcShop(Npc npc, ItemRecord[] itemToSell, ushort tokenId, bool priceLevel) {
            this.OpenDialog(new NpcShopExchange(this, npc, itemToSell, tokenId, priceLevel));
        }

        public void TalkToNpc(Npc npc, NpcActionRecord action) {
            this.OpenDialog(new NpcTalkDialog(this, npc, action));
        }

        public void OpenZaap(MapInteractiveElementSkill skill) {
            this.OpenDialog(new ZaapDialog(this, skill));
        }

        public void OpenZaapi(MapInteractiveElementSkill skill) {
            this.OpenDialog(new ZaapiDialog(this, skill));
        }

        public void OpenBidhouseSell(Npc npc, BidShopRecord bidshop, bool force) {
            this.OpenDialog(new SellExchange(this, npc, bidshop), force);
        }

        public void OpenBidhouseBuy(Npc npc, BidShopRecord bidshop, bool force) {
            this.OpenDialog(new BuyExchange(this, npc, bidshop), force);
        }

        public void OpenCraftPanel(uint skillId, JobsTypeEnum jobType) {
            this.OpenDialog(new CraftExchange(this, skillId, jobType));
        }

        public void OpenSmithMagicPanel(uint skillId, JobsTypeEnum jobType) {
            this.OpenDialog(new SmithMagicExchange(this, skillId, jobType));
        }

        public void ReadDocument(ushort documentId) {
            this.OpenDialog(new BookDialog(this, documentId));
        }

        public bool AddKamas(int value) {
            if (value <= int.MaxValue) {
                if (this.Record.Kamas + value >= Inventory.MAX_KAMAS) {
                    this.Record.Kamas = Inventory.MAX_KAMAS;
                }
                else
                    this.Record.Kamas += value;

                this.Inventory.RefreshKamas();
                return true;
            }

            return false;
        }

        public void RegisterArena() {
            this.ArenaMember = ArenaProvider.Instance.Register(this);
            this.ArenaMember.UpdateStep(true, PvpArenaStepEnum.ARENA_STEP_REGISTRED);
        }

        public void OnItemUnequipedArena() {
            this.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 298);
        }

        public void UnregisterArena() {
            if (this.InArena) {
                ArenaProvider.Instance.Unregister(this);
                this.ArenaMember.UpdateStep(false, PvpArenaStepEnum.ARENA_STEP_UNREGISTER);
                this.ArenaMember = null;
            }
            else {
                Logger.Write<Character>("Try to unregister arena while not in arena...", ConsoleColor.Red);
            }
        }

        public void AnwserArena(bool accept) {
            if (this.InArena) {
                this.ArenaMember.Anwser(accept);
            }
            else {
                Logger.Write<Character>("Unable to answer arena while not in arena...", ConsoleColor.Red);
            }
        }

        public void RefreshArenaInfos() {
            this.Client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(this.Record.ArenaRank.GetArenaRankInfos()));
        }

        public PlayerStatus GetPlayerStatus() {
            return new PlayerStatus(0);
        }

        public CharacterJob GetJob(JobsTypeEnum job) {
            return this.Record.Jobs.Find(x => x.JobType == job);
        }

        public void RefreshJobs() {
            this.Client.Send(new JobCrafterDirectorySettingsMessage(this.Record.Jobs.ConvertAll<JobCrafterDirectorySettings>(x => x.GetDirectorySettings()).ToArray()));
            this.Client.Send(new JobDescriptionMessage(this.Record.Jobs.ConvertAll<JobDescription>(x => x.GetJobDescription()).ToArray()));
            this.Client.Send(new JobExperienceMultiUpdateMessage(this.Record.Jobs.ConvertAll<JobExperience>(x => x.GetJobExperience()).ToArray()));
        }

        public void AddJobExp(JobsTypeEnum jobType, ulong amount) {
            CharacterJob job = this.GetJob(jobType);
            ushort currentLevel = job.Level;
            ulong highest = ExperienceRecord.HighestExperience().Job;

            if (job.Experience + amount > highest)
                job.Experience = highest;
            else
                job.Experience += amount;

            this.Client.Send(new JobExperienceUpdateMessage(job.GetJobExperience()));

            if (currentLevel != job.Level) {
                this.Client.Send(new JobLevelUpMessage((byte) job.Level, job.GetJobDescription()));
                this.SkillsAllowed = SkillsProvider.Instance.GetAllowedSkills(this).ToArray();
            }
        }

        public void SetSide(AlignmentSideEnum side) {
            this.Record.Alignment.Side = side;
            this.RefreshStats();
            this.RefreshActorOnMap();
        }

        public void AddHonor(ushort amount) {
            ushort highest = (ushort) ExperienceRecord.HighestHonorExperience().Honor;

            if (this.Record.Alignment.Honor + amount > highest)
                this.Record.Alignment.Honor = highest;
            else
                this.Record.Alignment.Honor += amount;


            this.RefreshActorOnMap();
            this.RefreshStats();
        }

        public void RemoveHonor(ushort amount) {
            if (this.Record.Alignment.Honor - amount < 0) {
                this.Record.Alignment.Honor = 0;
            }
            else {
                this.Record.Alignment.Honor -= amount;
            }

            this.RefreshActorOnMap();
            this.RefreshStats();
        }

        public AggressableStatusEnum TogglePvP() {
            if (this.Record.Alignment.Agressable == AggressableStatusEnum.NON_AGGRESSABLE) {
                this.Record.Alignment.Agressable = AggressableStatusEnum.PvP_ENABLED_AGGRESSABLE;
            }
            else if (this.Record.Alignment.Agressable == AggressableStatusEnum.PvP_ENABLED_AGGRESSABLE) {
                this.Record.Alignment.Agressable = AggressableStatusEnum.NON_AGGRESSABLE;
            }


            this.RefreshActorOnMap();
            this.RefreshStats();

            return this.Record.Alignment.Agressable;
        }

        public void RefreshAlignment() {
            this.Client.Send(new AlignmentRankUpdateMessage(this.Record.Alignment.Value, false));
        }

        public bool RemoveKamas(int value) {
            if (this.Record.Kamas >= value) {
                this.Record.Kamas -= value;
                this.Inventory.RefreshKamas();
                return true;
            }
            else {
                return false;
            }
        }

        private void AddHumanOption(CharacterHumanOption option) {
            this.Record.HumanOptions.Add(option);
            this.RefreshActorOnMap();
        }

        private void RemoveHumanOption(CharacterHumanOption option) {
            this.Record.HumanOptions.Remove(option);
            this.RefreshActorOnMap();
        }

        public void RemoveHumanOption<T>() where T : CharacterHumanOption {
            this.Record.HumanOptions.RemoveAll(x => x is T);
            this.RefreshActorOnMap();
        }

        private T GetFirstHumanOption<T>() where T : CharacterHumanOption {
            return this.Record.HumanOptions.OfType<T>().ToArray().FirstOrDefault();
        }

        public void SendTitlesAndOrnamentsList() {
            this.Client.Send(new TitlesAndOrnamentsListMessage(this.Record.KnownTitles.ToArray(),
                                                               this.Record.KnownOrnaments.ToArray(),
                                                               (ushort) (this.ActiveTitle != null ? this.ActiveTitle.TitleId : 0),
                                                               (ushort) (this.ActiveOrnament != null ? this.ActiveOrnament.OrnamentId : 0)));
        }

        public bool LearnOrnament(ushort id, bool send) {
            if (!this.Record.KnownOrnaments.Contains(id)) {
                this.Record.KnownOrnaments.Add(id);
                if (send) this.Client.Send(new OrnamentGainedMessage((short) id));
                return true;
            }
            else {
                return false;
            }
        }

        public void UseItem(uint uid, bool send) {
            var item = this.Inventory.GetItem(uid);

            if (item != null) {
                if (ItemUseProvider.Handle(this.Client.Character, item))
                    this.Inventory.RemoveItem(item.UId, 1);

                if (send) {
                    this.RefreshStats();
                }
            }
        }

        public bool ForgetOrnament(ushort id) {
            if (this.Record.KnownOrnaments.Contains(id)) {
                this.Record.KnownOrnaments.Remove(id);
                if (this.ActiveOrnament.OrnamentId == id) {
                    this.RemoveHumanOption<CharacterHumanOptionOrnament>();
                    this.RefreshActorOnMap();
                }

                return true;
            }

            return false;
        }

        public bool Mute(int seconds) {
            if (!this.Record.Muted) {
                this.Record.Muted = true;

                ActionTimer timer = new ActionTimer(seconds * 1000, new Action(() => { this.Record.Muted = false; }), false);
                timer.Start();

                return true;
            }
            else {
                return false;
            }
        }

        public bool HasOrnament(ushort id) {
            return this.Record.KnownOrnaments.Contains(id) ? true : false;
        }

        public void OpenRequestBox(RequestBox box) {
            box.Source.RequestBox = box;
            this.RequestBox = box;
            box.Open();
        }

        public void OnExchangeError(ExchangeErrorEnum error) {
            this.Client.Send(new ExchangeErrorMessage((sbyte) error));
        }

        public void OnPartyJoinError(PartyJoinErrorEnum error, int partyId = 0) {
            this.Client.Send(new PartyCannotJoinErrorMessage((uint) partyId, (sbyte) error));
        }

        /// <summary>
        /// Conditions & EnterParty (Stump)
        /// </summary>
        /// <param name="character"></param>
        public void InviteParty(Character character) {
            if (!this.HasParty()) {
                AbstractParty party = PartyProvider.Instance.CreateParty(this);
                PartyProvider.Instance.Parties.Add(party);
                party.Create(this, character);
            }
            else {
                if (!this.Party.IsFull)
                    AbstractParty.SendPartyInvitationMessage(character, this, this.Party);
            }
        }

        public bool SetOrnament(ushort id) {
            if (id == 0) {
                this.RemoveHumanOption<CharacterHumanOptionOrnament>();
                this.Client.Send(new OrnamentSelectedMessage(id));
                return true;
            }

            if (this.HasOrnament(id)) {
                if (this.ActiveOrnament != null && this.ActiveOrnament.OrnamentId == id)
                    return false;

                this.RemoveHumanOption<CharacterHumanOptionOrnament>();
                this.AddHumanOption(new CharacterHumanOptionOrnament(id));
                this.Client.Send(new OrnamentSelectedMessage(id));
                return true;
            }

            return false;
        }

        public bool LearnTitle(ushort id) {
            if (!this.Record.KnownTitles.Contains(id)) {
                this.Record.KnownTitles.Add(id);
                this.Client.Send(new TitleGainedMessage(id));
                return true;
            }

            return false;
        }

        public bool ForgetTitle(ushort id) {
            if (this.Record.KnownTitles.Contains(id)) {
                this.Record.KnownTitles.Remove(id);
                this.Client.Send(new TitleLostMessage(id));

                return true;
            }

            return false;
        }

        public bool HasTitle(ushort id) {
            return this.Record.KnownTitles.Contains(id) ? true : false;
        }

        public bool SelectTitle(ushort id) {
            if (id == 0) {
                this.RemoveHumanOption<CharacterHumanOptionTitle>();
                this.Client.Send(new TitleSelectedMessage(id));
                return true;
            }

            if (this.HasTitle(id)) {
                if (this.ActiveTitle != null && this.ActiveTitle.TitleId == id)
                    return false;

                this.RemoveHumanOption<CharacterHumanOptionTitle>();
                this.AddHumanOption(new CharacterHumanOptionTitle(id, string.Empty));
                this.Client.Send(new TitleSelectedMessage(id));
                return true;
            }

            return false;
        }

        public void PlayCinematic(ushort id) {
            this.Client.Send(new CinematicMessage(id));
        }

        public void OnContextCreated() {
            if (this.New && WorldConfiguration.Instance.PlayDefaultCinematic) {
                this.PlayCinematic(10);
                this.New = false;
            }
        }

        public override GameRolePlayActorInformations GetActorInformations() {
            return new GameRolePlayCharacterInformations(this.Id,
                                                         this.Look.ToEntityLook(),
                                                         new EntityDispositionInformations((short) this.CellId, (sbyte) this.Direction),
                                                         this.Name,
                                                         new HumanInformations(new ActorRestrictionsInformations(),
                                                                               this.Record.Sex,
                                                                               this.Record.HumanOptions.ConvertAll<HumanOption>(x => x.GetHumanOption()).ToArray()),
                                                         this.Client.Account.Id,
                                                         this.Record.Alignment.GetActorAlignmentInformations());
        }

        public CharacterMinimalInformations GetCharacterMinimalInformations() {
            return new CharacterMinimalInformations((ulong) this.Id, this.Name, (byte) this.Level);
        }

        public void OnChatError(ChatErrorEnum error) {
            this.Client.Send(new ChatErrorMessage((sbyte) error));
        }

        public void SetRestrictions() {
            this.Client.Send(new SetCharacterRestrictionsMessage(this.Id,
                                                                 new ActorRestrictionsInformations(false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   true,
                                                                                                   true,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false,
                                                                                                   false)));
        }

        public void RejoinMap(FightTypeEnum fightType, bool winner, bool spawnJoin, FighterStats stats) {
            this.DestroyContext();
            this.CreateContext(GameContextEnum.ROLE_PLAY);
            this.RefreshStats();
            this.Fighter = null;
            this.FighterMaster = null;
            if (spawnJoin && !winner /*&& this.Client.Account.Role != ServerRoleEnum.Fondator*/) {
                this.SpawnPoint(true);
            }
            else {
                if (!this.OnFightEnded(winner, fightType)) this.CurrentMapMessage(this.Record.MapId);
            }
        }

        public void OnDisconnected() {
            if (this.Dialog != null) this.Dialog.Close();
            if (this.IsInRequest()) this.CancelRequest();

            if (this.InArena) this.UnregisterArena();

            if (this.Fighting) this.FighterMaster.OnDisconnected();

            if (this.HasParty()) this.Party.Leave(this);

            if (this.HasGuild) this.GuildMember.OnDisconnected();

            this.DeclineAllPartyInvitations();

            this.Look.RemoveAura();
            if (this.Map != null) this.Map.Instance.RemoveEntity(this);

            this.Record.UpdateElement();
        }

        private void DeclineAllPartyInvitations() {
            foreach (var party in new List<AbstractParty>(this.GuestedParties)) {
                party.RefuseInvation(this);
            }
        }


        public void OnGuildCreated(GuildCreationResultEnum result) {
            this.Client.Send(new GuildCreationResultMessage((sbyte) result));
            this.Dialog.Close();
        }


        public override string ToString() {
            return "Character: (" + this.Name + ")";
        }

        public void OnGuildJoined(GuildInstance guild, GuildMemberInstance member) {
            this.Guild = guild;
            this.Record.GuildId = this.Guild.Id;
            this.AddHumanOption(new CharacterHumanOptionGuild(this.Guild.Record.GetGuildInformations()));
            this.Client.Send(new GuildJoinedMessage(this.Guild.Record.GetGuildInformations(),
                                                    member.Record.Rights,
                                                    true));
        }

        public void OnMinationLevelUp(EffectMination minationEffect, ushort newLevel) {
            this.OpenPopup(0, "Félicitation", "Votre Pokéfus " + minationEffect.MonsterName + " vient de passer niveau " + newLevel + "!");
        }

        public void BeginHealthRegeneration() {
            byte regenPerSecond = WorldConfiguration.Instance.HealthRegenPerSecond;
            int intervalMs = 1000/regenPerSecond;
            
            this.StopHealthRegeneration();

            this.HealthRegenerationTimer = new Timer(intervalMs);
            this.HealthRegenerationTimer.Elapsed += (sender, args) => this.RegenAction();
            this.HealthRegenerationTimer.Start();
        }

        public void StopHealthRegeneration() {
            if (this.HealthRegenerationTimer != null) {
                this.HealthRegenerationTimer?.Dispose();
                this.HealthRegenerationTimer = null;
                // this.Reply($"Stopping regen");
            }
        }
        
        private void RegenAction() {
            this.Record.Stats.LifePoints++;
            this.RefreshStats();
            // this.Reply($"Hp: {this.Record.Stats.LifePoints}");

            if (this.Record.Stats.LifePoints >= this.Record.Stats.MaxLifePoints) {
                this.StopHealthRegeneration();
            }
        }
    }
}