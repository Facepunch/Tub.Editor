using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch
{
    public static class List
    {
        /// <summary>
        /// Compare two lists and categorize the difference into 3 lists - added, removed and remained
        /// </summary>
        public static void Compare<T>( this List<T> a, List<T> b, List<T> added, List<T> removed, List<T> remained )
        {
            //
            // Technically all thise code does is this
            //
            // added.AddRange( b.Where( x => !a.Contains( x ) ) );
            // removed.AddRange( a.Where( x => !b.Contains( x ) ) );
            // remained.AddRange( a.Where( x => b.Contains( x ) ) );
            //
            // But in a slightly faster way

            // Both null or empty
            if ( a == null && b == null ) return;

            // a is null, then everything is new
            if ( a == null )
            {
                if ( added != null ) added.AddRange( b );
                return;
            }

            // b is null, then everything is removed
            if ( b == null )
            {
                if ( removed != null ) removed.AddRange( a );
                return;
            }

            if ( a.Count == 0 && b.Count == 0 ) return;

            // By default all B were 'added'
            if ( added != null ) added.AddRange( b );

            // By default all A were 'removed'
            if ( removed != null ) removed.AddRange( a );

            //
            // If in B and A then add to remained, and remove from added and removed
            //
            foreach ( T item in b )
            {
                if ( !a.Contains( item ) ) continue;

                if ( remained != null )
                    remained.Add( item );

                // Needs to remove all
                if ( added  != null )
                    while ( added.Remove( item ) ) { }

                if ( removed != null )
                    while ( removed.Remove( item ) ) { }
                
            }
        }

        private static System.Random random = new System.Random();

        public static void Shuffle<T>( this IList<T> list )
        {
            int n = list.Count;
            while ( n > 1 )
            {
                n--;
                int k = random.Next( n + 1 );
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddIfNotExists<T>( this IList<T> list, T add )
        {
            if ( list.Contains( add ) ) return;

            list.Add( add );
        }
    }
}
