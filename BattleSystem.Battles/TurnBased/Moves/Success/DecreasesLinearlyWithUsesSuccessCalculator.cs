using System;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Random;
using BattleSystem.Core.Success;

namespace BattleSystem.Battles.TurnBased.Moves.Success
{
    /// <summary>
    /// Calculates a move's success rate as the base success rate minus a linear
    /// factor multiplied by the amount of times this action has been used
    /// successfully since it last failed, bounded by a minimum success rate.
    /// </summary>
    public class DecreasesLinearlyWithUsesSuccessCalculator : ISuccessCalculator<Move, MoveUseResult>
    {
        /// <summary>
        /// The base success rate.
        /// </summary>
        private readonly int _baseSuccessRate;

        /// <summary>
        /// The linear factor.
        /// </summary>
        private readonly int _linearFactor;

        /// <summary>
        /// The minimum success rate.
        /// </summary>
        private readonly int _minimumSuccessRate;

        /// <summary>
        /// The user of the move.
        /// </summary>
        private readonly Character _user;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly IRandom _random;

        /// <summary>
        /// The result to return in the case of failure.
        /// </summary>
        private readonly MoveUseResult _failureResult;

        /// <summary>
        /// The action history.
        /// </summary>
        private readonly IActionHistory _actionHistory;

        /// <summary>
        /// Creates a new <see cref="DecreasesLinearlyWithUsesSuccessCalculator"/> instance.
        /// </summary>
        /// <param name="baseSuccessRate">The starting chance of success.</param>
        /// <param name="linearFactor">The linear factor.</param>
        /// <param name="minimumSuccessRate">The minimum success rate</param>
        /// <param name="random">The random number generator.</param>
        /// <param name="failureResult">The failure result.</param>
        /// <param name="actionHistory">The action history.</param>
        public DecreasesLinearlyWithUsesSuccessCalculator(
            int baseSuccessRate,
            int linearFactor,
            int minimumSuccessRate,
            Character user,
            IRandom random,
            MoveUseResult failureResult,
            IActionHistory actionHistory)
        {
            _baseSuccessRate = baseSuccessRate;
            _linearFactor = linearFactor;
            _minimumSuccessRate = minimumSuccessRate;
            _user = user;
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _failureResult = failureResult;
            _actionHistory = actionHistory ?? throw new ArgumentNullException(nameof(actionHistory));
        }

        /// <inheritdoc />
        public MoveUseResult Calculate(Move move)
        {
            var count = _actionHistory.GetMoveConsecutiveSuccessCount(move, _user);
            var chance = Math.Max(_minimumSuccessRate, _baseSuccessRate - _linearFactor * count);
            var r = _random.Next(100) + 1;
            return r <= chance ? MoveUseResult.Success : _failureResult;
        }
    }
}
