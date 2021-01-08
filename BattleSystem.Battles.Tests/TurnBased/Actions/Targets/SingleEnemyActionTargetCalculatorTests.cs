using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased.Actions.Targets;
using BattleSystem.Core.Characters;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Actions.Targets
{
    /// <summary>
    /// Unit tests for <see cref="SingleEnemyActionTargetCalculator"/>.
    /// </summary>
    [TestFixture]
    public class SingleEnemyActionTargetCalculatorTests
    {
        [Test]
        public void Calculate_WithMultipleEnemies_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "b"),
                TestHelpers.CreateBasicCharacter(team: "b"),
            };

            var userInput = new Mock<IUserInput>();
            userInput
                .Setup(m => m.SelectTarget(It.IsAny<IEnumerable<Character>>()))
                .Returns(otherCharacters[0]);

            var calculator = new SingleEnemyActionTargetCalculator(userInput.Object);

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
        public void Calculate_WithOneEnemy_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "b"),
                TestHelpers.CreateBasicCharacter(team: "a"),
            };

            var calculator = new SingleEnemyActionTargetCalculator(new Mock<IUserInput>().Object);

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
        public void Calculate_WithNoEnemies_Fails()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "a"),
                TestHelpers.CreateBasicCharacter(team: "a"),
            };

            var calculator = new SingleEnemyActionTargetCalculator(new Mock<IUserInput>().Object);

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
