using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Facepunch.Extend;

namespace Spatial
{
    /// <summary>
    /// A 2D grid of items.
    /// </summary>
    public class Grid<T>
    {
        /// <summary>
        /// How many cells wide and long
        /// </summary>
        public int CellCount { get; private set; }

        /// <summary>
        /// Each cells world size
        /// </summary>
        public int CellSize { get; private set; }

        private float CenterX;
        private float CenterY;

        private Node<T>[,] Nodes;
        private Dictionary<T, Node<T>> Lookup;

        public Grid( int CellSize, float WorldSize )
        {
            this.CellSize = CellSize;
            this.CellCount = (int)((WorldSize / CellSize) + 0.5f);
            this.CenterX = WorldSize * 0.5f;
            this.CenterY = WorldSize * 0.5f;

            Nodes = new Node<T>[CellCount, CellCount];
            Lookup = new Dictionary<T, Node<T>>( 512 );
        }

        /// <summary>
        /// Please note that not all of these items will be inside this radius.
        /// This is a broadphase, so they will be near (in the same cell as the radius).
        /// If you need accuracy you should really post process these results to work out
        /// whether they actually are within.
        /// </summary>
        public int Query( float x, float y, float radius, T[] result, Func<T, bool> filter = null )
        {
            var minx = Clamp( (x + CenterX - radius) / CellSize );
            var maxx = Clamp( (x + CenterX + radius) / CellSize );
            var miny = Clamp( (y + CenterY - radius) / CellSize );
            var maxy = Clamp( (y + CenterY + radius) / CellSize );

            int found = 0;

            for ( int xx = minx; xx <= maxx; xx++ )
            {
                for ( int yy = miny; yy <= maxy; yy++ )
                {
                    if ( Nodes[xx, yy] == null ) continue;

                    foreach ( var t in Nodes[xx, yy].Contents )
                    {
                        if ( filter != null && filter( t ) == false )
                            continue;

                        result[found] = t;
                        found++;

                        if ( found >= result.Length )
                            return found;
                    }
                }
            }

            return found;
        }

        internal class Node<U>
        {
            public HashSet<U> Contents = new HashSet<U>();

            public void Add( U obj )
            {
                Contents.Add( obj );
            }

            public bool Remove( U obj )
            {
                return Contents.Remove( obj );
            }
        }

        int Clamp( float input )
        {
            var i = (int)input;

            if ( i < 0 ) return 0;
            if ( i > CellCount - 1 ) return CellCount - 1;

            return i;
        }

        Node<T> GetNode( float x, float y, bool create = true )
        {
            x += CenterX;
            y += CenterY;

            int nodex = Clamp( x / CellSize );
            int nodey = Clamp( y / CellSize );

            var n = Nodes[nodex, nodey];
            if ( n == null && create )
            {
                n = new Node<T>();
                Nodes[nodex, nodey] = n;
            }

            return n;
        }

        /// <summary>
        /// Add an object. No duplicate tests are done.
        /// </summary>
        public void Add( T obj, float x, float y )
        {
            var node = GetNode( x, y );
            node.Add( obj );
            Lookup.Add( obj, node );
        }

        public void Move( T obj, float x, float y )
        {
            var newNode = GetNode( x, y );
            Node<T> node = null;
            if ( Lookup.TryGetValue( obj, out node ) )
            {
                if ( newNode == node )
                    return;

                node.Remove( obj );
                newNode.Add( obj );
                Lookup[obj] = newNode;
            }
        }

        public bool Remove( T obj )
        {
            Node<T> node = null;
            if ( Lookup.TryGetValue( obj, out node ) )
            {
                node.Remove( obj );
                Lookup.Remove( obj );
                return true;
            }

            return false;
        }
    }
}