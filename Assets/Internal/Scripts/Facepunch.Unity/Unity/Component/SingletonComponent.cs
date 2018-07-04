using UnityEngine;

namespace Facepunch
{
    public abstract class SingletonComponent<T> : SingletonComponent where T : MonoBehaviour
    {
        public static T Instance
        {
            get { return instance; }
        }

        private static T instance = default( T );

        public override void SingletonSetup()
        {
            if ( instance != this ) instance = this as T;
        }

        public override void SingletonClear()
        {
            if ( instance == this ) instance = null;
        }
    }


    public abstract class SingletonComponent : MonoBehaviour
    {
        public abstract void SingletonSetup();
        public abstract void SingletonClear();

        protected virtual void Awake()
        {
            SingletonSetup();
        }

        protected virtual void OnDestroy()
        {
            SingletonClear();
        }
    }
}