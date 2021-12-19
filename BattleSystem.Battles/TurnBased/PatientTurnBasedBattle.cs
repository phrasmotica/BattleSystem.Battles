using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased.Extensions;
using BattleSystem.Core.Characters;

namespace BattleSystem.Battles.TurnBased
{
    /// <summary>
    /// Class for processing a patient turn-based battle, which differs from a regular battle in
    /// that it waits to be advanced by an outside source.
    /// </summary>
    public class PatientTurnBasedBattle
    {
        /// <summary>
        /// The move processor.
        /// </summary>
        private readonly MoveProcessor _moveProcessor;

        /// <summary>
        /// The game output.
        /// </summary>
        private readonly IGameOutput _gameOutput;

        /// <summary>
        /// The characters in the battle.
        /// </summary>
        private readonly IEnumerable<Character> _characters;

        /// <summary>
        /// Gets the teams involved in the battle.
        /// </summary>
        private IEnumerable<IGrouping<string, Character>> Teams => _characters.GroupBy(c => c.Team);

        /// <summary>
        /// The action history for the battle.
        /// </summary>
        private readonly IActionHistory _actionHistory;

        /// <summary>
        /// Creates a new <see cref="TurnBasedBattle"/> instance.
        /// </summary>
        /// <param name="moveProcessor">The move processor.</param>
        /// <param name="actionHistory">The action history.</param>
        /// <param name="gameOutput">The game output.</param>
        /// <param name="characters">The characters in the battle.</param>
        public PatientTurnBasedBattle(
            MoveProcessor moveProcessor,
            IActionHistory actionHistory,
            IGameOutput gameOutput,
            IEnumerable<Character> characters)
        {
            _moveProcessor = moveProcessor;
            _actionHistory = actionHistory;
            _gameOutput = gameOutput;
            _characters = characters;

            CurrentPhase = BattlePhase.BattleStart;
        }

        /// <summary>
        /// Gets whether the battle is over, i.e. whether there is some team
        /// whose characters are all dead.
        /// </summary>
        public bool IsOver => Teams.Any(t => t.All(c => c.IsDead));

        /// <summary>
        /// The current battle phase.
        /// </summary>
        public BattlePhase CurrentPhase { get; private set; }

        /// <summary>
        /// Acts the current battle phase and advances to the next phase, returning the value of the
        /// next phase.
        /// </summary>
        public BattlePhase Next()
        {
            if (CurrentPhase == BattlePhase.BattleStart)
            {
                CurrentPhase = BattlePhase.TurnStart;
            }
            else if (CurrentPhase == BattlePhase.TurnStart)
            {
                _actionHistory.StartTurn();
                _gameOutput.ShowStartTurn(_actionHistory.TurnCounter);

                foreach (var team in Teams)
                {
                    _gameOutput.ShowTeamSummary(team);
                }

                var characterOrder = _characters.Where(c => !c.IsDead)
                                                .OrderByDescending(c => c.CurrentSpeed)
                                                .ToArray();

                foreach (var character in characterOrder)
                {
                    var otherCharacters = characterOrder.Where(c => c.Id != character.Id);
                    var startTurnResult = character.OnStartTurn(otherCharacters);
                    ShowBattlePhaseResult(startTurnResult);
                }

                CurrentPhase = IsOver ? BattlePhase.BattleEnd : BattlePhase.TurnChoice;
            }
            else if (CurrentPhase == BattlePhase.TurnChoice)
            {
                var characterOrder = _characters.Where(c => !c.IsDead)
                                                .OrderByDescending(c => c.CurrentSpeed)
                                                .ToArray();

                foreach (var character in characterOrder)
                {
                    var otherCharacters = characterOrder.Where(c => c.Id != character.Id);
                    var moveUse = character.ChooseMove(otherCharacters);
                    moveUse.SetTargets();
                    _moveProcessor.Push(moveUse);
                }

                CurrentPhase = BattlePhase.TurnExecute;
            }
            else if (CurrentPhase == BattlePhase.TurnExecute)
            {
                while (!_moveProcessor.MoveUseQueueIsEmpty)
                {
                    var moveUse = _moveProcessor.ApplyNext();
                    if (moveUse.HasResult)
                    {
                        _actionHistory.AddMoveUse(moveUse);
                        _gameOutput.ShowMoveUse(moveUse);
                    }
                }

                CurrentPhase = IsOver ? BattlePhase.BattleEnd : BattlePhase.TurnEnd;
            }
            else if (CurrentPhase == BattlePhase.TurnEnd)
            {
                var characterOrder = _characters.Where(c => !c.IsDead)
                                                .OrderByDescending(c => c.CurrentSpeed)
                                                .ToArray();

                foreach (var character in characterOrder)
                {
                    var otherCharacters = characterOrder.Where(c => c.Id != character.Id);
                    var endTurnResult = character.OnEndTurn(otherCharacters);
                    ShowBattlePhaseResult(endTurnResult);
                }

                CurrentPhase = BattlePhase.TurnStart;
            }
            else if (CurrentPhase == BattlePhase.BattleEnd)
            {
                ShowBattleEnd();
            }

            return CurrentPhase;
        }

        /// <summary>
        /// Outputs the given battle phase result.
        /// </summary>
        /// <param name="battlePhaseResult">The battle phase result.</param>
        private void ShowBattlePhaseResult(BattlePhaseResult battlePhaseResult)
        {
            foreach (var actionUseResult in battlePhaseResult.ItemActionsResults)
            {
                foreach (var result in actionUseResult.Results)
                {
                    _gameOutput.ShowResult(result);
                }
            }
        }

        /// <summary>
        /// Outputs the end of the battle.
        /// </summary>
        private void ShowBattleEnd()
        {
            var winningTeam = Teams.Single(t => t.Any(c => !c.IsDead));
            _gameOutput.ShowBattleEnd(winningTeam.Key);
        }
    }

    public enum BattlePhase
    {
        BattleStart,
        TurnStart,
        TurnChoice,
        TurnExecute,
        TurnEnd,
        BattleEnd,
    }
}
