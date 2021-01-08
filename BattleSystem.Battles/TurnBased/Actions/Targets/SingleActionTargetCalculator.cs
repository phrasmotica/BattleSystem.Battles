using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abstractions.Control;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Characters.Targets;

namespace BattleSystem.Battles.TurnBased.Actions.Targets
{
    /// <summary>
    /// Lets the user choose a single action target.
    /// </summary>
    public class SingleActionTargetCalculator : IActionTargetCalculator
    {
        /// <summary>
        /// The user input.
        /// </summary>
        private readonly IUserInput _userInput;

        /// <summary>
        /// Creates a new <see cref="SingleActionTargetCalculator"/> instance.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        public SingleActionTargetCalculator(IUserInput userInput)
        {
            _userInput = userInput;
        }

        /// <inheritdoc />
        public bool IsReactive => false;

        /// <inheritdoc />
        public (bool success, IEnumerable<Character> targets) Calculate(Character user, IEnumerable<Character> otherCharacters)
        {
            var targets = otherCharacters.Append(user);

            return (true, new[] { _userInput.SelectTarget(targets) });
        }
    }
}
