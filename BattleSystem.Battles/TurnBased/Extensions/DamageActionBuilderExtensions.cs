using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased.Actions.Damage.Calculators;
using BattleSystem.Battles.TurnBased.Actions.Tags;
using BattleSystem.Battles.TurnBased.Actions.Targets;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Random;

namespace BattleSystem.Battles.TurnBased.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DamageActionBuilder"/>.
    /// </summary>
    public static class DamageActionBuilderExtensions
    {
        /// <summary>
        /// Sets the built damage action to use percentage-of-last-received-move damage.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="percentage">The percentage of damage to deal.</param>
        /// <param name="actionHistory">The action history.</param>
        public static DamageActionBuilder PercentageOfLastReceivedMoveDamage(
            this DamageActionBuilder builder,
            int percentage,
            IActionHistory actionHistory)
        {
            return builder.WithDamageCalculator(new PercentageOfLastReceivedMoveDamageCalculator(percentage, actionHistory));
        }

        /// <summary>
        /// Sets the built damage action to use percentage-of-last-received-move damage.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="startingBasePower">The starting base power.</param>
        /// <param name="linearFactor">The linear factor.</param>
        /// <param name="random">The random number generator.</param>
        /// <param name="actionHistory">The action history.</param>
        public static DamageActionBuilder BasePowerIncreasesLinearlyWithUses(
            this DamageActionBuilder builder,
            int startingBasePower,
            int linearFactor,
            IRandom random,
            IActionHistory actionHistory)
        {
            return builder.WithDamageCalculator(new BasePowerIncreasesLinearlyWithUsesDamageCalculator(startingBasePower, linearFactor, random, actionHistory));
        }

        /// <summary>
        /// Sets the built damage action to target the user of the move
        /// containing the action that last affected the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="actionHistory">The action history.</param>
        public static DamageActionBuilder Retaliates(
            this DamageActionBuilder builder,
            IActionHistory actionHistory)
        {
            return builder.WithActionTargetCalculator(new RetaliationActionTargetCalculator(actionHistory))
                          .WithTag(DamageTags.Retaliation);
        }

        /// <summary>
        /// Sets the built damage action to target a single character chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleTarget(this DamageActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built damage action to target a single other character chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleOtherTarget(this DamageActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleOtherActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built damage action to target a single ally chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleAlly(this DamageActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleAllyActionTargetCalculator(userInput));
        }

        /// <summary>
        /// Sets the built damage action to target a single enemy chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleEnemy(this DamageActionBuilder builder, IUserInput userInput)
        {
            return builder.WithActionTargetCalculator(new SingleEnemyActionTargetCalculator(userInput));
        }
    }
}
