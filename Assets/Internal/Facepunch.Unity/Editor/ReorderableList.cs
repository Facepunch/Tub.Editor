using UnityEngine;
using UnityEditor;

namespace Facepunch.Editor
{
    public class ReorderableList : UnityEditorInternal.ReorderableList
    {
        public delegate void DrawElementDelegate( SerializedProperty property, Rect rect, int index, bool isActive, bool isFocused );

        public DrawElementDelegate DrawElementFunction;

        string Title;

        public ReorderableList( string name, SerializedObject serializedObject, SerializedProperty elements, bool draggable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true ) : base( serializedObject, elements, draggable, displayHeader, displayAddButton, displayRemoveButton )
        {
            Title = name;

            drawElementCallback += DrawElement;
            drawHeaderCallback += DrawHeader;
            elementHeight = 22;
        }

        private void DrawHeader( Rect rect )
        {
            EditorGUI.LabelField( rect, Title );
        }

        private void DrawElement( Rect rect, int index, bool isActive, bool isFocused )
        {
            EditorGUI.BeginChangeCheck();

            rect.y += 2;
            rect.height -= 6;

            if ( DrawElementFunction != null )
            {
                DrawElementFunction.Invoke( serializedProperty, rect, index, isActive, isFocused );
            }
            else
            {
                EditorGUI.PropertyField( rect, serializedProperty.GetArrayElementAtIndex( index ), new GUIContent( "" ) );
            }
        }
    }
}
