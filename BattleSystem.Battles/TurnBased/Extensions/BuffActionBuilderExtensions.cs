using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased.Actions.Targets;
using BattleSystem.Core.Actions.Buff;

namespace BattleSystem.Battles.TurnBased.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="BuffActionBuilder"/>.
    /// </summary>
    public static class BuffActionBuilderExtensions
    {
        /// <summary>
        /// Sets the built buff to target a single character chosen by the user.
        /// </summary>
        /// <param name="builder">The buff builder.</param>
        /// <param name="userInput">The user input.</param>
        public static BuffActionBuilder UserSelectsSingleTarget(this BuffActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built buff to target a single other character chosen by the user.
        /// </summary>
        /// <param name="builder">The buff builder.</param>
        /// <param name="userInput">The user input.</param>
        public static BuffActionBuilder UserSelectsSingleOtherTarget(this BuffActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleOtherActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built buff to target a single ally chosen by the user.
        /// </summary>
        /// <param name="builder">The buff builder.</param>
        /// <param name="userInput">The user input.</param>
        public static BuffActionBuilder UserSelectsSingleAlly(this BuffActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleAllyActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built buff to target a single enemy chosen by the user.
        /// </summary>
        /// <param name="builder">The buff builder.</param>
        /// <param name="userInput">The user input.</param>
        public static BuffActionBuilder UserSelectsSingleEnemy(this BuffActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleEnemyActionTargetCalculator(userInput));
        }
    }
}
