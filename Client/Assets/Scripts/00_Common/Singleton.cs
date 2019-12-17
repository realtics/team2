using UnityEngine;
using System.Collections;

namespace Single
{
    public class Singleton<T> where T : class, new()
    {
        protected static object _instanceLock = new object();
        protected static volatile T _instance;
        public static T instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (null == _instance) _instance = new T();
                }
                return _instance;
            }
        }
    }
}