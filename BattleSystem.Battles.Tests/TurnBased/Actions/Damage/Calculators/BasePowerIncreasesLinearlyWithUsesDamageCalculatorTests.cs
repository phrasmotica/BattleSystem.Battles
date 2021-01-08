using System;
using System.Linq;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Battles.TurnBased.Actions.Damage.Calculators;
using BattleSystem.Core.Random;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased.Actions.Damage.Calculators
{
    /// <summary>
    /// Unit tests for <see cref="BasePowerIncreasesLinearlyWithUsesDamageCalculator"/>.
    /// </summary>
    [TestFixture]
    public class BasePowerIncreasesLinearlyWithUsesDamageCalculatorTests
    {
        [Test]
        public void Ctor_NullRandom_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new BasePowerIncreasesLinearlyWithUsesDamageCalculator(
                    0,
                    0,
                    null,
                    new Mock<IActionHistory>().Object);
            });
        }

        [Test]
        public void Ctor_NullActionHistory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new BasePowerIncreasesLinearlyWithUsesDamageCalculator(
                    0,
                    0,
                    new Mock<IRandom>().Object,
                    null);
            });
        }

        [Test]
        public void Calculate_NoPreviousUses_UsesStartingBasePower()
        {
            // Arrange
            var random = new Mock<IRandom>();
            random
                .Setup(m => m.Next(80, 101))
                .Returns(100);

            var damage = TestHelpers.CreateDamageAction();

            var user = TestHelpers.CreateBasicCharacter(attack: 6);
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(defence: 5),
            };

            var calculator = new BasePowerIncreasesLinearlyWithUsesDamageCalculator(10, 5, random.Object, new Mock<IActionHistory>().Object);

            // Act
            var calculation = calculator.Calculate(user, damage, otherCharacters).Single();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(calculation.Success, Is.True);
                Assert.That(calculation.Amount, Is.EqualTo(10));
            });
        }

        [Test]
        public void Calculate_WithPreviousUses_UsesIncreasedBasePower()
        {
            // Arrange
            var random = new Mock<IRandom>();
            random
                .Setup(m => m.Next(80, 101))
                .Returns(100);

            var damage = TestHelpers.CreateDamageAction();

            var user = TestHelpers.CreateBasicCharacter(attack: 6);
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(defence: 5),
            };

            var actionHistory = new Mock<IActionHistory>();
            actionHistory
                .Setup(m => m.GetMoveActionConsecutiveSuccessCount(damage, user))
                .Returns(1);

            var calculator = new BasePowerIncreasesLinearlyWithUsesDamageCalculator(10, 5, random.Object, actionHistory.Object);

            // Act
            var calculation = calculator.Calculate(user, damage, otherCharacters).Single();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(calculation.Success, Is.True);
                Assert.That(calculation.Amount, Is.EqualTo(15));
            });
        }
    }
}
