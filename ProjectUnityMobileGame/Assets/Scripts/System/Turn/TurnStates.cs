using MigalhaSystem.ScriptableEvents;
using MigalhaSystem.StateMachine;
using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Turns
{
    public abstract class Turn : AbstractState
    {
        public int m_turnsCount { get; private set; }
        protected TurnController m_turnController;
        public override void EnterState(StateMachineController stateMachineController)
        {
            m_turnController = (TurnController) stateMachineController;
            base.EnterState(stateMachineController);
        }
        public override void ExitState(StateMachineController stateMachineController)
        {
            base.ExitState(stateMachineController);
            m_turnsCount++;
        }
    }
    [System.Serializable]
    public class PlayerTurn : Turn
    {

    }

    [System.Serializable]
    public class EnemyTurn : Turn
    {
        [SerializeField] Timer m_enemyTurnTimer;
        public override void EnterState(StateMachineController stateMachineController)
        {
            base.EnterState(stateMachineController);
            m_enemyTurnTimer.ActiveTimer(true);
            m_enemyTurnTimer.OnTimerElapsed.AddListener(ExitEnemyTurn);
        }

        public override void UpdateState(StateMachineController stateMachineController)
        {
            base.UpdateState(stateMachineController);
            m_enemyTurnTimer.TimerElapse(Time.deltaTime);
        }

        public override void ExitState(StateMachineController stateMachineController)
        {
            base.ExitState(stateMachineController);
            m_enemyTurnTimer.OnTimerElapsed.RemoveListener(ExitEnemyTurn);

        }

        void ExitEnemyTurn()
        {
            m_turnController.SwitchState(m_turnController.m_PlayerTurn);
        }
    }
}