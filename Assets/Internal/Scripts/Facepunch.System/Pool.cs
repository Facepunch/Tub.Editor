//#define POOL_DIAGNOSTICS
using System;
using System.Collections.Generic;

//
// This is kept very simple by design.
// This should be kept platform agnostic at all costs
//

namespace Facepunch
{
    public static class Pool
    {
        public interface IPooled
        {
            void EnterPool();
            void LeavePool();
        }

        public interface ICollection
        {
            // Using long instead of ulong to allow comparison operators with integers
            long ItemsInStack { get; }
            long ItemsInUse { get; }
            long ItemsCreated { get; }
            long ItemsTaken { get; }
            long ItemsSpilled { get; }
        }

        public class PoolCollection<T> : ICollection
        {
            public T[] buffer = new T[512];

            public long ItemsInStack { get; set; }
            public long ItemsInUse { get; set; }
            public long ItemsCreated { get; set; }
            public long ItemsTaken { get; set; }
            public long ItemsSpilled { get; set; }
        }

        public static Dictionary< System.Type, object > directory = new Dictionary<System.Type, object>();

        /// <summary>
        /// Clears a list and adds it to the pool
        /// </summary>
        public static void FreeList<T>( ref List<T> obj )
        {
            if ( obj == null )
                throw new System.ArgumentNullException();

            obj.Clear();
            Free( ref obj );

            if ( obj != null )
                throw new System.SystemException( "Pool.Free is not setting object to NULL" );
        }


        public static void FreeMemoryStream( ref System.IO.MemoryStream obj )
        {
            if ( obj == null )
                throw new System.ArgumentNullException();

            obj.Position = 0;
            obj.SetLength( 0 );
            Free( ref obj );

            if ( obj != null )
                throw new System.SystemException( "Pool.Free is not setting object to NULL" );
        }

        /// <summary>
        /// Return the object to the queue.
        /// This object should have been reset to its default state.
        /// </summary>
        public static void Free<T>( ref T obj ) where T : class
        {
            if ( obj == null )
                throw new System.ArgumentNullException();

            var collection = FindCollection<T>();

#if POOL_DIAGNOSTICS
            //
            // Safety Test Mode
            //
            for ( int i = 0; i < collection.ItemsInStack; i++ )
            {
                if ( collection.buffer[i].Equals( obj ) )
                    throw new System.ArgumentException( "Object is already in the collection!" );
            }
#endif

            //
            // Enforce an upper Limit
            //
            if ( collection.ItemsInStack >= collection.buffer.Length )
            {
                collection.ItemsSpilled++;
                collection.ItemsInUse--;
                obj = null;
                return;
            }

            //
            // Add it
            //
            collection.buffer[collection.ItemsInStack] = obj;
            collection.ItemsInStack++;
            collection.ItemsInUse--;

            //
            // Notify the interface, if available
            //
            var poolable = obj as IPooled;
            if ( poolable != null )
            {
                poolable.EnterPool();
            }

            //
            // Reset the pointer so it can't be used anymore
            //
            obj = null;
        }

        /// <summary>
        /// A pooled version of calling "new T()"
        /// </summary>
        public static T Get<T>() where T : class, new()
        {
            var collection = FindCollection<T>();

            if ( collection.ItemsInStack > 0 )
            {
                collection.ItemsInStack--;
                collection.ItemsInUse++;
                var obj = collection.buffer[collection.ItemsInStack];
                collection.buffer[collection.ItemsInStack] = null;

                //
                // Notify the interface, if available
                //
                var poolable = obj as IPooled;
                if ( poolable != null )
                {
                    poolable.LeavePool();
                }

                collection.ItemsTaken++;

                return obj;
            }

            collection.ItemsCreated++;
            collection.ItemsInUse++;

            return new T();
        }

        public static List<T> GetList<T>()
        {
            return Get<List<T>>();
        }

        /// <summary>
        /// Resizes the pool buffer for a type.
        /// </summary>
        public static void ResizeBuffer<T>( int size )
        {
            var col = FindCollection<T>();

            Array.Resize( ref col.buffer, size );
        }

        /// <summary>
        /// Fills the pool buffer for a type.
        /// </summary>
        public static void FillBuffer<T>( int count = int.MaxValue ) where T : class, new()
        {
            var collection = FindCollection<T>();

            for ( int i = 0; i < count; i++ )
            {
                if ( collection.ItemsInStack >= collection.buffer.Length ) break;

                collection.buffer[collection.ItemsInStack] = new T();
                collection.ItemsInStack++;
            }
        }

        /// <summary>
        /// Gets a PoolCollection. Use for diagnostics, debug printing.
        /// </summary>
        public static PoolCollection<T> FindCollection<T>()
        {
            object obj;

            if ( !directory.TryGetValue( typeof( T ), out obj ) )
            {
                obj = new PoolCollection<T>();
                directory.Add( typeof( T ), obj );
            }

            return (PoolCollection<T>)obj;
        }

        public static void Clear()
        {
            directory.Clear();
            directory = new Dictionary<System.Type, object>();
        }
    }
}
