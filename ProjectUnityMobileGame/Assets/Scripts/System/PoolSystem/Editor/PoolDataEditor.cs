using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MigalhaSystem.Pool
{
    [CustomEditor(typeof(PoolData))]
    public class PoolDataEditor : Editor
    {
        GUILayoutOption[] MyGUILayout;
        PoolData poolData;

        Color enableColor = Color.green;
        Color disableColor = Color.red;
        private void OnEnable()
        {
            MyGUILayout = new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(75),
                GUILayout.MaxWidth(300)
            };
        }

        public override void OnInspectorGUI()
        {
            poolData = (PoolData)target;
            DrawBasicSettings();
            DrawExtraObjects();

        }

        void DrawBasicSettings()
        {
            if (poolData == null) return;


            GUILayout.Label("Pool Tag:");
            poolData.m_PoolTag = GUILayout.TextField(poolData.m_PoolTag, 30, MyGUILayout);
            GUILayout.Space(10);


            GUILayout.Label("Prefab:");
            poolData.m_Prefab = (GameObject)EditorGUILayout.ObjectField("", poolData.m_Prefab, typeof(GameObject), false, MyGUILayout);
            GUILayout.Space(10);


            GUILayout.Label("Deafault Pool Size:");
            poolData.m_PoolSize = EditorGUILayout.IntField("", poolData.m_PoolSize, MyGUILayout);
            if (poolData.m_PoolSize <= 0)
            {
                poolData.m_PoolSize = 1;
            }
            GUILayout.Space(10);

            GUI.backgroundColor = poolData.m_ExpandablePool ? enableColor : disableColor;
            if (GUILayout.Button("Expandable Pool", MyGUILayout))
            {
                poolData.m_ExpandablePool = !poolData.m_ExpandablePool;
            }
            GUI.backgroundColor = Color.white;
        }

        void DrawExtraObjects()
        {
            if (poolData == null) return;
            if (!poolData.m_ExpandablePool) return;
            GUI.backgroundColor = poolData.m_DestroyExtraObjectsAfterUse ? enableColor : disableColor;
            if (GUILayout.Button("Destroy Extra Objects", MyGUILayout))
            {
                poolData.m_DestroyExtraObjectsAfterUse = !poolData.m_DestroyExtraObjectsAfterUse;
            }
            GUI.backgroundColor = Color.white;
            if (!poolData.m_DestroyExtraObjectsAfterUse) return;

            GUILayout.Label("Extra Objects Allowed:");
            poolData.m_ExtraObjectsAllowed = EditorGUILayout.IntField("", poolData.m_ExtraObjectsAllowed, MyGUILayout);
            if (poolData.m_ExtraObjectsAllowed < 0)
            {
                poolData.m_ExtraObjectsAllowed = 0;
            }
        }
    }
}