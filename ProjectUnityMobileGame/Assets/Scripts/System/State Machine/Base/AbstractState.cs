using UnityEngine;
using UnityEngine.Events;
namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class AbstractState
    {
        [Header("State Settings")]
        [SerializeField] protected bool m_canBeInterrupt = true;
        public bool m_CanBeInterrupt => m_canBeInterrupt;
        [SerializeField] UnityEvent m_onEnterState;
        [SerializeField] UnityEvent m_onUpdateState;
        [SerializeField] UnityEvent m_onExitState;

        public UnityEvent m_OnEnterState => m_onEnterState;
        public UnityEvent m_OnUpdateState => m_onUpdateState;
        public UnityEvent m_OnExitState => m_onExitState;

        public virtual void EnterState(StateMachineController stateMachineController) { m_onEnterState?.Invoke(); }
        public virtual void UpdateState(StateMachineController stateMachineController) { m_onUpdateState?.Invoke(); }
        public virtual void FixedUpdateState(StateMachineController stateMachineController) { }
        public virtual void LateUpdateState(StateMachineController stateMachineController) { }
        public virtual void ExitState(StateMachineController stateMachineController) { m_onExitState?.Invoke(); }
    }
}