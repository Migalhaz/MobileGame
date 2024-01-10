using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;
using MigalhaSystem.ScriptableEvents;

namespace Game.Player
{
    public class PlayerAnimation : AnimationController
    {
        [Header("Animations")]
        [SerializeField] AnimationSO m_moveAnimation;
        [SerializeField] AnimationSO m_idleAnimation;

        [Header("Events")]
        [SerializeField] ScriptableEvent m_onMove;
        [SerializeField] ScriptableEvent m_onStop;

        private void OnEnable()
        {
            m_onMove += PlayMoveAnimation;
            m_onStop += PlayIdleAnimation;
        }

        private void OnDisable()
        {
            m_onMove -= PlayMoveAnimation;
            m_onStop -= PlayIdleAnimation;
        }

        public void SetLookDir(Vector2 dir)
        {
            GetAnimator().SetFloat("lookX", dir.x);
            GetAnimator().SetFloat("lookY", dir.y);
        }

        void PlayMoveAnimation()
        {
            PlayAnimation(m_moveAnimation, 0);
        }

        void PlayIdleAnimation() 
        {
            PlayAnimation(m_idleAnimation, 0);
        }
    }
}