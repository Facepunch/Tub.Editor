using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Facepunch
{

    [System.Serializable]
    public class WeightedGameObjectList
    {
        public List<Container> Objects = new List<Container>();

        private float _total = 0;

        private float Total
        {
            get
            {
                if ( _total == 0 )
                    _total = Objects.Sum( x => x.Weight );

                return _total;                
            }
        }

        [System.Serializable]
        public struct Container
        {
            public float Weight;
            public GameObject Object;
        }


        /// <summary>
        /// Get the object at f - where f is a float between 0 - 1
        /// </summary>
        public GameObject Get( float f )
        {
            if ( Objects.Count == 0 )
                return null;

            f *= Total;
            float c = 0;
            for( int i=0; i< Objects.Count; i++ )
            {
                c += Objects[i].Weight;

                if (f <= c )
                    return Objects[i].Object;
            }

            return Objects[Objects.Count-1].Object;
        }

        /// <summary>
        /// Get a random object
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public GameObject GetRandom()
        {
            return Get( Random.Range( 0.0f, 1.0f ) );
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer( typeof( WeightedGameObjectList ) )]
    public class WeightedGameObjectListDrawer : PropertyDrawer
    {
        private UnityEditorInternal.ReorderableList list;
        public string HeaderName = "";

        private UnityEditorInternal.ReorderableList List( SerializedProperty property )
        {
            if ( list != null )
                return list;

            list = new UnityEditorInternal.ReorderableList( property.serializedObject, property, true, true, true, true );
            
            list.elementHeight = EditorGUIUtility.singleLineHeight;
            list.drawElementCallback = ( UnityEngine.Rect rect, int index, bool isActive, bool isFocused ) =>
            {

                var p = property.GetArrayElementAtIndex( index );

                // Object
                {
                    var r = rect;
                    r.width *= 0.3f;
                    rect.x += r.width + 8;
                    rect.width -= r.width + 8;
                    EditorGUI.PropertyField( r, p.FindPropertyRelative( "Object" ), GUIContent.none );
                }


                // Weight
                {
                    var r = rect;
                    //EditorGUI.PropertyField( r, p.FindPropertyRelative( "Weight" ), GUIContent.none );

                    EditorGUI.BeginChangeCheck();

                    EditorGUI.Slider( r, p.FindPropertyRelative( "Weight" ), 0, 100, "" );

                    if ( EditorGUI.EndChangeCheck() )
                    {
  
                    }                    
                }


            };

            list.drawHeaderCallback = ( Rect rect ) =>
            {
                EditorGUI.LabelField( rect, HeaderName );
            };

            return list;
        }

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            HeaderName = label.text;

            var arrayProperty = property.FindPropertyRelative( "Objects" );
            List( arrayProperty ).DoList( position );
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight( SerializedProperty property, UnityEngine.GUIContent label )
        {
            var arrayProperty = property.FindPropertyRelative( "Objects" );
            return List( arrayProperty ).GetHeight();
        }
    }

#endif

}

