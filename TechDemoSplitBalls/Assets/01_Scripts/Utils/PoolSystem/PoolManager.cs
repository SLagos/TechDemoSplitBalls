using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.PoolSystem
{
    public class PoolManager:Singleton<PoolManager>
    {
        [SerializeField]
        private PoolScriptableObject _poolSettings;


        private Dictionary<EPools, Pool> _pools;

        private bool _initialized = false;

        public void Awake()
        {
            StartCoroutine(Initialize());
        }

        public IEnumerator Initialize()
        {
            _pools = new Dictionary<EPools, Pool>();
            foreach (var pool in _poolSettings.PoolData)
            {
                GameObject nPool = new GameObject();
                nPool.name = pool.PoolName;
                nPool.transform.SetParent(this.transform);
                List<GameObject> objects = new List<GameObject>();
                for (int i = 0; i < pool.StartingAmount; i++)
                {
                    GameObject obj = Instantiate(pool._object, nPool.transform);
                    obj.SetActive((false));
                    objects.Add(obj);
                    yield return new WaitForEndOfFrame(); //To Avoid threadLock in the initialization
                }

                Pool cPool = new Pool
                {
                    poolType = (EPools)Enum.Parse(typeof(EPools),pool.PoolName),
                    objects = new Stack<GameObject>(objects),
                    parent = nPool
                };
                _pools.Add(cPool.poolType,cPool);
            }

            _initialized = true;
        }

        public GameObject Instantiate(EPools pool,Vector3 pos, Transform parent = null)
        {
            GameObject obj = _pools[pool].objects.Pop();
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            obj.SetActive((true));
            return _pools[pool].objects.Pop();
        }

        public void Dispose(GameObject obj, EPools pool)
        {
            Pool p = _pools[pool];
            
            obj.SetActive(false);
            obj.transform.SetParent(p.parent.transform);
            p.objects.Push(obj);
        }
    }

    public class Pool
    {
        public EPools poolType;
        public Stack<GameObject> objects;
        public GameObject parent;
    }
}