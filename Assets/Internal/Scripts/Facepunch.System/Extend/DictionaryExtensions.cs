using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Extend
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// If the key doesn't exist it is created and returned
        /// </summary>
        public static TValue GetOrCreate<TKey, TValue>( this Dictionary<TKey, TValue> dict, TKey key ) where TValue : new()
        {
            TValue val;
            if ( dict.TryGetValue( key, out val ) )
                return val;

            val = new TValue();
            dict.Add( key, val );
            return val;
        }

        /// <summary>
        /// If the key doesn't exist it is created and returned
        /// Pooled version
        /// </summary>
        public static TValue GetOrCreatePooled<TKey, TValue>( this Dictionary<TKey, TValue> dict, TKey key ) where TValue : class, new()
        {
            TValue val;
            if ( dict.TryGetValue( key, out val ) )
                return val;

            val = Facepunch.Pool.Get<TValue>();
            dict.Add( key, val );
            return val;
        }

        /// <summary>
        /// Clones the dictionary. Doesn't clone the values.
        /// </summary>
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>( this Dictionary<TKey, TValue> dict )
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>( dict.Count, dict.Comparer );

            foreach ( KeyValuePair<TKey, TValue> entry in dict )
            {
                ret.Add( entry.Key, (TValue)entry.Value );
            }

            return ret;
        }
    }
}
