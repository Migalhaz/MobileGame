using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(fileName = "New Animation Data", menuName = "Scriptable Object/Animation/New Animation Data")]
    public class AnimationSO : ScriptableObject
    {
        [Header("Animation Settings")]
        [SerializeField] string m_animationClipName;
        [SerializeField, Min(0)] float m_transitionDuration;
        [SerializeField, Min(0)] int m_animationLayer;
        int m_animationClipHash = 0;
        public string m_AnimationClipName => m_animationClipName;
        public float m_TransitionDuration => m_transitionDuration;
        public int m_AnimationLayer => m_animationLayer;
        public int m_AnimationClipHash
        {
            get
            {
                if (m_animationClipHash == 0)
                {
                    m_animationClipHash = Animator.StringToHash(m_animationClipName);
                }
                return m_animationClipHash;
            }
        }

        bool AnimatorAvailable(Animator enemyAnimator)
        {
            if (!enemyAnimator.gameObject.activeSelf) return false;
            if (enemyAnimator is null) return false;
            if (!enemyAnimator.isActiveAndEnabled) return false;
            if (!enemyAnimator.enabled) return false;
            return true;
        }

        public void Play(Animator enemyAnimator)
        {
            if (!AnimatorAvailable(enemyAnimator)) return;
            enemyAnimator.Play(m_AnimationClipHash, m_animationLayer);
        }

        public void Play(Animator enemyAnimator, float normalizedTime)
        {
            if (!AnimatorAvailable(enemyAnimator)) return;
            enemyAnimator.Play(m_AnimationClipHash, m_animationLayer, normalizedTime);
        }

        public void CrossFade(Animator enemyAnimator)
        {
            if (!AnimatorAvailable(enemyAnimator)) return;
            enemyAnimator.CrossFade(m_AnimationClipHash, m_transitionDuration, m_animationLayer);
        }

        public void CrossFade(Animator enemyAnimator, float transtionDuration)
        {
            if (!AnimatorAvailable(enemyAnimator)) return;
            enemyAnimator.CrossFade(m_AnimationClipHash, transtionDuration, m_animationLayer);
        }

        public bool IsAnimationPlaying(Animator enemyAnimator)
        {
            if (!AnimatorAvailable(enemyAnimator))
            {
                Debug.LogWarning("Animator is not available!");
                return false;
            }
            bool playing = enemyAnimator.GetCurrentAnimatorStateInfo(m_animationLayer).IsName(m_animationClipName);
            return playing;
        }
    }
}