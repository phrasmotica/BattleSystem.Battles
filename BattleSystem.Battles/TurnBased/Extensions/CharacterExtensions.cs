using System.Collections.Generic;
using BattleSystem.Battles.TurnBased.Constants;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Items;

namespace BattleSystem.Battles.TurnBased.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Character"/> and its sub-types.
    /// </summary>
    public static class CharacterExtensions
    {
        /// <summary>
        /// Starts this character's turn.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="otherCharacters">The other characters.</param>
        public static BattlePhaseResult OnStartTurn(this Character character, IEnumerable<Character> otherCharacters)
        {
            var result = new BattlePhaseResult();

            character.ProcessCharacterTaggedActions(otherCharacters, ActionTags.StartTurn, result);

            return result;
        }

        /// <summary>
        /// Ends this character's turn.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="otherCharacters">The other characters.</param>
        public static BattlePhaseResult OnEndTurn(this Character character, IEnumerable<Character> otherCharacters)
        {
            var result = new BattlePhaseResult();

            character.ProcessCharacterTaggedActions(otherCharacters, ActionTags.EndTurn, result);
            character.ClearProtectQueue();

            return result;
        }

        /// <summary>
        /// Processes the character actions with the given tag for the given character.
        /// </summary>
        /// <param name="character">The character whose actions should be processed.</param>
        /// <param name="otherCharacters">The other characters.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="result">The encapsulating result.</param>
        private static void ProcessCharacterTaggedActions(
            this Character character,
            IEnumerable<Character> otherCharacters,
            string tag,
            BattlePhaseResult result)
        {
            if (character.HasItem)
            {
                var taggedActions = character.Item.GetCharacterTaggedActions(tag);
                foreach (var a in taggedActions)
                {
                    a.Action.SetTargets(character, otherCharacters);

                    var actionUseResult = a.Action.Use<Item>(character, otherCharacters);
                    foreach (var r in actionUseResult.Results)
                    {
                        r.Source = character.Item;
                    }

                    result.ItemActionsResults.Add(actionUseResult);
                }
            }
        }
    }
}
