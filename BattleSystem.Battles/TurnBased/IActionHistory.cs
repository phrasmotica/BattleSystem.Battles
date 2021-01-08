using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Moves;

namespace BattleSystem.Battles.TurnBased
{
    public interface IActionHistory
    {
        /// <summary>
        /// Gets the turn counter.
        /// </summary>
        int TurnCounter { get; }

        /// <summary>
        /// Adds the given action to the move action history.
        /// </summary>
        /// <typeparam name="TSource">The type of the object that caused the action.</typeparam>
        /// <param name="result">The result of the action.</param>
        void AddAction<TSource>(IActionResult<TSource> result);

        /// <summary>
        /// Adds the given move use to the move use history.
        /// </summary>
        /// <param name="moveUse">The move use.</param>
        void AddMoveUse(MoveUse moveUse);

        /// <summary>
        /// Gets the number of times the given move has been used successfully
        /// by the given user since it last failed.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="user">The user.</param>
        int GetMoveConsecutiveSuccessCount(Move move, Character user);

        /// <summary>
        /// Gets the number of times the given action has been used successfully
        /// as part of a move by the given user since it last failed.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="user">The user.</param>
        int GetMoveActionConsecutiveSuccessCount(IAction action, Character user);

        /// <summary>
        /// Gets the most recent damage action from a move that affected the
        /// given character on the current turn.
        /// </summary>
        /// <param name="character">The affected character.</param>
        /// <param name="turnNumber">The turn number.</param>
        DamageActionResult<Move> LastMoveDamageResultAgainst(Character character);

        /// <summary>
        /// Starts the turn.
        /// </summary>
        void StartTurn();
    }
}
