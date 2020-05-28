using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;

        [SerializeField]
        private bool _destroyOnLoad = false;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get { return m_Instance; }
        }

        public virtual void Awake()
        {
            m_Instance = this.GetComponent<T>();
            if(!_destroyOnLoad)
                DontDestroyOnLoad(this);
        }
    }
}

