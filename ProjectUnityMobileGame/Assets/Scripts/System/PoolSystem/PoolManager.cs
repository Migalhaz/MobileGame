using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Pool
{
    [Serializable]
    public class Pool 
    {
        #region Variables
        [SerializeField] PoolData m_poolData;

        Transform m_poolParent;
        List<GameObject> m_freeObjects;
        List<GameObject> m_inUseObjects;
        #region Getters
        public PoolData m_PoolData => m_poolData;
        public List<GameObject> m_FreeObjects => m_freeObjects;
        public List<GameObject> m_InUseObjects => m_inUseObjects;
        #endregion

        #endregion

        #region Methods
        public bool CompareTag(string _tag)
        {
            return m_poolData.CompareTag(_tag);
        }

        public bool ComparePoolData(PoolData _poolData)
        {
            return m_poolData == _poolData;
        }

        public void SetupPool(Transform _parent)
        {
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = _parent;
        }

        public void CreateObject()
        {
            GameObject newGameObject = UnityEngine.Object.Instantiate(m_poolData.m_Prefab, m_poolParent);
            newGameObject.SetActive(false);
            m_freeObjects.Add(newGameObject);
        }

        public GameObject PullObject()
        {
            if (AllowCreateNewObject())
            {
                CreateObject();
            }
            if (!FreeGameObjects())
            {
                return null;
            }

            GameObject poolGameObject = m_FreeObjects[m_FreeObjects.Count - 1];
            m_freeObjects.Remove(poolGameObject);
            m_inUseObjects.Add(poolGameObject);
            poolGameObject.SetActive(true);

            IPullable[] pullableArray = poolGameObject.GetComponentsInChildren<IPullable>();
            if (PullableAvailable())
            {
                foreach (IPullable pullableItem in pullableArray)
                {
                    pullableItem.OnPull();
                }
            }

            bool FreeGameObjects()
            {
                if (m_freeObjects == null) return false;
                if (m_freeObjects.Count <= 0) return false;
                return true;
            }
            bool AllowCreateNewObject()
        {
            if (m_poolParent.childCount <= 0) return true;
            if (m_freeObjects.Count > 0) return false;
            if (m_poolParent.childCount >= m_poolData.m_PoolSize)
            {
                if (!m_poolData.m_ExpandablePool)
                {
                    return false;
                }
            }
            return true;
        }
            bool PullableAvailable()
            {
                if (pullableArray == null) return false;
                if (pullableArray.Length <= 0) return false;
                return true;
            }
            return poolGameObject;
        }
        public T PullObject<T>() where T : Component
        {
            if (PullObject().TryGetComponent(out T component))
            {
                return component;
            }
            Debug.LogError($"{typeof(T)} component not found!");
            return null;
        }

        public List<GameObject> PullAllObjects()
        {
            List<GameObject> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject());
            }

            return gameObjects;
        }
        public List<T> PullAllObjects<T>() where T : Component
        {
            List<T> gameObjects = new();
            while (m_freeObjects.Count >= 1)
            {
                gameObjects.Add(PullObject<T>());
            }

            return gameObjects;
        }

        public void PushObject(GameObject _activeGameObject)
        {
            if (!m_inUseObjects.Contains(_activeGameObject)) return;
            IPushable[] pushableArray = _activeGameObject.GetComponentsInChildren<IPushable>(true);
            if (IPushableAvailable())
            {
                foreach (IPushable pushableItem in pushableArray)
                {
                    pushableItem.OnPush();
                }
            }
            m_freeObjects.Add(_activeGameObject);
            m_inUseObjects.Remove(_activeGameObject);
            _activeGameObject.SetActive(false);

            if (!m_poolData.m_DestroyExtraObjectsAfterUse) return;
            
            if (m_poolParent.childCount <= m_poolData.MaxPoolSize()) return;
            UnityEngine.Object.Destroy(_activeGameObject);
            m_freeObjects.Remove(_activeGameObject);

            bool IPushableAvailable()
            {
                if (pushableArray == null) return false;
                if (pushableArray.Length <= 0) return false;
                return true;
            }
        }

        public void PushAllObjects()
        {
            while (m_inUseObjects.Count >= 1)
            {
                PushObject(m_inUseObjects[0]);
            }
        }
        #endregion
    }
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] List<Pool> m_pools = new List<Pool>() { new Pool()};

        protected override void Awake()
        {
            base.Awake();

            foreach (Pool pool in m_pools)
            {
                pool.SetupPool(new GameObject($"{pool.m_PoolData.m_PoolTag} Pool").transform);
                for (int i = 0; i < pool.m_PoolData.m_PoolSize; i++)
                {
                    pool.CreateObject();
                }
            }
        }
        bool PoolCheck(List<Pool> availablePools)
        {
            if (availablePools == null)
            {
                Debug.LogError("No pool found!");
                return false;
            }
            if (availablePools.Count <= 0)
            {
                Debug.LogError("No pool found!");
                return false;
            }
            if (availablePools.Count > 1)
            {
                Debug.LogError("More than 1 found!");
                return false;
            }
            return true;       
        }
        public Pool GetPool(string _poolTag)
        {
            //return m_pools.Find(x => x.CompareTag(_poolTag));
            List<Pool> pools = m_pools.FindAll(x => x.CompareTag(_poolTag));
            if (!PoolCheck(pools)) return null;
            return pools[0];
        }

        public Pool GetPool(PoolData _poolData)
        {
            //return m_pools.Find(x => x.ComparePoolData(_poolTag));
            List<Pool> pools = m_pools.FindAll(x => x.ComparePoolData(_poolData));
            if (!PoolCheck(pools)) return null;
            return pools[0];
        }

        public GameObject PullObject(string _poolTag)
        {
            return GetPool(_poolTag).PullObject();
        }

        public GameObject PullObject(PoolData _poolData)
        {
            return GetPool(_poolData).PullObject();
        }

        public T PullObject<T>(string _poolTag) where T : Component
        {
            return GetPool(_poolTag).PullObject<T>();
        }

        public bool PullObject<T>(string _poolTag, out T component) where T : Component
        {
            component = GetPool(_poolTag).PullObject<T>();
            return component != null;
        }

        public T PullObject<T>(PoolData _poolData) where T : Component
        {
            return GetPool(_poolData).PullObject<T>();
        }

        public bool PullObject<T>(PoolData _poolData, out T component) where T : Component
        {
            component = GetPool(_poolData).PullObject<T>();
            return component != null;
        }

        public List<GameObject> PullAllObjects(string _poolTag)
        {
            return GetPool(_poolTag).PullAllObjects();
        }
        
        public List<GameObject> PullAllObjects(PoolData _poolData)
        {
            return GetPool(_poolData).PullAllObjects();
        }

        public List<T> PullAllObjects<T>(string _poolTag) where T : Component
        {
            return GetPool(_poolTag).PullAllObjects<T>();
        }

        public List<T> PullAllObjects<T>(PoolData _poolData) where T : Component
        {
            return GetPool(_poolData).PullAllObjects<T>();
        }



        public void PushObject(string _poolTag, GameObject _gameObject)
        {
            GetPool(_poolTag).PushObject(_gameObject);
        }

        public void PushObject(PoolData _poolData, GameObject _gameObject)
        {
            GetPool(_poolData).PushObject(_gameObject);
        }

        public void PushAllObjects(string _poolTag)
        {
            GetPool(_poolTag).PushAllObjects();
        }

        public void PushAllObjects(PoolData _poolData)
        {
            GetPool(_poolData).PushAllObjects();
        }
    }

    public interface IPoolable : IPullable, IPushable
    {
    }

    public interface IPullable
    {
        void OnPull();
    }

    public interface IPushable
    {
        void OnPush();
    }
}
