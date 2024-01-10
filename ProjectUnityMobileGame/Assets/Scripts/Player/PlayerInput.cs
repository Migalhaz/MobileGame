using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MigalhaSystem.Extensions;
namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        Vector3 m_startTouchPos;
        Vector3 m_endTouchPos;
        [Header("Swipe Settings")]
        [SerializeField] DrawSettings m_deadZoneDrawSettings;
        [SerializeField] float m_swipeDeadZone;
        bool m_beganOverUi;

        [Header("Tap Settings")]
        [SerializeField] Timer m_doubleTapThreshold;
        bool m_singleTap;
        
        [Header("Events")]
        [SerializeField] InputEvents m_inputEvents;

        private void Awake()
        {
            m_doubleTapThreshold.OnTimerElapsed.AddListener(() => { SetupTap(false); });
            m_beganOverUi = false;
        }

        private void Update()
        {
            m_doubleTapThreshold.TimerElapse(Time.deltaTime);
            TouchCheck();
        }

        void TouchCheck()
        {
            if (Input.touchCount <= 0) return;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                TouchEnded();
            }
            if (touch.phase == TouchPhase.Began)
            {
                TouchBegan();
            }

            void TouchBegan()
            {
                m_startTouchPos = touch.position;
                m_inputEvents.m_OnTouchBegan?.Invoke();
                m_beganOverUi = MigalhazHelper.IsOverUI(touch.position);
            }

            void TouchEnded()
            {
                m_endTouchPos = touch.position;
                m_inputEvents.m_OnTouchEnded?.Invoke();
                OnEndTouch();
            }
        }

        void OnEndTouch()
        {
            if (m_beganOverUi)
            {
                m_beganOverUi = false;
                return;
            }
            Vector2 inputVector = m_endTouchPos - m_startTouchPos;

            m_startTouchPos = Vector3.zero;
            m_endTouchPos = Vector3.zero;

            if (inputVector.magnitude <= m_swipeDeadZone)
            {
                TapCheck();
                return;
            }
            if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
            {
                HorizontalInput();
            }
            else
            {
                VerticalInput();
            }

            void HorizontalInput()
            {
                if (inputVector.x > 0)
                {
                    m_inputEvents.m_OnRightSwipe?.Invoke();
                }
                else
                {
                    m_inputEvents.m_OnLeftSwipe?.Invoke();
                }
            }
            void VerticalInput()
            {
                if (inputVector.y > 0)
                {
                    m_inputEvents.m_OnUpSwipe?.Invoke();
                }
                else
                {
                    m_inputEvents.m_OnDownSwipe?.Invoke();
                }
            }
        }

        void TapCheck()
        {
            if (m_singleTap)
            {
                m_inputEvents.m_OnDoubleTap?.Invoke();
                SetupTap(false);
            }
            else
            {
                m_inputEvents.m_OnSingleTap?.Invoke();
                SetupTap(true);
            }

            
        }

        void SetupTap(bool value)
        {
            m_singleTap = value;
            m_doubleTapThreshold.ActiveTimer(value);
        }

        private void OnDrawGizmos()
        {
            if (!m_deadZoneDrawSettings.CanDraw(DrawMethod.OnDrawGizmos)) return;
            DrawSwipeDeadZone();
        }

        private void OnDrawGizmosSelected()
        {
            if (!m_deadZoneDrawSettings.CanDraw(DrawMethod.OnDrawGizmosSelected)) return;
            DrawSwipeDeadZone();
        }

        void DrawSwipeDeadZone()
        {
            Gizmos.color = m_deadZoneDrawSettings.GetColor();
            Gizmos.DrawWireSphere(m_startTouchPos, m_swipeDeadZone);
        }
    }

    [System.Serializable]
    public class InputEvents
    {
        [Header("Touch")]
        [SerializeField] UnityEvent m_onTouchBegan;
        [SerializeField] UnityEvent m_onTouchEnded;
        [SerializeField] UnityEvent m_onSingleTap;
        [SerializeField] UnityEvent m_onDoubleTap;

        [Header("Directional Swipe")]
        [SerializeField] UnityEvent m_onSwipe;
        [SerializeField] UnityEvent m_onUpSwipe;
        [SerializeField] UnityEvent m_onLeftSwipe;
        [SerializeField] UnityEvent m_onDownSwipe;
        [SerializeField] UnityEvent m_onRightSwipe;

        public UnityEvent m_OnTouchBegan => m_onTouchBegan;
        public UnityEvent m_OnTouchEnded => m_onTouchEnded;
        public UnityEvent m_OnDoubleTap => m_onDoubleTap;
        public UnityEvent m_OnSingleTap => m_onSingleTap;
        public UnityEvent m_OnSwipe => m_onSwipe;
        public UnityEvent m_OnUpSwipe => m_onUpSwipe;
        public UnityEvent m_OnLeftSwipe => m_onLeftSwipe;
        public UnityEvent m_OnDownSwipe => m_onDownSwipe;
        public UnityEvent m_OnRightSwipe => m_onRightSwipe;
    }
}