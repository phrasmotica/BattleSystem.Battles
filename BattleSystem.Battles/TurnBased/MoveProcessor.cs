using System.Collections.Generic;
using System.Linq;
using BattleSystem.Core.Moves;

namespace BattleSystem.Battles.TurnBased
{
    /// <summary>
    /// Class for managing the flow of a battle.
    /// </summary>
    public class MoveProcessor
    {
        /// <summary>
        /// The move use queue.
        /// </summary>
        private List<MoveUse> Queue;

        /// <summary>
        /// Gets whether the move use queue is empty.
        /// </summary>
        public bool MoveUseQueueIsEmpty => !Queue.Any();

        /// <summary>
        /// Creates a new <see cref="MoveProcessor"/> instance.
        /// </summary>
        public MoveProcessor()
        {
            Queue = new List<MoveUse>();
        }

        /// <summary>
        /// Tries to add the given move use to the move use queue and returns whether the attempt succeeded.
        /// </summary>
        /// <param name="moveUse">The move use.</param>
        public bool Push(MoveUse moveUse)
        {
            if (moveUse.Move.CanUse())
            {
                Queue.Add(moveUse);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Processes the next move use and returns info about it.
        /// </summary>
        public MoveUse ApplyNext()
        {
            if (MoveUseQueueIsEmpty)
            {
                return null;
            }

            SortMoveUseQueue();

            var moveUse = Queue[0];

            if (!moveUse.User.IsDead)
            {
                moveUse.Apply();
            }

            Queue.RemoveAt(0);

            return moveUse;
        }

        /// <summary>
        /// Sorts the move use queue into the order in which the moves should be applied.
        /// </summary>
        private void SortMoveUseQueue()
        {
            var sortedQueue = Queue.OrderByDescending(m => m.Move.Priority) // moves with higher priority go first
                                   .ThenByDescending(m => m.User.CurrentSpeed); // characters with higher speed go first

            Queue = sortedQueue.ToList();
        }
    }
}
