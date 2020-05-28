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


        private Dictionary<EPools, Pool> _pools = new Dictionary<EPools, Pool>();

        public void InitializePool(EPools pool, int overrideAmount = 0, Action<GameObject> OnObjectInstantiated = null)
        {
            PoolScriptableObject.PoolDataStruct data = _poolSettings.Data[pool.ToString()];
            
            GameObject nPool = new GameObject();
            nPool.name = pool.ToString();
            nPool.transform.SetParent(this.transform);
            List<GameObject> objects = new List<GameObject>();
            int amount = overrideAmount > 0 ? overrideAmount : data.StartingAmount;
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(data.Object, nPool.transform);
                obj.SetActive((false));
                objects.Add(obj);
                OnObjectInstantiated?.Invoke(obj);
            }

            Pool cPool = new Pool
            {
                poolType = pool,
                objects = new Stack<GameObject>(objects),
                parent = nPool
            };
            _pools.Add(cPool.poolType,cPool);
        }
        public GameObject Instantiate(EPools pool,Vector3 pos, Transform parent = null)
        {
            GameObject obj;
            if (_pools[pool].objects.Count > 0)
            {
                obj = _pools[pool].objects.Pop();
                obj.transform.SetParent(parent);
                obj.transform.position = pos;
                obj.SetActive((true));
            }
            else
            {
                obj = Instantiate(_poolSettings.Data[pool.ToString()].Object,parent);
                obj.transform.position = pos;
                obj.SetActive((true));
            }
           
            return obj;
        }

        public void Dispose(GameObject obj, EPools pool)
        {
            Pool p = _pools[pool];
            
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
            obj.transform.SetParent(p.parent.transform);
            p.objects.Push(obj);
        }
        public void Clear()
        {
            _pools.Clear();
            foreach (Transform child in transform) 
            {
                Destroy(child.gameObject);
            }
        }
    }

    public class Pool
    {
        public EPools poolType;
        public Stack<GameObject> objects;
        public GameObject parent;
    }
}