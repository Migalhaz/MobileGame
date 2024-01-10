using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerManager : MigalhaSystem.Singleton<PlayerManager>
    {
        [SerializeField] PlayerMove m_playerMove;
        [SerializeField] PlayerInput m_playerInputs;
        [SerializeField] PlayerCombat m_playerCombat;

        public PlayerMove m_PlayerMove => m_playerMove;
        public PlayerInput m_PlayerInputs => m_playerInputs;
        public PlayerCombat m_PlayerCombat => m_playerCombat;

        public bool CanAct()
        {
            Turns.TurnManager turnManager = Turns.TurnManager.Instance;
            if (turnManager is null) return true;

            return turnManager.IsPlayerTurn();
        }
    }
}