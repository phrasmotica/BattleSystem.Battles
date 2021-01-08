using System.Linq;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Battles.TurnBased.Actions.Damage.Calculators;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Moves;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Actions.Damage.Calculators
{
    /// <summary>
    /// Unit tests for <see cref="PercentageOfLastReceivedMoveDamageCalculator"/>.
    /// </summary>
    [TestFixture]
    public class PercentageOfLastReceivedMoveDamageCalculatorTests
    {
        [TestCase(50, 5)]
        [TestCase(10, 1)]
        [TestCase(5, 1)]
        public void Calculate_WithLastReceivedMoveDamage_ReturnsPercentageAmount(int percentage, int expectedAmount)
        {
            // Arrange
            var damage = TestHelpers.CreateDamageAction();

            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var actionHistory = new Mock<IActionHistory>();
            actionHistory
                .Setup(m => m.LastMoveDamageResultAgainst(user))
                .Returns(new DamageActionResult<Move>
                {
                    StartingHealth = 20,
                    EndingHealth = 10,
                });

            var calculator = new PercentageOfLastReceivedMoveDamageCalculator(percentage, actionHistory.Object);

            // Act
            var calculation = calculator.Calculate(user, damage, otherCharacters).Single();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(calculation.Success, Is.True);
                Assert.That(calculation.Amount, Is.EqualTo(expectedAmount));
            });
        }

        [Test]
        public void Calculate_NoLastReceivedMoveDamage_Fails()
        {
            // Arrange
            var damage = TestHelpers.CreateDamageAction();

            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var calculator = new PercentageOfLastReceivedMoveDamageCalculator(50, new Mock<IActionHistory>().Object);

            // Act
            var calculation = calculator.Calculate(user, damage, otherCharacters).Single();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(calculation.Success, Is.False);
                Assert.That(calculation.Amount, Is.Zero);
            });
        }
    }
}
