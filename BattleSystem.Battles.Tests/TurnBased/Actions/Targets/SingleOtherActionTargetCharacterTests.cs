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
    /// Unit tests for <see cref="SingleOtherActionTargetCalculator"/>.
    /// </summary>
    [TestFixture]
    public class SingleOtherActionTargetCalculatorTests
    {
        [Test]
        public void Calculate_WithMultipleOthers_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
                TestHelpers.CreateBasicCharacter(),
            };

            var userInput = new Mock<IUserInput>();
            userInput
                .Setup(m => m.SelectTarget(It.IsAny<IEnumerable<Character>>()))
                .Returns(otherCharacters[0]);

            var calculator = new SingleOtherActionTargetCalculator(userInput.Object);

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
        public void Calculate_WithOneOther_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var calculator = new SingleOtherActionTargetCalculator(new Mock<IUserInput>().Object);

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
        public void Calculate_WithNoOthers_Fails()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();

            var calculator = new SingleOtherActionTargetCalculator(new Mock<IUserInput>().Object);

            // Act
            var (success, targets) = calculator.Calculate(user, Enumerable.Empty<Character>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(targets, Is.Empty);
            });
        }
    }
}
