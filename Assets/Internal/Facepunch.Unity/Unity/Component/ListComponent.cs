using UnityEngine;

namespace Facepunch
{
    public abstract class ListComponent<T> : ListComponent where T : MonoBehaviour
    {
        public static ListHashSet<T> InstanceList
        {
            get { return instanceList; }
        }

        private static ListHashSet<T> instanceList = new ListHashSet<T>();

        public override void Setup()
        {
            if ( !instanceList.Contains( this as T ) )
            {
                instanceList.Add( this as T );
            }
        }

        public override void Clear()
        {
            instanceList.Remove( this as T );
        }
    }

    public abstract class ListComponent : MonoBehaviour
    {
        public abstract void Setup();
        public abstract void Clear();

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void OnDisable()
        {
            Clear();
        }
    }
}
