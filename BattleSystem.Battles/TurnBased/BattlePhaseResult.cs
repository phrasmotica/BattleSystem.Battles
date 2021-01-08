using System.Collections.Generic;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Items;

namespace BattleSystem.Battles.TurnBased
{
    /// <summary>
    /// Represents the result of a battle phase, during which some actions might
    /// occur from non-character sources.
    /// </summary>
    public class BattlePhaseResult
    {
        /// <summary>
        /// Gets or sets the list of action results from items.
        /// </summary>
        public List<ActionUseResult<Item>> ItemActionsResults { get; private set; }

        /// <summary>
        /// Creates a new <see cref="BattlePhaseResult"/> instance.
        /// </summary>
        public BattlePhaseResult()
        {
            ItemActionsResults = new List<ActionUseResult<Item>>();
        }
    }
}
