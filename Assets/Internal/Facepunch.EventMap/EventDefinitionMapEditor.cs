#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Facepunch;


namespace Facepunch
{
    [CustomPropertyDrawer( typeof( EventDefinitionMap ) )]
    public class EventDefinitionMapDrawer : PropertyDrawer
    {
        // This allows us to use GuiLayout
        public override bool CanCacheInspectorGUI( SerializedProperty property ) => false;
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => -2;

        TriggersEventAttribute[] Triggers;
        bool initialized = false;

        void Initialize( SerializedProperty property )
        {
            if ( initialized ) return;
            initialized = true;

            var mb = property.serializedObject.targetObject as Component;
            if ( mb == null ) return;

            var allc = mb.GetComponents<Component>();

            Triggers = allc.Select( x => x.GetType() )
                .SelectMany( x => x.GetCustomAttributes( typeof( TriggersEventAttribute ), true ) )
                .Cast<TriggersEventAttribute>()
                .ToArray();
        }


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            Initialize( property );

            var events = property.FindPropertyRelative( "Events" );
            var tab = property.FindPropertyRelative( "editorselected" ).stringValue;

            var grouped = Enumerable
                .Range( 0, events.arraySize )
                .Select( x => new { Index = x, Element = events.GetArrayElementAtIndex( x ) } )
                .GroupBy( x => x.Element.FindPropertyRelative( "Name" ).stringValue )
                .OrderBy( x => x.Key );

            var keys = grouped.Select( x => x.Key ).ToArray();

            bool AddCategory = false;

            using ( new EditorGUILayout.HorizontalScope() )
            {
                DrawToolbar( property, ref tab, keys, ref AddCategory );

                using ( new EditorGUILayout.VerticalScope( Styles.Content ) )
                {
                    var attr = Triggers.FirstOrDefault( x => x.TriggerName == tab );

                    string renamed = null;
                    using ( new EditorGUILayout.VerticalScope( Styles.ContentHeader ) )
                    {
                        using ( new EditorGUILayout.HorizontalScope() )
                        {
                            renamed = EditorGUILayout.TextField( tab );
                            if ( keys.Contains( renamed ) ) renamed = null;

                            if ( renamed != null )
                                property.FindPropertyRelative( "editorselected" ).stringValue = renamed;

                            if ( GUILayout.Button( "+", GUILayout.Width( 22 ) ) )
                            {
                                events.InsertArrayElementAtIndex( events.arraySize );

                                var created = events.GetArrayElementAtIndex( events.arraySize - 1 );
                                created.FindPropertyRelative( "Name" ).stringValue = tab;
                            }
                        }

                        if ( attr != null )
                        {
                            EditorGUILayout.LabelField( "Info: " + attr.Description, new GUIStyle( "MiniLabel" ) );
                        }
                    }

                    if ( grouped.Any( x => x.Key == tab ) )
                    {
                        foreach ( var prop in grouped.First( x => x.Key == tab ) )
                        {
                            if ( !string.IsNullOrEmpty( renamed ) )
                            {
                                prop.Element.FindPropertyRelative( "Name" ).stringValue = renamed;
                            }

                            EditorGUI.indentLevel = 0;

                            var lw = EditorGUIUtility.labelWidth;
                            EditorGUIUtility.labelWidth = 140;

                            using ( new EditorGUILayout.HorizontalScope( Styles.Event ) )
                            {
                                using ( new EditorGUILayout.VerticalScope( Styles.EventInner ) )
                                {
                                    EditorGUILayout.PropertyField( prop.Element, GUIContent.none, GUILayout.ExpandWidth( true ) );
                                }

                                if ( GUILayout.Button( "", "WinBtnClose", GUILayout.Width( 13 ), GUILayout.Height( 13 ) ) )
                                {
                                    events.DeleteArrayElementAtIndex( prop.Index );
                                }
                            }

                            EditorGUIUtility.labelWidth = lw;
                        }


                    }
                    else if ( grouped.Count() == 0 )
                    {
                        EditorGUILayout.HelpBox( "No events. Press the button on the left to add one.", MessageType.Info, true );
                    }
                }

            }

            EditorGUILayout.Space();

            if ( AddCategory )
            {
                events.InsertArrayElementAtIndex( events.arraySize );

                var created = events.GetArrayElementAtIndex( events.arraySize - 1 );
                created.FindPropertyRelative( "Name" ).stringValue = "untitled";
                property.FindPropertyRelative( "editorselected" ).stringValue = "untitled";
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawToolbar( SerializedProperty property, ref string tab, string[] keys, ref bool AddCategory )
        {
            using ( new EditorGUILayout.VerticalScope( Styles.Menu, GUILayout.Width( 150 ), GUILayout.MinHeight( 160 ) ) )
            {
                var selected = Array.IndexOf( keys, tab );

                selected = GUILayout.SelectionGrid( selected, keys, 1, Styles.MenuButton );

                if ( selected >= 0 )
                {
                    tab = keys.Length > 0 ? keys[selected] : "";
                    property.FindPropertyRelative( "editorselected" ).stringValue = tab;
                }

                GUI.color = Color.gray;
                foreach ( var a in Triggers )
                {
                    if ( keys.Contains( a.TriggerName ) )
                        continue;

                    if ( GUILayout.Button( a.TriggerName, Styles.MenuButton ) )
                    {
                        tab = a.TriggerName;
                        property.FindPropertyRelative( "editorselected" ).stringValue = tab;
                    }
                }
                GUI.color = Color.white;

                GUILayout.Space( 130 );

                if ( GUILayout.Button( "+", Styles.MenuAddButton ) )
                {
                    AddCategory = true;
                }
            }
        }

        class CustomStyles
        {
            public GUIStyle Menu;
            public GUIStyle MenuButton;
            public GUIStyle MenuAddButton;

            public GUIStyle Content;
            public GUIStyle ContentHeader;


            public GUIStyle Event;
            public GUIStyle EventInner;

            public CustomStyles()
            {
                Menu = new GUIStyle( "ObjectPickerBackground" );
                Menu.padding = new RectOffset( 4, 4, 4, 4 );
          //      Menu.stretchHeight = true;
           //     Menu.fixedHeight = 0;

                MenuButton = new GUIStyle( "PR Label" );
                MenuButton.stretchWidth = true;
                MenuButton.fixedWidth = 0;
                MenuButton.alignment = TextAnchor.MiddleLeft;
                MenuButton.onNormal = MenuButton.onHover;

                MenuAddButton = new GUIStyle( "PR Label" );
                MenuAddButton.stretchWidth = true;
                MenuAddButton.fixedWidth = 0;
                MenuAddButton.alignment = TextAnchor.MiddleRight;

                Content = new GUIStyle();
                Content.padding = new RectOffset( 8, 4, 0, 0 );

                ContentHeader = new GUIStyle( GUI.skin.box );
                ContentHeader.padding = new RectOffset( 4, 4, 4, 4 );
                ContentHeader.margin = new RectOffset( 0, 0, 0, 8 );

                Event = new GUIStyle( GUI.skin.box );
                Event.padding = new RectOffset( 4, 4, 4, 4 );
                Event.margin = new RectOffset( 0, 0, 8, 0 );

                EventInner = new GUIStyle();
                EventInner.padding = new RectOffset( 4, 4, 4, 4 );
                EventInner.margin = new RectOffset( 0, 0, 0, 0 );
            }
        }

        static CustomStyles Styles = new CustomStyles();
    }
}

#endif