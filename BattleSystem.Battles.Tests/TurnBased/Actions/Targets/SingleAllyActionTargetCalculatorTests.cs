using System.Linq;
using System.Collections.Generic;
using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased.Actions.Targets;
using BattleSystem.Core.Characters;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Actions.Targets
{
    /// <summary>
    /// Unit tests for <see cref="SingleAllyActionTargetCalculator"/>.
    /// </summary>
    [TestFixture]
    public class SingleAllyActionTargetCalculatorTests
    {
        [Test]
        public void Calculate_WithMultipleAllies_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "a"),
                TestHelpers.CreateBasicCharacter(team: "a"),
            };

            var userInput = new Mock<IUserInput>();
            userInput
                .Setup(m => m.SelectTarget(It.IsAny<IEnumerable<Character>>()))
                .Returns(otherCharacters[0]);

            var calculator = new SingleAllyActionTargetCalculator(userInput.Object);

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
        public void Calculate_WithOneAlly_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "a"),
                TestHelpers.CreateBasicCharacter(team: "b"),
            };

            var calculator = new SingleAllyActionTargetCalculator(new Mock<IUserInput>().Object);

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
        public void Calculate_WithNoAllies_Fails()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter(team: "a");
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(team: "b"),
                TestHelpers.CreateBasicCharacter(team: "b"),
            };

            var calculator = new SingleAllyActionTargetCalculator(new Mock<IUserInput>().Object);

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
