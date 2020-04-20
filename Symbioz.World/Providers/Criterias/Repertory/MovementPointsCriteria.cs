﻿using Symbioz.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Criterias.Repertory {
    [Criteria("CM")]
    class MovementPointsCriteria : AbstractCriteria {
        public override bool Eval(WorldClient client) {
            return BasicEval(this.CriteriaValue, this.ComparaisonSymbol, client.Character.Record.Stats.MovementPoints.Total());
        }
    }
}