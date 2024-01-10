using UnityEngine;
using DG.Tweening;
using Trigger.System2D;
using MigalhaSystem.ScriptableEvents;
using UnityEngine.Events;

namespace Game.Player
{
    public class PlayerMove : PlayerComponent
    {
        [Header("Move Settings")]
        [SerializeField, Min(0)] float m_moveMagnitude = 1;
        [SerializeField, Min(0)] float m_moveDuration;
        [SerializeField] BoxTrigger2D m_boxTrigger;
        bool m_canMove;
        Vector3 m_lookDirection;

        [Header("Events")]
        [SerializeField] UnityEvent<Vector2> m_onLookDirectionChange;
        [SerializeField] ScriptableEvent m_onMove;
        [SerializeField] ScriptableEvent m_onStop;
        private void Awake()
        {
            m_lookDirection = Vector2.up;
            m_canMove = true;
        }

        public void SetDirectionUp()
        {
            SetLookDirection(Direction.Up);
        }

        public void SetDirectionDown()
        {
            SetLookDirection(Direction.Down);
        }

        public void SetDirectionLeft()
        {
            SetLookDirection(Direction.Left);
        }

        public void SetDirectionRight()
        {
            SetLookDirection(Direction.Right);
        }

        void SetLookDirection(Direction direction)
        {
            if (!CanAct()) return;
            
            switch (direction)
            {
                case Direction.Left:
                    m_lookDirection = Vector3.left;
                    break; 
                case Direction.Right:
                    m_lookDirection = Vector3.right;
                    break; 
                case Direction.Up:
                    m_lookDirection = Vector3.up;
                    break;
                case Direction.Down:
                    m_lookDirection = Vector3.down;
                    break;
            }
            m_onLookDirectionChange?.Invoke(m_lookDirection);
            m_boxTrigger.SetTriggerOffset(m_lookDirection);
            Move();
        }

        void Move()
        {
            PlayerAction();
            if (!CanMove()) return;
            m_onMove.Invoke();
            m_canMove = false;
            Vector3 finalPos = transform.position + (m_lookDirection * m_moveMagnitude);
            var MoveTweening = transform.DOMove(finalPos, m_moveDuration);
            MoveTweening.OnComplete(Stop);
        }

        void Stop()
        {
            m_canMove = true;
            m_onStop.Invoke();
        }

        bool CanMove()
        {
            if (m_boxTrigger.InTrigger(transform)) return false;
            return m_canMove;
        }

        private void OnDrawGizmosSelected()
        {
            m_boxTrigger.DrawTrigger(transform);
        }
    }
}