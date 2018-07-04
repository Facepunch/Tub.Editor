using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Facepunch
{
    public abstract class EventBase
    {
        public abstract string Identifier { get; }
        public abstract string DisplayName { get; }

        public abstract void Run( EventDefinition e, GameObject go );

#if UNITY_EDITOR

        public virtual void RunEditor( EventDefinition e, GameObject go )
        {

        }

        public UnityEditor.SerializedProperty Property;

        public abstract void Inspector();

        protected void RandomRangeVector3( string property, string label, float min, float max )
        {
            var volume = Property.FindPropertyRelative( property ).vector3Value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.MinMaxSlider( label, ref volume.x, ref volume.y, min, max );
            volume.x = Mathf.Round( volume.x * 100 ) / 100.0f;
            volume.y = Mathf.Round( volume.y * 100 ) / 100.0f;
            volume.x = EditorGUILayout.FloatField( volume.x, GUILayout.Width( 50 ) );
            volume.y = EditorGUILayout.FloatField( volume.y, GUILayout.Width( 50 ) );
            EditorGUILayout.EndHorizontal();

            Property.FindPropertyRelative( property ).vector3Value = volume;
        }

        protected void ObjectField<T>( string name, string label ) => EditorGUILayout.ObjectField( Property.FindPropertyRelative( name ), typeof( T ), new GUIContent( label ) );
        protected void PropertyField( string name, string label ) => EditorGUILayout.PropertyField( Property.FindPropertyRelative( name ), new GUIContent( label ), true );
        protected void PropertyField( string name, int arrayIndex, string label ) => EditorGUILayout.PropertyField( Property.FindPropertyRelative( name ).GetArrayElementAtIndex( arrayIndex ), new GUIContent( label ), true );

#endif
    }
}