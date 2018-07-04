using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Duplicate a gameobject in place, returning the component on that object.
        /// New object will have the same parent.
        /// </summary>
        public static T Duplicate<T>( this T obj ) where T : Component
        {
            var go = GameObject.Instantiate<GameObject>( obj.gameObject, obj.transform.parent );
            return go.GetComponent<T>();
        }

        /// <summary>
        /// Duplicate but create multiple
        /// </summary>
        public static T[] Duplicate<T>( this T obj, int amount, bool includeOriginalInArray = false ) where T : Component
        {
            if ( includeOriginalInArray ) amount += 1;

            var ta = new T[amount];

            for( int i =0; i< amount; i++ )
            {
                if ( includeOriginalInArray && i == 0 )
                {
                    ta[i] = obj;
                }
                else
                {
                    ta[i] = obj.Duplicate();
                }
            }

            return ta;
        }

        /// <summary>
        /// Pass through, calls gameObject.SetActive
        /// </summary>
        public static void SetActive<T>( this T obj, bool active ) where T : Component
        {
            obj.gameObject.SetActive( active );
        }

        public static Vector2 WorldToRectTransform<T>( this T obj, Vector2 worldPos ) where T : Component
        {
            Rect newRect = obj.GetWorldRect();

            worldPos.x = (worldPos.x - newRect.xMin) / newRect.width;
            worldPos.y = (worldPos.y - newRect.yMin) / newRect.height;

            return worldPos;
        }

        public static Rect GetWorldRect<T>( this T obj ) where T : Component
        {
            Vector3[] corners = new Vector3[4];
            ((RectTransform)obj.transform).GetWorldCorners( corners );
            return new Rect( corners[0], corners[2] - corners[0] );
        }


        public static bool GetComponent<T, U>( this T obj, out U value ) where T : Component
        {
            value = obj.GetComponent<U>();
            return value != null;
        }

        public static bool GetComponentInParent<T, U>( this T obj, out U value ) where T : Component
        {
            value = obj.GetComponentInParent<U>();
            return value != null;
        }

        public static bool GetComponentInChildren<T, U>( this T obj, out U value ) where T : Component
        {
            value = obj.GetComponentInChildren<U>();
            return value != null;
        }
    }
}