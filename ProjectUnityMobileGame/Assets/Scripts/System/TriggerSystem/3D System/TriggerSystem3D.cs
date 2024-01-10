using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Trigger.System3D
{
    [System.Serializable]
    public abstract class System3D : BasicTriggerSystem
    {
        [SerializeField] Vector3 m_triggerOffset = Vector3.zero;
        public Vector3 m_TriggerOffset => m_triggerOffset;

        #region Methods
        public System3D() : base()
        {
            SetTriggerOffset(Vector3.zero);
        }

        public void SetTriggerOffset(Vector3 _newOffset)
        {
            m_triggerOffset = _newOffset;
        }

        public void FlipOffset()
        {
            SetTriggerOffset(-m_triggerOffset);
        }

        public void FlipXOffset()
        {
            Vector3 newOffset = m_triggerOffset;
            newOffset.Set(-newOffset.x, newOffset.y, newOffset.x);
            SetTriggerOffset(newOffset);
        }

        public void FlipYOffset()
        {
            Vector3 newOffset = m_triggerOffset;
            newOffset.Set(newOffset.x, -newOffset.y, newOffset.x);
            SetTriggerOffset(newOffset);
        }

        public void FlipZOffset()
        {
            Vector3 newOffset = m_triggerOffset;
            newOffset.Set(newOffset.x, newOffset.y, -newOffset.x);
            SetTriggerOffset(newOffset);
        }

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="collider">Trigger's center by collider. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Collider collider, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="position">Trigger's center. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="colliders">List of colliders in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Vector3 position, out List<Collider> colliders, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders in It.
        /// </summary>
        /// <param name="transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="colliders">List of colliders in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Transform transform, out List<Collider> colliders, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="colliders">List of colliders in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(GameObject gameObject, out List<Collider> colliders, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="collider">Trigger's center by collider. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="colliders">List of colliders in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Collider collider, out List<Collider> colliders, bool callbacks = true);


        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="collider">Trigger's center by collider. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Collider collider, bool callbacks = true, bool debugError = true) where T : Component;
        public abstract void DrawTrigger(Collider collider);
        #endregion
    }

    [System.Serializable]
    public class BoxTrigger3D : System3D
    {
        [SerializeField, Min(0)] Vector3 m_triggerSize = Vector3.one;
        [SerializeField] Vector3 m_triggerRotation = Vector3.zero;
        public Vector3 m_TriggerSize => m_triggerSize;

        

        public override bool InTrigger(Vector3 position, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Collider[] cols = Physics.OverlapBox(position + m_TriggerOffset, m_TriggerSize, Quaternion.Euler(m_triggerRotation), m_TriggerLayerMask);
            bool isIn = cols.Length > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }

            return isIn;
        }
        public override bool InTrigger(Transform transform, bool callbacks = true)
        {
            return InTrigger(transform.position, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider collider, bool callbacks = true)
        {
            return InTrigger(collider.transform.position, callbacks);
        }

        public override bool InTrigger(Vector3 position, out List<Collider> colliders, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            colliders = Physics.OverlapBox(position + m_TriggerOffset, m_TriggerSize, Quaternion.Euler(m_triggerRotation), m_TriggerLayerMask).ToList();
            bool isIn = colliders.Count > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }

            return isIn;
        }

        public override bool InTrigger(Transform transform, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(transform.position, out colliders, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, out colliders, callbacks);
        }

        public override bool InTrigger(Collider collider, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(collider.transform.position, out colliders, callbacks);
        }

        public override T InTrigger<T>(Vector3 position, bool callbacks = true, bool debugError = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            List<Collider> colliders = Physics.OverlapBox(position + m_TriggerOffset, m_TriggerSize, Quaternion.Euler(m_triggerRotation), m_TriggerLayerMask).ToList();
            bool isIn = colliders.Count > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            if (isIn) return null;
            foreach (Collider c in colliders)
            {
                if (c.TryGetComponent(out T component))
                {
                    return component;
                }
            }
            return null;
        }

        public override T InTrigger<T>(Transform transform, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(transform.position, callbacks, debugError);
        }

        public override T InTrigger<T>(GameObject gameObject, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(gameObject.transform.position, callbacks, debugError);

        }

        public override T InTrigger<T>(Collider collider, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(collider.transform.position, callbacks, debugError);
        }

        public override void DrawTrigger(Vector3 position)
        {
            if (!m_DrawSettings.m_Draw) return;
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(position, Quaternion.Euler(m_triggerRotation), Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = InTrigger(position, false) ? m_DrawSettings.m_InColor : m_DrawSettings.m_OutColor;
            if (m_DrawSettings.m_DrawSolid)
            {
                Gizmos.DrawCube(m_TriggerOffset, m_triggerSize);
            }
            else
            {
                Gizmos.DrawWireCube(m_TriggerOffset, m_triggerSize);
            }
        }

        public override void DrawTrigger(Transform transform)
        {
            DrawTrigger(transform.position);
        }

        public override void DrawTrigger(GameObject gameObject)
        {
            DrawTrigger(gameObject.transform.position);
        }
        public override void DrawTrigger(Collider collider)
        {
            DrawTrigger(collider.transform.position);
        }
    }
    [System.Serializable]
    public class SphereTrigger3D : System3D
    {
        [SerializeField, Min(0)] float m_triggerRadius = 1;
        public float m_TriggerRadius => m_triggerRadius;

        public override bool InTrigger(Vector3 position, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Collider[] cols = Physics.OverlapSphere(position + m_TriggerOffset, m_triggerRadius, m_TriggerLayerMask);
            bool isIn = cols.Length > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }

            return isIn;
        }
        public override bool InTrigger(Transform transform, bool callbacks = true)
        {
            return InTrigger(transform.position, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider collider, bool callbacks = true)
        {
            return InTrigger(collider.transform.position, callbacks);
        }

        public override bool InTrigger(Vector3 position, out List<Collider> colliders, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            colliders = Physics.OverlapSphere(position + m_TriggerOffset, m_triggerRadius, m_TriggerLayerMask).ToList();
            bool isIn = colliders.Count > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }

            return isIn;
        }

        public override bool InTrigger(Transform transform, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(transform.position, out colliders, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, out colliders, callbacks);
        }

        public override bool InTrigger(Collider collider, out List<Collider> colliders, bool callbacks = true)
        {
            return InTrigger(collider.transform.position, out colliders, callbacks);
        }

        public override T InTrigger<T>(Vector3 position, bool callbacks = true, bool debugError = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            List<Collider> colliders = Physics.OverlapSphere(position + m_TriggerOffset, m_triggerRadius, m_TriggerLayerMask).ToList();
            bool isIn = colliders.Count > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            if (isIn) return null;
            foreach (Collider c in colliders)
            {
                if (c.TryGetComponent(out T component))
                {
                    return component;
                }
            }
            return null;
        }

        public override T InTrigger<T>(Transform transform, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(transform.position, callbacks, debugError);
        }

        public override T InTrigger<T>(GameObject gameObject, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(gameObject.transform.position, callbacks, debugError);

        }

        public override T InTrigger<T>(Collider collider, bool callbacks = true, bool debugError = true)
        {
            return InTrigger<T>(collider.transform.position, callbacks, debugError);
        }

        public override void DrawTrigger(Vector3 position)
        {
            if (!m_DrawSettings.m_Draw) return;
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Gizmos.color = InTrigger(position, false) ? m_DrawSettings.m_InColor : m_DrawSettings.m_OutColor;
            if (m_DrawSettings.m_DrawSolid)
            {
                Gizmos.DrawSphere(m_TriggerOffset, m_triggerRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(m_TriggerOffset, m_triggerRadius);
            }
        }

        public override void DrawTrigger(Transform transform)
        {
            DrawTrigger(transform.position);
        }

        public override void DrawTrigger(GameObject gameObject)
        {
            DrawTrigger(gameObject.transform.position);
        }
        public override void DrawTrigger(Collider collider)
        {
            DrawTrigger(collider.transform.position);
        }
    }
}