using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [DisallowMultipleComponent]
    public abstract class StateMachineController : MonoBehaviour
    {
        protected AbstractState m_currentState;
        protected AbstractState m_lastState;

        #region Getters
        public AbstractState m_CurrentState => m_currentState;
        public AbstractState m_LastState => m_lastState;
        #endregion

        protected virtual void Start()
        {
            StartState();
        }

        public virtual void StartState()
        {
            m_currentState = null;
            m_lastState = null;
            ForceSwitchState(FirstState());
        }
        protected abstract AbstractState FirstState();


        protected virtual void Update()
        {
            m_currentState?.UpdateState(this);
        }

        protected virtual void FixedUpdate()
        {
            m_currentState?.FixedUpdateState(this);
        }

        protected virtual void LateUpdate()
        {
            m_currentState?.LateUpdateState(this);
        }

        public virtual void SwitchState(AbstractState newState)
        {
            if (m_currentState is not null)
            {
                if (!m_currentState.m_CanBeInterrupt)
                {
                    return;
                }
                m_currentState.ExitState(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterState(this);
        }

        public virtual void ForceSwitchState(AbstractState newState)
        {
            if (m_currentState is not null)
            {
                m_currentState.ExitState(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterState(this);
        }

        public virtual void SwitchStateByBool(bool verification, AbstractState trueState, AbstractState falseState, bool forceSwitch = false)
        {
            AbstractState newState = verification ? trueState : falseState;

            if (!forceSwitch)
            {
                SwitchState(newState);
            }
            else
            {
                ForceSwitchState(newState);
            }
        }

        public virtual void SwitchStateByBool<T1, T2>(bool verification, T1 trueState, T2 falseState, bool forceSwitch = false) where T1 : AbstractState where T2 : AbstractState
        {
            AbstractState newState;

            if (verification)
            {
                if (CheckCurrentState<T1>()) return;
                newState = trueState;
            }
            else
            {
                if (CheckCurrentState<T2>()) return;
                newState = falseState;
            }
            if (!forceSwitch)
            {
                SwitchState(newState);
            }
            else
            {
                ForceSwitchState(newState);
            }
        }

        public bool CheckCurrentState<T>() where T : AbstractState
        {
            return m_currentState is T;
        }

        public bool CheckLastState<T>() where T : AbstractState
        {
            return m_lastState is T;
        }

        public static bool CheckState<T>(AbstractState state) where T : AbstractState
        {
            return state is T;
        }
    }
}
