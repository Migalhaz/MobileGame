using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MigalhaSystem.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 ToVector3(this Vector2 _vector2)
        {
            return _vector2;
        }

        public static void ClearVector(this ref Vector2 _vector2)
        {
            _vector2 = Vector2.zero;
        }

        public static void ClearVector(this ref Vector3 _vector3)
        {
            _vector3 = Vector3.zero;
        }

        public static Vector2 ToVector2(this Vector3 _vector3)
        {
            return _vector3;
        }
    }

    public static class EnumExtension
    {
        public static int GetEnumCount<TEnum>(this TEnum _enum) where TEnum : struct, System.Enum
        {
            return System.Enum.GetNames(typeof(TEnum)).Length;
        }

        public static int GetEnumCount<TEnum>() where TEnum : struct, System.Enum
        {
            return System.Enum.GetNames(typeof(TEnum)).Length;
        }

        public static int GetEnumCount(this System.Type _type)
        {
            return System.Enum.GetNames(_type.GetType()).Length;
        }
    }

    public enum UpdateMethod
    {
        N, Update, FixedUpdate, LateUpdate
    }

    public enum DrawMethod
    {
        N, OnDrawGizmos, OnDrawGizmosSelected
    }

    public static class StringExtend
    {
        public static string Color(this string _string, Color _textColor)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(_textColor)}>{_string}</color>";
        }

        public static string Color(this string _string, string _textColorHex)
        {

            string _colorString = _textColorHex;
            if (_textColorHex.StartsWith('#'))
            {
                _colorString = _textColorHex.Split('#')[1];

            }
            return $"<color=#{_colorString}>{_string}</color>";
        }

        public static string Bold(this string _string)
        {
            return $"<b>{_string}</b>";
        }

        public static string Italic(this string _string)
        {
            return $"<i>{_string}</i>";
        }

        public static string Warning(this string _string)
        {
            return $"{"WARNING:".Bold().Color(UnityEngine.Color.yellow)} {_string.Italic()}";
        }

        public static string Error(this string _string)
        {
            return $"{"ERROR:".Bold().Color(UnityEngine.Color.red)} {_string.Italic()}";
        }
    }

    public static class ListExtend
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }

    [System.Serializable]
    public struct FloatRange
    {
        [SerializeField] float m_minValue;
        [SerializeField] float m_maxValue;
        public float m_MinValue => m_minValue;
        public float m_MaxValue => m_maxValue;
        public FloatRange(float _minValue, float _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }
        public float GetRandomValue()
        {
            return Random.Range(m_minValue, m_maxValue);
        }
        public bool InRange(float value)
        {
            return value >= m_minValue && value <= m_maxValue;
        }
        public void ChangeMinValue(float newValue)
        {
            m_minValue = newValue;
        }
        public void ChangeMaxValue(float newValue)
        {
            m_maxValue = newValue;
        }
    }

    [System.Serializable]
    public struct IntRange
    {
        [SerializeField] int m_minValue;
        [SerializeField] int m_maxValue;
        public int m_MinValue => m_minValue;
        public int m_MaxValue => m_maxValue;

        public IntRange(int _minValue, int _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }

        public int GetRandomValue(bool _maxInclusive = false)
        {
            return Random.Range(m_minValue, m_maxValue + (_maxInclusive ? 1 : 0));
        }
        public bool InRange(float value)
        {
            return value >= m_minValue && value <= m_maxValue;
        }
        public void ChangeMinValue(int newValue)
        {
            m_minValue = newValue;
        }
        public void ChangeMaxValue(int newValue)
        {
            m_maxValue = newValue;
        }
    }

    [System.Serializable]
    public class Timer
    {
        [Header("Timer Settings")]
        [SerializeField] bool m_Countdown = true;
        [SerializeField] bool m_repeater = true;
        [SerializeField] FloatRange m_startTimer = new(1, 1);
        float m_currentTimerValue;
        [SerializeField] protected UnityEvent m_OnTimerElapsed;
        public UnityEvent OnTimerElapsed => m_OnTimerElapsed;
        public FloatRange m_StartTimer => m_startTimer;

        public virtual void ActiveTimer(bool active)
        {
            m_Countdown = active;
            if (active)
            {
                SetupTimer();
            }
        }


        public void SetStartTimer(FloatRange newStartTimer)
        {
            m_startTimer = newStartTimer;
        }
        public void ChangeMinStartTimerValue(float newMinValue)
        {
            m_startTimer.ChangeMinValue(newMinValue);
        }

        public void ChangeMaxStartTimerValue(float newMaxValue)
        {
            m_startTimer.ChangeMaxValue(newMaxValue);
        }

        public void ChangeStartTimerValue(float newMinValue, float newMaxValue)
        {
            m_startTimer.ChangeMinValue(newMinValue);
            m_startTimer.ChangeMaxValue(newMaxValue);
        }

        public void DecreaseStartTimerValue(float decreaseValue)
        {
            m_startTimer.ChangeMinValue(m_startTimer.m_MinValue - decreaseValue);
            m_startTimer.ChangeMaxValue(m_startTimer.m_MaxValue - decreaseValue);
        }

        public void DecreaseStartTimerValue(float decreaseMinValue, float decreaseMaxValue)
        {
            m_startTimer.ChangeMinValue(m_startTimer.m_MinValue - decreaseMinValue);
            m_startTimer.ChangeMaxValue(m_startTimer.m_MaxValue - decreaseMaxValue);
        }

        public virtual void SetupTimer()
        {
            m_currentTimerValue = m_startTimer.GetRandomValue();
        }

        public virtual bool TimerElapse(float deltaTime)
        {
            if (!m_Countdown) return false;
            m_currentTimerValue -= deltaTime;
            if (m_currentTimerValue <= 0)
            {
                TimerElapsedAction();
                return true;
            }

            return false;
        }

        protected virtual void TimerElapsedAction()
        {
            ActiveTimer(m_repeater);
            m_OnTimerElapsed?.Invoke();
        }
    }

    [System.Serializable]
    public class ChancePicker
    {
        [SerializeField, Range(0, 100)] float m_chance;
        public ChancePicker() 
        {
            SetChance(50);
        }

        public ChancePicker(float value)
        {
            SetChance(value);
        }

        public void SetChance(float value)
        {
            m_chance = Mathf.Clamp(value, 0, 100);
        }
        public bool PickChance()
        {
            return Random.value < (m_chance * 0.01f);
        }

        public static bool PickChance(float chance)
        {
            return Random.value < (chance * 0.01f);
        }
    }

    [System.Serializable]
    public class DrawSettings
    {
        [SerializeField] bool m_draw;
        [SerializeField] DrawMethod m_drawMethod;
        [SerializeField] Color m_color;
        public bool CanDraw(DrawMethod drawMethod)
        {
            return m_drawMethod == drawMethod && m_draw;
        }

        public virtual Color GetColor()
        {
            return m_color;
        }
    }

    [System.Serializable]
    public class DamageSettings
    {
        [SerializeField, Min(0)] float m_minDamageValue = 1;
        [SerializeField, Min(0)] float m_maxDamageValue = 1;
        [SerializeField, Min(0)] float m_criticalMultiplier = 1;
        [SerializeField] ChancePicker m_criticalChance;

        public float GetDamage()
        {
            float damage = Random.Range(m_minDamageValue, m_maxDamageValue);
            if (m_criticalChance.PickChance())
            {
                damage *= m_criticalMultiplier;
            }
            return damage;
        }
    }
    public static class MigalhazHelper
    {
        #region Camera
        static Camera m_mainCamera;
        public static Camera m_MainCamera
        {
            get
            {
                if (m_mainCamera is null)
                {
                    m_mainCamera = Camera.main;
                }
                return m_mainCamera;
            }
        }

        static Camera m_currentCamera;
        public static Camera m_CurrentCamera
        {
            get 
            {
                if (m_currentCamera is null)
                {
                    m_currentCamera = Camera.current;
                } 
                return m_currentCamera;
            }
        }
        #endregion

        #region WaitForSeconds
        static Dictionary<float, WaitForSeconds> m_waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (m_waitForSecondsDictionary.TryGetValue(seconds, out WaitForSeconds wait)) return wait;
            m_waitForSecondsDictionary[seconds] = new WaitForSeconds(seconds);
            return m_waitForSecondsDictionary[seconds];
        }
        #endregion

        #region MouseOverUI
        static PointerEventData m_eventDataCurrentPos;
        static List<RaycastResult> m_results;
        static EventSystem m_currentEventSystem;

        public static EventSystem m_CurrentEventSystem
        {
            get
            {
                if (m_currentEventSystem is null) m_currentEventSystem = EventSystem.current;
                return m_currentEventSystem;
            }
        }
        public static bool IsOverUI(Vector3 _position)
        {
            m_eventDataCurrentPos = new PointerEventData(m_CurrentEventSystem)
            {
                position = _position
            };
            m_results = new List<RaycastResult>();
            m_CurrentEventSystem.RaycastAll(m_eventDataCurrentPos, m_results);
            return m_results.Count > 0;
        }
        #endregion

        #region RectTransformPosition
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, m_MainCamera, out Vector3 worldPoint);
            return worldPoint;
        }
        public static Vector2 GetCanvasPositionOfWorldElement(GameObject element)
        {
            return RectTransformUtility.WorldToScreenPoint(m_MainCamera, element.transform.position);
        }

        #endregion

        #region RangeExtend
        public static float RangeBy0(float _maxInclusive)
        {
            return Random.Range(0f, _maxInclusive);
        }
        public static int RangeBy0(int _maxExclusive)
        {
            return Random.Range(0, _maxExclusive);
        }
        #endregion

        #region TransformExtend
        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static Transform GetMainParent(this Transform transform)
        {
            Transform mainParent = transform;
            while (mainParent.parent is not null)
            {
                mainParent = mainParent.parent;
            }
            return mainParent;
        }
        public static void ResetLocalTransformation(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        
        public static void ResetTransformation(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        #endregion

        #region CacheGetComponent
        public static T CacheGetComponent<T>(this GameObject obj, ref T component)
        {
            if (component is not null) return component;
            bool get = obj.TryGetComponent(out T getComponent);
            if (get)
            {
                component = getComponent;
            }
            return component;
        }

        public static T CacheGetComponent<T>(this Component obj, ref T component)
        {
            if (component is not null) return component;
            bool get = obj.TryGetComponent(out T getComponent);
            if (get)
            {
                component = getComponent;
            }
            return component;
        }
        #endregion
    }
}