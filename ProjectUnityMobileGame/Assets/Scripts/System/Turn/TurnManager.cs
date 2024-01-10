using MigalhaSystem.Extensions;
using MigalhaSystem.ScriptableEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turns
{
    public class TurnManager : MigalhaSystem.Singleton<TurnManager>
    {
        [SerializeField] bool t_deubgMode;
        [SerializeField] TurnController m_turnController;
        public TurnController m_TurnController => m_turnController;

        public int m_PlayerTurnCount { get { return m_turnController.m_PlayerTurn.m_turnsCount; } }
        public int m_EnemyTurnCount { get { return m_turnController.m_EnemyTurn.m_turnsCount; } }
        public int m_TurnCount { get { return Mathf.FloorToInt((m_PlayerTurnCount + m_EnemyTurnCount)/2); } }

        [Header("Events")]
        [SerializeField] ScriptableEvent m_playerAction;

        private void OnEnable()
        {
            m_playerAction += PlayerAction;
        }

        private void OnDisable()
        {
            m_playerAction -= PlayerAction;
        }

        private void Update()
        {
            if (t_deubgMode) { DebugMode(); }
        }

        public Turn GetCurrentTurn()
        {
            return (Turn)m_turnController.m_CurrentState;
        }

        public bool CheckTurn<T>() where T : Turn
        {
            return m_turnController.CheckCurrentState<T>();
        }

        public bool IsPlayerTurn()
        {
            return m_turnController.CheckCurrentState<PlayerTurn>();
        }

        public bool IsEnemyTurn()
        {
            return m_turnController.CheckCurrentState<EnemyTurn>();
        }

        void PlayerAction()
        {
            m_turnController.SwitchState(m_turnController.m_EnemyTurn);
        }

        void DebugMode()
        {
            string turnCountDebug =
                "Player Turns: ".Color(Color.green) + $"{m_PlayerTurnCount} " +
                "Enemy Turns: ".Color(Color.red) + $"{m_EnemyTurnCount}\n" +
                "Turns: ".Color(Color.blue) + $"{m_TurnCount}";
            Debug.Log(turnCountDebug);
            string turnDebug = "Current turn: " + $"{GetCurrentTurn().GetType()}".Color(Color.red);
            Debug.Log(turnDebug);

        }
    }
}