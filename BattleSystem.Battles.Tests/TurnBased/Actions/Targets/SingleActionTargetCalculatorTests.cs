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
    /// Unit tests for <see cref="SingleActionTargetCalculator"/>.
    /// </summary>
    [TestFixture]
    public class SingleActionTargetCalculatorTests
    {
        [Test]
        public void Calculate_Succeeds()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var userInput = new Mock<IUserInput>();
            userInput
                .Setup(m => m.SelectTarget(It.IsAny<IEnumerable<Character>>()))
                .Returns(otherCharacters[0]);

            var calculator = new SingleActionTargetCalculator(userInput.Object);

            // Act
            var (success, targets) = calculator.Calculate(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(targets.Single(), Is.EqualTo(otherCharacters[0]));
            });
        }
    }
}
