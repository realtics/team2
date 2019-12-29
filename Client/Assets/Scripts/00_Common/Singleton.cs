using UnityEngine;
using System.Collections;

namespace Single
{
    public class Singleton<T> where T : class, new()
    {
        protected static object _instanceLock = new object();
        protected static volatile T _Instance;
        public static T Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (null == _Instance) _Instance = new T();
                }
                return _Instance;
            }
        }
    }
}