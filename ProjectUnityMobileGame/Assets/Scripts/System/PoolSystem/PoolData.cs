using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.Pool 
{
    [CreateAssetMenu(fileName = "New Pool Data", menuName = "Scriptable Object/Pool/New Pool")]
    public class PoolData : ScriptableObject
    {
        public string m_PoolTag = "New Pool";
        public GameObject m_Prefab;
        [Min(1)] public int m_PoolSize = 1;
        public bool m_ExpandablePool = false;
        public bool m_DestroyExtraObjectsAfterUse;
        [Min(0)] public int m_ExtraObjectsAllowed;
        public bool CompareTag(string _tag)
        {
            return m_PoolTag.Equals(_tag);
        }

        public int MaxPoolSize()
        {
            return m_PoolSize + m_ExtraObjectsAllowed;
        }
    }
}