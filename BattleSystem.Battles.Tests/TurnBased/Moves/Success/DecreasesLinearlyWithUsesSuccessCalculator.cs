using System;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Battles.TurnBased.Moves.Success;
using BattleSystem.Core.Moves.Success;
using BattleSystem.Core.Random;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Moves.Success
{
    /// <summary>
    /// Unit tests for <see cref="DecreasesLinearlyWithUsesSuccessCalculator"/>.
    /// </summary>
    [TestFixture]
    public class DecreasesLinearlyWithUsesSuccessCalculatorTests
    {
        [Test]
        public void Ctor_NullRandom_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new DecreasesLinearlyWithUsesSuccessCalculator(
                    0,
                    0,
                    0,
                    null,
                    default,
                    new Mock<IActionHistory>().Object);
            });
        }

        [Test]
        public void Ctor_NullActionHistory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new DecreasesLinearlyWithUsesSuccessCalculator(
                    0,
                    0,
                    0,
                    new Mock<IRandom>().Object,
                    default,
                    null);
            });
        }

        [TestCase(85, MoveUseResult.Success)]
        [TestCase(95, MoveUseResult.Failure)]
        public void Calculate_WithPreviousMoveSuccess_InsideThreshold_ReturnsResult(
            int generatedValue,
            MoveUseResult expectedResult)
        {
            // Arrange
            var random = new Mock<IRandom>();
            random
                .Setup(m => m.Next(100))
                .Returns(generatedValue);

            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var move = TestHelpers.CreateMove();

            var actionHistory = new Mock<IActionHistory>();
            actionHistory
                .Setup(m => m.GetMoveConsecutiveSuccessCount(move, user))
                .Returns(1);

            var calculator = new DecreasesLinearlyWithUsesSuccessCalculator(
                100,
                10,
                0,
                random.Object,
                MoveUseResult.Failure,
                actionHistory.Object);

            // Act
            var result = calculator.Calculate(user, move, otherCharacters);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(85, MoveUseResult.Success)]
        [TestCase(95, MoveUseResult.Failure)]
        public void Calculate_NoPreviousMoveSuccess_ReturnsResult(
            int generatedValue,
            MoveUseResult expectedResult)
        {
            // Arrange
            var random = new Mock<IRandom>();
            random
                .Setup(m => m.Next(100))
                .Returns(generatedValue);

            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var move = TestHelpers.CreateMove();

            var calculator = new DecreasesLinearlyWithUsesSuccessCalculator(
                90,
                10,
                0,
                random.Object,
                MoveUseResult.Failure,
                new Mock<IActionHistory>().Object);

            // Act
            var result = calculator.Calculate(user, move, otherCharacters);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
