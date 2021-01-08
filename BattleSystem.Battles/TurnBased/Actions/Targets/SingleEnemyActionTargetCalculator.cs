using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abstractions.Control;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Characters.Targets;

namespace BattleSystem.Battles.TurnBased.Actions.Targets
{
    /// <summary>
    /// Lets the user choose a single enemy as the action target.
    /// </summary>
    public class SingleEnemyActionTargetCalculator : IActionTargetCalculator
    {
        /// <summary>
        /// The user input.
        /// </summary>
        private readonly IUserInput _userInput;

        /// <summary>
        /// Creates a new <see cref="SingleEnemyActionTargetCalculator"/> instance.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        public SingleEnemyActionTargetCalculator(IUserInput userInput)
        {
            _userInput = userInput;
        }

        /// <inheritdoc />
        public bool IsReactive => false;

        /// <inheritdoc />
        public (bool success, IEnumerable<Character> targets) Calculate(Character user, IEnumerable<Character> otherCharacters)
        {
            var enemies = otherCharacters.Where(c => c.Team != user.Team).ToArray();

            if (!enemies.Any())
            {
                return (false, Enumerable.Empty<Character>());
            }

            if (enemies.Length == 1)
            {
                return (true, enemies);
            }

            return (true, new[] { _userInput.SelectTarget(enemies) });
        }
    }
}
