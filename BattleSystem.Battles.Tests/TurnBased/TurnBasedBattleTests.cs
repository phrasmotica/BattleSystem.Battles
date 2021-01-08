using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Core.Actions.Damage.Calculators;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Characters.Targets;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased
{
    /// <summary>
    /// Unit tests for <see cref="TurnBasedBattle"/>.
    /// </summary>
    [TestFixture]
    public class TurnBasedBattleTests
    {
        [Test]
        public void Start_WhenFinished_BattleIsOver()
        {
            // Arrange

            // characters will attack for one damage each turn until they die
            var characters = new[]
            {
                CreateCharacter("a"),
                CreateCharacter("b"),
            };

            var battle = new TurnBasedBattle(
                new MoveProcessor(),
                new Mock<IActionHistory>().Object,
                new Mock<IGameOutput>().Object,
                characters);

            // Act
            battle.Start();

            // Assert
            Assert.That(battle.IsOver, Is.True);
        }

        private static Character CreateCharacter(string team)
        {
            return TestHelpers.CreateBasicCharacter(
                team: team,
                moveSet: TestHelpers.CreateMoveSet(
                    TestHelpers.CreateMove(
                        moveActions: new[]
                        {
                            TestHelpers.CreateDamageAction(
                                damageCalculator: new AbsoluteDamageCalculator(1),
                                actionTargetCalculator: new OthersActionTargetCalculator()
                            ),
                        }
                    )
                )
            );
        }
    }
}
