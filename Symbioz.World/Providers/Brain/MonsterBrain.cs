﻿using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Providers.Brain.Actions;
using Symbioz.World.Providers.Brain.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Brain {
    /// <summary>
    /// Représente l'intelligence d'un monstre.
    /// </summary>
    public class MonsterBrain {
        private Behavior Behavior { get; set; }

        public bool HasBehavior {
            get { return this.Behavior != null; }
        }

        public BrainFighter Fighter { get; private set; }

        public T GetBehavior<T>() where T : Behavior {
            return (T) this.Behavior;
        }

        public MonsterBrain(BrainFighter fighter) {
            this.Fighter = fighter;

            if (fighter.Template.BehaviorName != string.Empty && fighter.Template.BehaviorName != null)
                this.Behavior = BehaviorManager.GetBehavior(fighter.Template.BehaviorName, this.Fighter);
        }

        public void Play() {
            var actionTypes = EnvironmentAnalyser.Instance.GetSortedActions(this.Fighter);

            var actions = new List<BrainAction>();

            foreach (var action in actionTypes) {
                actions.Add(BrainProvider.GetAction(this.Fighter, action));
            }

            foreach (var action in actions) {
                action.Analyse();
            }

            foreach (var action in actions) {
                if (this.Fighter.Alive && this.Fighter.IsFighterTurn) {
                    action.Execute();
                }
                else
                    break;
            }
        }
    }
}