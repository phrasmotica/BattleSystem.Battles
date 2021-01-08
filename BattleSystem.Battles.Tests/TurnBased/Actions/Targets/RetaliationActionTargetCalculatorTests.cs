using System.Linq;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Battles.TurnBased.Actions.Targets;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Moves;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Actions.Targets
{
    /// <summary>
    /// Unit tests for <see cref="RetaliationActionTargetCalculator"/>.
    /// </summary>
    [TestFixture]
    public class RetaliationActionTargetCalculatorTests
    {
        [Test]
        public void Calculate_WithLastReceivedMoveDamageFromOther_Succeeds()
        {
            // Arrange
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
                    User = otherCharacters[0],
                    StartingHealth = 20,
                    EndingHealth = 10,
                });

            var calculator = new RetaliationActionTargetCalculator(actionHistory.Object);

            // Act
            var (success, targets) = calculator.Calculate(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(targets.Single(), Is.EqualTo(otherCharacters[0]));
            });
        }

        [Test]
        public void Calculate_WithLastReceivedMoveDamageFromSelf_Fails()
        {
            // Arrange
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
                    User = user,
                });

            var calculator = new RetaliationActionTargetCalculator(actionHistory.Object);

            // Act
            var (success, targets) = calculator.Calculate(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(targets, Is.Empty);
            });
        }

        [Test]
        public void Calculate_NoLastReceivedMoveDamage_Fails()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var calculator = new RetaliationActionTargetCalculator(new Mock<IActionHistory>().Object);

            // Act
            var (success, targets) = calculator.Calculate(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(targets, Is.Empty);
            });
        }
    }
}
