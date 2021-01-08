using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Actions.Damage.Calculators;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Characters.Targets;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Moves.Success;
using BattleSystem.Core.Random;
using BattleSystem.Core.Stats;
using Moq;

namespace BattleSystem.Battles.Tests
{
    /// <summary>
    /// Helper methods for unit tests.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Returns a basic character.
        /// </summary>
        public static BasicCharacter CreateBasicCharacter(
            string name = "yeti",
            string team = "a",
            int maxHealth = 5,
            int attack = 1,
            int defence = 1,
            int speed = 1,
            MoveSet moveSet = null,
            IRandom random = null)
        {
            return new BasicCharacter(
                name,
                team,
                maxHealth,
                CreateStatSet(attack, defence, speed),
                moveSet ?? CreateMoveSet(),
                random ?? new Mock<IRandom>().Object);
        }

        /// <summary>
        /// Returns a stat set with the given base values in each stat.
        /// </summary>
        private static StatSet CreateStatSet(int attack, int defence, int speed)
        {
            return new StatSet
            {
                Attack = CreateStat(attack),
                Defence = CreateStat(defence),
                Speed = CreateStat(speed),
            };
        }

        /// <summary>
        /// Returns a stat with the given base value.
        /// </summary>
        private static Stat CreateStat(int baseValue = 1)
        {
            return new Stat(baseValue);
        }

        /// <summary>
        /// Returns a move set with the given moves.
        /// </summary>
        public static MoveSet CreateMoveSet(params Move[] moves)
        {
            var moveSet = new MoveSet();

            foreach (var move in moves)
            {
                moveSet.AddMove(move);
            }

            return moveSet;
        }

        /// <summary>
        /// Returns a move with the given actions.
        /// </summary>
        public static Move CreateMove(
            string name = "yeti",
            string description = "amon",
            int maxUses = 5,
            ISuccessCalculator successCalculator = null,
            params IAction[] moveActions)
        {
            var builder = new MoveBuilder()
                            .Name(name)
                            .Describe(description)
                            .WithMaxUses(maxUses)
                            .WithSuccessCalculator(successCalculator ?? new AlwaysSuccessCalculator());

            foreach (var action in moveActions)
            {
                builder = builder.WithAction(action);
            }

            return builder.Build();
        }

        /// <summary>
        /// Returns a damage action.
        /// </summary>
        public static DamageAction CreateDamageAction(
            IDamageCalculator damageCalculator = null,
            IActionTargetCalculator actionTargetCalculator = null)
        {
            return new DamageActionBuilder()
                .WithDamageCalculator(damageCalculator ?? new Mock<IDamageCalculator>().Object)
                .WithActionTargetCalculator(actionTargetCalculator ?? new Mock<IActionTargetCalculator>().Object)
                .Build();
        }
    }
}
