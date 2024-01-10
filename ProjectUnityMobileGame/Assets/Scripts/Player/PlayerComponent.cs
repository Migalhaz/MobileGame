using MigalhaSystem.ScriptableEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        [Header("Player Action Event")]
        [SerializeField] ScriptableEvent m_playerAction;
        protected void PlayerAction()
        {
            m_playerAction.Invoke();
        }

        protected bool CanAct()
        {
            return GetPlayerManager().CanAct();
        }

        protected PlayerManager GetPlayerManager()
        {
            return PlayerManager.Instance;
        }
    }
}