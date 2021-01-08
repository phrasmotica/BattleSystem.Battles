using System.Linq;
using BattleSystem.Battles.TurnBased;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Moves;
using NUnit.Framework;

namespace BattleSystem.Battles.Tests.TurnBased
{
    /// <summary>
    /// Unit tests for <see cref="MoveProcessor"/>.
    /// </summary>
    [TestFixture]
    public class MoveProcessorTests
    {
        [TestCase(5, true, false)]
        [TestCase(0, false, true)]
        public void Push_CanUseMove_Succeeds(
            int remainingUses,
            bool expectedResult,
            bool expectedQueueIsEmpty)
        {
            // Arrange
            var moveUse = new MoveUse
            {
                Move = TestHelpers.CreateMove(maxUses: remainingUses),
            };

            var moveProcessor = new MoveProcessor();

            // Act
            var result = moveProcessor.Push(moveUse);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedResult));
                Assert.That(moveProcessor.MoveUseQueueIsEmpty, Is.EqualTo(expectedQueueIsEmpty));
            });
        }

        [Test]
        public void ApplyNext_QueueNotEmptyAndUserAlive_ReturnsAppliedMoveUse()
        {
            // Arrange
            var moveUse = new MoveUse
            {
                Move = TestHelpers.CreateMove(),
                User = TestHelpers.CreateBasicCharacter(),
                OtherCharacters = Enumerable.Empty<Character>(),
            };

            var moveProcessor = new MoveProcessor();
            _ = moveProcessor.Push(moveUse);

            // Act
            var appliedMoveUse = moveProcessor.ApplyNext();

            // Assert
            Assert.That(appliedMoveUse.HasResult, Is.True);
        }

        [Test]
        public void ApplyNext_QueueNotEmptyAndUserDead_ReturnsUnappliedMoveUse()
        {
            // Arrange
            var moveUse = new MoveUse
            {
                Move = TestHelpers.CreateMove(),
                User = TestHelpers.CreateBasicCharacter(),
            };

            _ = moveUse.User.ReceiveDamage<Move>(6, TestHelpers.CreateBasicCharacter());

            var moveProcessor = new MoveProcessor();
            _ = moveProcessor.Push(moveUse);

            // Act
            var appliedMoveUse = moveProcessor.ApplyNext();

            // Assert
            Assert.That(appliedMoveUse.HasResult, Is.False);
        }

        [Test]
        public void ApplyNext_QueueEmpty_ReturnsNull()
        {
            Assert.That(new MoveProcessor().ApplyNext(), Is.Null);
        }
    }
}
