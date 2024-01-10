using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Trigger.Core;

namespace Trigger
{
    [Serializable]
    public abstract class BasicTriggerSystem
    {
        #region Variables
        [SerializeField] string m_triggerTag = "New Trigger";
        [SerializeField] LayerMask m_triggerLayerMask;
        [SerializeField] Transform m_centerObject;
        [SerializeField] TriggerEvents m_triggerEvents;
        [SerializeField] DrawTrigger m_drawSettings = new DrawTrigger();


        #region Getters
        public string m_TriggerTag => m_triggerTag;
        public LayerMask m_TriggerLayerMask => m_triggerLayerMask;
        public Transform m_CenterObject => m_centerObject;
        public bool m_entered { get; private set; }
        public TriggerEvents m_TriggerEvents => m_triggerEvents;
        public DrawTrigger m_DrawSettings => m_drawSettings;
        #endregion

        #endregion

        #region Methods

        public BasicTriggerSystem()
        {
            m_triggerTag = "New Trigger";
            m_drawSettings = new DrawTrigger();
        }

        /// <summary>
        /// Checks the trigger tag against the defined tag.
        /// </summary>
        /// <param name="tag">Tag to compare.</param>
        /// <returns>Returns true if trigger has the same tag. Returns false otherwise.</returns>
        public virtual bool CompareTag(string tag)
        {
            return Equals(tag, m_triggerTag);
        }

        #region In Trigger Methods
        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="position">Trigger's center. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Vector3 position, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Transform transform, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(GameObject gameObject, bool callbacks = true);

        /// <summary>
        /// Checks if there's something in trigger and gets a component from It.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="position">Trigger's center by position. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Vector3 position, bool callbacks = true, bool debugError = true) where T : Component;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from It.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Transform transform, bool callbacks = true, bool debugError = true) where T : Component;

        /// <summary>
        /// Checks if there's something in trigger and gets a component from It.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(GameObject gameObject, bool callbacks = true, bool debugError = true) where T : Component;
        #endregion

        protected void InvokeCallbacks(bool isIn)
        {
            if (isIn)
            {
                if (m_entered)
                {
                    m_triggerEvents.OnTriggerStayInsideInvoke();
                }
                else
                {
                    m_entered = true;
                    m_triggerEvents.OnTriggerEnterInvoke();
                }
            }
            else
            {
                if (m_entered)
                {
                    m_entered = false;
                    m_triggerEvents.OnTriggerExitInvoke();
                }
                else
                {
                    m_triggerEvents.OnTriggerStayOutsideInvoke();
                }
            }
        }

        #region DrawMethods
        public abstract void DrawTrigger(Vector3 position);
        public abstract void DrawTrigger(Transform transform);
        public abstract void DrawTrigger(GameObject gameObject);
        #endregion

        #endregion
    }
}

namespace Trigger.Core
{
    [Serializable]
    public class DrawTrigger
    {
        #region Variables
        [SerializeField, Tooltip("Draw the trigger gizmos")] bool m_draw = true;
        [SerializeField, Tooltip("Draw the trigger gizmos only when the game object is selected")] DrawMode m_drawMethod = DrawMode.OnDrawGizmos;
        [SerializeField, Tooltip("Draw a solid trigger")] bool m_drawSolid = false;
        [SerializeField, Tooltip("Color of the trigger when there's something inside")] Color m_inColor = Color.green;
        [SerializeField, Tooltip("Color of the trigger when there's anything inside")] Color m_outColor = Color.red;

        #region Getters
        /// <summary> Draw the trigger on gizmos. </summary>
        public bool m_Draw => m_draw;

        /// <summary> Draw the trigger on gizmos. </summary>
        public DrawMode m_DrawMethod => m_drawMethod;

        /// <summary> Draw a solid trigger. </summary>
        public bool m_DrawSolid => m_drawSolid;

        /// <summary> Color of the trigger when there's something inside. </summary>
        public Color m_InColor => m_inColor;

        /// <summary> Color of the trigger when there's nothing inside. </summary>
        public Color m_OutColor => m_outColor;
        #endregion

        #endregion

        #region Constructors
        public DrawTrigger()
        {
            m_draw = true;
            m_inColor = Color.green;
            m_outColor = Color.red;
        }
        public DrawTrigger(Color inColor)
        {
            m_draw = true;
            m_inColor = inColor;
            m_outColor = Color.red;
        }
        public DrawTrigger(Color inColor, Color outColor)
        {
            m_draw = true;
            m_inColor = inColor;
            m_outColor = outColor;
        }
        #endregion

        #region Enuns
        [Serializable]
        public enum DrawMode
        {
            OnDrawGizmos, OnDrawGizmosSelect
        }
        #endregion
    }


    [Serializable]
    public class TriggerEvents
    {
        #region Variables
        [SerializeField, Tooltip("Methods that are called when something enter the trigger")] UnityEvent OnTriggerEnter;
        [SerializeField, Tooltip("Methods that are called when something stay inside the trigger")] UnityEvent OnTriggerStayInside;
        [SerializeField, Tooltip("Methods that are called when something stay outside the trigger")] UnityEvent OnTriggerStayOutside;
        [SerializeField, Tooltip("Methods that are called when something exit the trigger")] UnityEvent OnTriggerExit;
        #endregion

        #region Methods
        #region InvokeMethods
        /// <summary> Invoke all methods from OnTriggerEnter unity event. </summary>
        public void OnTriggerEnterInvoke() => OnTriggerEnter?.Invoke();

        /// <summary> Invoke all methods from OnTriggerStayInside unity event. </summary>
        public void OnTriggerStayInsideInvoke() => OnTriggerStayInside?.Invoke();

        /// <summary> Invoke all methods from OnTriggerStayInside unity event. </summary>
        public void OnTriggerStayOutsideInvoke() => OnTriggerStayOutside?.Invoke();

        /// <summary> Invoke all methods from OnTriggerExit unity event. </summary>
        public void OnTriggerExitInvoke() => OnTriggerExit?.Invoke();
        #endregion

        #region AddMethods
        /// <summary> Add a listner to OnTriggerEnter unity event. </summary>
        /// <param name="newAction"></param>
        public void OnTriggerEnterAddListner(UnityAction newAction) => OnTriggerEnter.AddListener(newAction);

        /// <summary> Add a listner to OnTriggerStayInside unity event. </summary>
        /// <param name="newAction"></param>
        public void OnTriggerStayInsideAddListner(UnityAction newAction) => OnTriggerStayInside.AddListener(newAction);

        /// <summary> Add a listner to OnTriggerStayOutside unity event. </summary>
        /// <param name="newAction"></param>
        public void OnTriggerStayOutsideAddListner(UnityAction newAction) => OnTriggerStayOutside.AddListener(newAction);

        /// <summary> Add a listner to OnTriggerExit unity event. </summary>
        /// <param name="newAction"></param>
        public void OnTriggerExitAddListner(UnityAction newAction) => OnTriggerExit.AddListener(newAction);
        #endregion

        #region RemoveMethods
        /// <summary> Remove a listner from OnTriggerEnter unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerEnterRemoveListner(UnityAction _actionToRemove) => OnTriggerEnter.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerStayInside unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerStayInsideRemoveListner(UnityAction _actionToRemove) => OnTriggerStayInside.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerStayOutside unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerStayOutsideRemoveListner(UnityAction _actionToRemove) => OnTriggerStayOutside.RemoveListener(_actionToRemove);

        /// <summary> Remove a listner from OnTriggerExit unity event. </summary>
        /// <param name="_actionToRemove"></param>
        public void OnTriggerExitRemoveListner(UnityAction _actionToRemove) => OnTriggerExit.RemoveListener(_actionToRemove);
        #endregion

        #region ClearMethods
        /// <summary> Remove all listeners from OnTriggerEnter </summary>
        public void OnTriggerEnterClear() => OnTriggerEnter.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerStayInside </summary>
        public void OnTriggerStayInsideClear() => OnTriggerStayInside.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerStayInside </summary>
        public void OnTriggerStayOutsideClear() => OnTriggerStayOutside.RemoveAllListeners();

        /// <summary> Remove all listeners from OnTriggerExit </summary>
        public void OnTriggerExitClear() => OnTriggerExit.RemoveAllListeners();
        #endregion
        #endregion
    }

    public static class Vector2Extension
    {
        public static Vector3 ToVector3(this Vector2 _vector2)
        {
            return _vector2;
        }
    }

    [Serializable]
    public enum UpdateMethod
    {
        Update = 0, FixedUpdate = 1, LateUpdate = 2
    }
}