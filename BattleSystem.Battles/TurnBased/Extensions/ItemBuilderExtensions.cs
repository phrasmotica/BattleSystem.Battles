using BattleSystem.Battles.TurnBased.Constants;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Items;

namespace BattleSystem.Battles.TurnBased.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ItemBuilder"/>.
    /// </summary>
    public static class ItemBuilderExtensions
    {
        /// <summary>
        /// Adds an start-of-turn action to the built item.
        /// </summary>
        /// <param name="action">The action.</param>
        public static ItemBuilder WithStartTurnAction(this ItemBuilder builder, IAction action)
        {
            return builder.WithTaggedAction(action, ActionTags.StartTurn);
        }

        /// <summary>
        /// Adds an end-of-turn action to the built item.
        /// </summary>
        /// <param name="action">The action.</param>
        public static ItemBuilder WithEndTurnAction(this ItemBuilder builder, IAction action)
        {
            return builder.WithTaggedAction(action, ActionTags.EndTurn);
        }
    }
}
