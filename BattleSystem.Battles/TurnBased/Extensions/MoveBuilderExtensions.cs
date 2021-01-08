using BattleSystem.Battles.TurnBased.Moves.Success;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Moves.Success;
using BattleSystem.Core.Random;

namespace BattleSystem.Battles.TurnBased.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="MoveBuilder"/>.
    /// </summary>
    public static class MoveBuilderExtensions
    {
        /// <summary>
        /// Sets the built move's success rate to decrease linearly each
        /// time it is used.
        /// </summary>
        /// <param name="builder">The move builder.</param>
        /// <param name="baseSuccess">The base success rate.</param>
        /// <param name="linearFactor">The linear factor.</param>
        /// <param name="minimumSuccessRate">The minimum success rate.</param>
        /// <param name="random">The random number generator.</param>
        /// <param name="failureResult">The result to return in the case of failure.</param>
        /// <param name="actionHistory">The action history.</param>
        public static MoveBuilder SuccessDecreasesLinearlyWithUses(
            this MoveBuilder builder,
            int baseSuccess,
            int linearFactor,
            int minimumSuccessRate,
            IRandom random,
            MoveUseResult failureResult,
            IActionHistory actionHistory)
        {
            return builder.WithSuccessCalculator(
                new DecreasesLinearlyWithUsesSuccessCalculator(
                    baseSuccess,
                    linearFactor,
                    minimumSuccessRate,
                    random,
                    failureResult,
                    actionHistory
                )
            );
        }
    }
}
