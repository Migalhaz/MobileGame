using MigalhaSystem;
using MigalhaSystem.Extensions;
using MigalhaSystem.ScriptableEvents;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using MigalhaSystem.StateMachine;

namespace Game.Turns
{
    public class TurnController : StateMachineController
    {
        [SerializeField] PlayerTurn m_playerTurn;
        [SerializeField] EnemyTurn m_enemyTurn;
        public PlayerTurn m_PlayerTurn => m_playerTurn;
        public EnemyTurn m_EnemyTurn => m_enemyTurn;
        protected override AbstractState FirstState()
        {
            return m_PlayerTurn;
        }

        protected override void Update()
        {
            base.Update();
            //Debug.Log("O turno atual é: " + $"{m_currentState.GetType()}".Color(Color.red));
        }
    }
}