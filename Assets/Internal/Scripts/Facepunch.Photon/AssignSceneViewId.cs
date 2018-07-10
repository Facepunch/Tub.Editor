#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[InitializeOnLoad]
public class AssignSceneViewId : EditorWindow
{
    static AssignSceneViewId()
    {
		EditorApplication.hierarchyChanged += HierarchyChange;
    }

    internal static void HierarchyChange()
    {
        if ( Application.isPlaying )
            return;

        if ( FixMultipleViews() )
            return;

        Facepunch.Networker[] views = SceneManager.GetActiveScene().GetRootGameObjects()
                                            .SelectMany( x => x.GetComponentsInChildren<Facepunch.Networker>( true ) )
                                            .OrderBy( x => x.gameObject.GetInstanceID() )
                                            .ToArray();

        foreach ( Facepunch.Networker view in views )
        {
            if ( view.viewIdField <= 0 || views.Count( x => x.viewIdField == view.viewIdField ) > 1 )
            {
                view.ownerId = 0;
                view.viewIdField = UnusedViewId( views );
                view.prefixBackup = -1;
                view.instantiationId = view.viewIdField;

                EditorUtility.SetDirty( view );
            }
        }

        //
        // Make sure all the prefabs are still viewid = 0
        //
        foreach ( Facepunch.Networker view in Resources.FindObjectsOfTypeAll( typeof( Facepunch.Networker ) )
                                                        .Cast<Facepunch.Networker>()
                                                        .Where( x => EditorUtility.IsPersistent( x.gameObject ) ) )
        {
            if ( view.viewIdField == 0 ) continue;

            view.viewIdField = 0;
            view.prefixBackup = -1;
            view.instantiationId = -1;
            EditorUtility.SetDirty( view );
        }
    }

    static bool FixMultipleViews()
    {
        foreach ( Facepunch.Networker view in Resources.FindObjectsOfTypeAll( typeof( Facepunch.Networker ) ).Cast<Facepunch.Networker>() )
        {
            var multipleComponentCheck = view.GetComponents<Facepunch.Networker>();
            if ( multipleComponentCheck.Length > 1 )
            {
                Debug.LogWarning( $"Object {view.gameObject.name} has multiple Networker, fixing" );
                GameObject.DestroyImmediate( multipleComponentCheck[1], true );
                EditorUtility.SetDirty( view );
                HierarchyChange();
                return true;
            }
        }

        return false;
    }

    static int UnusedViewId( Facepunch.Networker[] views )
    {
        for( int i=1; i < 5000; i++ )
        {
            if ( views.Any( x => x.viewIdField == i ) )
                continue;

            return i;
        }

        throw new System.Exception( "Error getting Unused View Id" );
    }
}
#endif