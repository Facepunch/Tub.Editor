using UnityEngine;
using UnityEditor;

public class CreateNewMission : EditorWindow
{
    /*
    string MissionName = "Mission Name";
    string FolderName = "MissionName";

    void OnGUI()
    {
        MissionName = EditorGUILayout.TextField( "Mission Name", MissionName );

        EditorGUILayout.HelpBox( "Folder Name is usually Mission Name without any spaces or punctuation", MessageType.Info );
        FolderName = EditorGUILayout.TextField( "Folder Name", FolderName );

        if ( GUILayout.Button( "Create New Mission" ) )
        {
            CreateMission();
            Close();
        }
    }

    void CreateMission()
    {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        var niceName = char.ToUpperInvariant( FolderName[0] ) + FolderName.Substring( 1 );

        AssetDatabase.CreateFolder( "Assets/Levels", niceName );

        //
        // Copy Shit over
        //
        AssetDatabase.CopyAsset( "Assets/Levels/Template/Template01.unity", $"Assets/Levels/{niceName}/{niceName}01.unity" );
        AssetDatabase.CopyAsset( "Assets/Levels/Template/Template.Background.png", $"Assets/Levels/{niceName}/{niceName}.Background.png" );
        AssetDatabase.CopyAsset( "Assets/Levels/Template/Template.Tile.png", $"Assets/Levels/{niceName}/{niceName}.Tile.png" );
        AssetDatabase.CopyAsset( "Assets/Levels/Template/Template.Mission.asset", $"Assets/Levels/{niceName}/{niceName}.Mission.asset" );

        //
        // Fix references in mission shit
        //
        var mission = AssetDatabase.LoadAssetAtPath<MissionDef>( $"Assets/Levels/{niceName}/{niceName}.Mission.asset" );
        mission.Name = MissionName;
        mission.Identifier = FolderName.ToLower();
        mission.FirstScene.SetFromPath( $"Assets/Levels/{niceName}/{niceName}01.unity" );
        mission.TileImage = AssetDatabase.LoadAssetAtPath<Sprite>( $"Assets/Levels/{niceName}/{niceName}.Tile.png" );
        mission.HeadlineImage = AssetDatabase.LoadAssetAtPath<Sprite>( $"Assets/Levels/{niceName}/{niceName}.Background.png" );

        //
        // Save mission shit
        //
        EditorUtility.SetDirty( mission );
        AssetDatabase.SaveAssets();

        //
        // Load template scene
        //
        var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene( $"Assets/Levels/{niceName}/{niceName}01.unity" );

        //
        // Fix reference to mission in TubLevel
        //
        var levelInfo = FindObjectOfType<Tub.TubLevel>();
        levelInfo.Mission = mission;

        //
        // Save scene shit
        // 
        EditorUtility.SetDirty( levelInfo );
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene( scene );
    }

    [MenuItem( "Tub/Create Mission" )]
    static void CreateProjectCreationWindow()
    {
        CreateNewMission window = new CreateNewMission();
        window.ShowUtility();
    }
    */
}