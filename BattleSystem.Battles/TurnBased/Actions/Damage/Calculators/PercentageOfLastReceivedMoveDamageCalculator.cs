using System;
using System.Collections.Generic;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Actions.Damage.Calculators;
using BattleSystem.Core.Characters;

namespace BattleSystem.Battles.TurnBased.Actions.Damage.Calculators
{
    /// <summary>
    /// Calculates damage equal to a percentage of the amount of the last damage
    /// action from a move that affected the user.
    /// </summary>
    public class PercentageOfLastReceivedMoveDamageCalculator : IDamageCalculator
    {
        /// <summary>
        /// The percentage of the damage to deal.
        /// </summary>
        private readonly int _percentage;

        /// <summary>
        /// The action history.
        /// </summary>
        private readonly IActionHistory _actionHistory;

        /// <summary>
        /// Creates a new <see cref="PercentageOfLastReceivedMoveDamageCalculator"/> instance.
        /// </summary>
        /// <param name="percentage">The percentage of damage to deal.</param>
        /// <param name="actionHistory">The action history.</param>
        public PercentageOfLastReceivedMoveDamageCalculator(
            int percentage,
            IActionHistory actionHistory)
        {
            _percentage = percentage;
            _actionHistory = actionHistory;
        }

        /// <inheritdoc/>
        public IEnumerable<DamageCalculation> Calculate(Character user, DamageAction damage, IEnumerable<Character> targets)
        {
            var result = _actionHistory.LastMoveDamageResultAgainst(user);

            var calculations = new List<DamageCalculation>();

            foreach (var target in targets)
            {
                if (result is null)
                {
                    calculations.Add(new DamageCalculation
                    {
                        Target = target,
                        Success = false,
                        Amount = 0,
                    });
                }
                else
                {
                    calculations.Add(new DamageCalculation
                    {
                        Target = target,
                        Success = true,
                        Amount = Math.Max(1, result.Amount * _percentage / 100),
                    });
                }
            }

            return calculations;
        }
    }
}
