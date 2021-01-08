using BattleSystem.Battles.TurnBased;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Items;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Moves.Success;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BattleSystem.Battles.Tests.TurnBased
{
    /// <summary>
    /// Unit tests for <see cref="ActionHistory"/>.
    /// </summary>
    [TestFixture]
    public class ActionHistoryTests
    {
        [Test]
        public void AddAction_ItemSource_AddsAction()
        {
            // Arrange
            var result = new DamageActionResult<Item>();
            var actionHistory = new ActionHistory();

            // Act
            actionHistory.AddAction(result);

            // Assert
            Assert.That(actionHistory.ItemActions, Contains.Item((0, result)));
        }

        [Test]
        public void AddAction_OtherSource_AddsAction()
        {
            // Arrange
            var actionHistory = new ActionHistory();

            // Act
            var result = new DamageActionResult<Move>();
            actionHistory.AddAction(result);

            // Assert
            Assert.That(actionHistory.ItemActions, new NotConstraint(Contains.Item(result)));
        }

        [Test]
        public void GetMoveConsecutiveSuccessCount_NoFailures_ReturnsCount()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var move = TestHelpers.CreateMove();

            var actionHistory = new ActionHistory();

            var moveUse = new MoveUse
            {
                User = user,
                Move = move,
                Result = MoveUseResult.Success,
            };

            actionHistory.AddMoveUse(moveUse);
            actionHistory.AddMoveUse(moveUse);

            // Act
            var count = actionHistory.GetMoveConsecutiveSuccessCount(move, user);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void GetMoveConsecutiveSuccessCount_WithFailure_ReturnsCount()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var move = TestHelpers.CreateMove();

            var actionHistory = new ActionHistory();

            var successMoveUse = new MoveUse
            {
                User = user,
                Move = move,
                Result = MoveUseResult.Success,
            };

            var failureMoveUse = new MoveUse
            {
                User = user,
                Move = move,
                Result = MoveUseResult.Failure,
            };

            actionHistory.AddMoveUse(successMoveUse);
            actionHistory.AddMoveUse(failureMoveUse);
            actionHistory.AddMoveUse(successMoveUse);
            actionHistory.AddMoveUse(successMoveUse);

            // Act
            var count = actionHistory.GetMoveConsecutiveSuccessCount(move, user);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void GetMoveActionConsecutiveSuccessCount_NoFailures_ReturnsCount()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();

            var action = TestHelpers.CreateDamageAction();

            var actionHistory = new ActionHistory();
            actionHistory.AddMoveUse(CreateMoveUse(action, user, true));
            actionHistory.AddMoveUse(CreateMoveUse(action, user, true));

            // Act
            var count = actionHistory.GetMoveActionConsecutiveSuccessCount(action, user);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void GetMoveActionConsecutiveSuccessCount_WithFailures_ReturnsCount()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();

            var action = TestHelpers.CreateDamageAction();

            var actionHistory = new ActionHistory();
            actionHistory.AddMoveUse(CreateMoveUse(action, user, true));
            actionHistory.AddMoveUse(CreateMoveUse(action, user, false));
            actionHistory.AddMoveUse(CreateMoveUse(action, user, true));
            actionHistory.AddMoveUse(CreateMoveUse(action, user, true));

            // Act
            var count = actionHistory.GetMoveActionConsecutiveSuccessCount(action, user);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        private static MoveUse CreateMoveUse(IAction action, Character user, bool actionApplied)
        {
            return new MoveUse
            {
                ActionsResults = new[]
                {
                    new ActionUseResult<Move>
                    {
                        Results = new[]
                        {
                            new DamageActionResult<Move>
                            {
                                Action = action,
                                User = user,
                                Applied = actionApplied,
                            }
                        }
                    }
                }
            };
        }
    }
}
