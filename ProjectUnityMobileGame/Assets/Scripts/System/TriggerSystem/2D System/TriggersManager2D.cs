using System.Collections.Generic;
using UnityEngine;

namespace Trigger.System2D.Manager
{
    public class TriggersManager2D : MonoBehaviour
    {
        [SerializeField] Core.UpdateMethod updateMethod = Core.UpdateMethod.FixedUpdate;
        [SerializeField] List<BoxTrigger2D> boxes = new List<BoxTrigger2D>() { new BoxTrigger2D() };
        [SerializeField] List<CircleTrigger2D> circles = new List<CircleTrigger2D>() { new CircleTrigger2D() };
        
        void Update()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.Update)) return;
            boxes.ForEach(x => x.InTrigger(transform.position));
            circles.ForEach(x => x.InTrigger(transform.position));
        }

        void FixedUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.FixedUpdate)) return;
            boxes.ForEach(x => x.InTrigger(transform.position));
            circles.ForEach(x => x.InTrigger(transform.position));
        }

        void LateUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.LateUpdate)) return;
            boxes.ForEach(x => x.InTrigger(transform.position));
            circles.ForEach(x => x.InTrigger(transform.position));
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (BoxTrigger2D trigger in boxes)
            {
                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
                trigger.DrawTrigger(transform.position);
            }

            foreach (CircleTrigger2D trigger in circles)
            {
                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
                trigger.DrawTrigger(transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (BoxTrigger2D trigger in boxes)
            {
                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
                trigger.DrawTrigger(transform.position);
            }

            foreach (CircleTrigger2D trigger in circles)
            {
                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
                trigger.DrawTrigger(transform.position);
            }
        }
#endif
        #endregion
    }
}