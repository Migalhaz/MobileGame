using System.Collections.Generic;
using UnityEngine;

namespace Trigger.System2D.Manager
{
    public class CircleTriggerManager2D : MonoBehaviour
    {
        [SerializeField] Core.UpdateMethod m_updateMethod = Core.UpdateMethod.FixedUpdate;
        [SerializeField] List<CircleTrigger2D> m_triggers = new List<CircleTrigger2D>() { new CircleTrigger2D() };

        void Update()
        {
            CallTrigger(Core.UpdateMethod.Update);
        }
        void FixedUpdate()
        {
            CallTrigger(Core.UpdateMethod.FixedUpdate);
        }
        void LateUpdate()
        {
            CallTrigger(Core.UpdateMethod.LateUpdate);
        }

        void CallTrigger(Core.UpdateMethod callMethod)
        {
            if (callMethod != m_updateMethod) return;
            foreach (CircleTrigger2D trigger in m_triggers)
            {
                trigger.InTrigger(transform.localPosition);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawTrigger(Core.DrawTrigger.DrawMode.OnDrawGizmos);
        }

        private void OnDrawGizmosSelected()
        {
            DrawTrigger(Core.DrawTrigger.DrawMode.OnDrawGizmosSelect);
        }

        void DrawTrigger(Core.DrawTrigger.DrawMode drawMode)
        {
            foreach (CircleTrigger2D trigger in m_triggers)
            {
                if (trigger.m_DrawSettings.m_DrawMethod != drawMode) continue;
                trigger.DrawTrigger(transform.position);
            }

        }
#endif
        #endregion
    }
}
