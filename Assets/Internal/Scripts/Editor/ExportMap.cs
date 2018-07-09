using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class ExportMap : MonoBehaviour
{
    [MenuItem( "Tub/Export", validate = true )]
    static public bool ExportThisMapValidate()
    {
        var tubLevel = Object.FindObjectOfType<TubLevel>();
        if ( tubLevel == null ) return false;
        if ( tubLevel.Mission == null ) return false;

        var targetName = EditorPrefs.GetString( $"Save{tubLevel.Mission.Identifier}As" );
        return !string.IsNullOrEmpty( targetName );
    }

    [MenuItem( "Tub/Export" )]

    static public void ExportThisMap()
    {
        var tubLevel = Object.FindObjectOfType<TubLevel>();
        if ( tubLevel == null )
            throw new System.Exception( "You need a TubLevel component to export a map" );

        if ( tubLevel.Mission == null )
            throw new System.Exception( "Your TubLevel doesn't have a mission set" );

        var targetName = EditorPrefs.GetString( $"Save{tubLevel.Mission.Identifier}As" );
        if ( string.IsNullOrEmpty( targetName  ))
        {
            ExportThisMapAs();
            return;
        }

        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        var bundles = new List<AssetBundleBuild>();

        bundles.AddRange( UnityEditor.AssetDatabase.GetAllAssetBundleNames().Select( x => new AssetBundleBuild
        {
            assetBundleName = x,
            assetNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( x )
        } ));


        var sceneBundle = new AssetBundleBuild
        {
            assetBundleName = $"{tubLevel.Mission.Identifier}.map",
            assetNames = new[] { scene.path }
        };

        var metaBundle = new AssetBundleBuild
        {
            assetBundleName = $"{tubLevel.Mission.Identifier}.meta",
            assetNames = new[] { UnityEditor.AssetDatabase.GetAssetPath( tubLevel.Mission ) }
        };

        bundles.Add( metaBundle );
        bundles.Add( sceneBundle );

        if ( !System.IO.Directory.Exists( "TempBundleBuild" ) )
            System.IO.Directory.CreateDirectory( "TempBundleBuild" );

        BuildPipeline.BuildAssetBundles( "TempBundleBuild", bundles.ToArray(), BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows64 );

        if ( !System.IO.Directory.Exists( "ExportedMaps" ) )
            System.IO.Directory.CreateDirectory( "ExportedMaps" );

        var metaBundleInfo = new System.IO.FileInfo( $"TempBundleBuild/{metaBundle.assetBundleName}" );
        var sceneBundleInfo = new System.IO.FileInfo( $"TempBundleBuild/{sceneBundle.assetBundleName}" );

        var missionHeader = new MissionHeader
        {
            Version = 1,

            Meta = new MissionHeader.Bundle
            {
                Size = metaBundleInfo.Length
            },

            Scenes = new[]
            {
                new MissionHeader.Bundle
                {
                    Size = sceneBundleInfo.Length
                }
            }
        };

        var json = JsonUtility.ToJson( missionHeader );

        if ( System.IO.File.Exists( targetName ) )
            System.IO.File.Delete( targetName );

        using ( var stream = new System.IO.FileStream( targetName, System.IO.FileMode.OpenOrCreate ) )
        {
            using ( var writer = new System.IO.BinaryWriter( stream ) )
            {
                writer.Write( json );
                writer.Write( System.IO.File.ReadAllBytes( $"TempBundleBuild/{metaBundle.assetBundleName}" ) );
                writer.Write( System.IO.File.ReadAllBytes( $"TempBundleBuild/{sceneBundle.assetBundleName}" ) );
            }
        }
    }

    [MenuItem( "Tub/Export As..." )]

    static public void ExportThisMapAs()
    {
        var tubLevel = Object.FindObjectOfType<TubLevel>();
        if ( tubLevel == null )
            throw new System.Exception( "You need a TubLevel component to export a map" );

        if ( tubLevel.Mission == null )
            throw new System.Exception( "Your TubLevel doesn't have a mission set" );

        var target = EditorUtility.SaveFilePanel( "Export Map As..", "", "", "tub" );

        if ( !string.IsNullOrEmpty( target ) )
        {
            EditorPrefs.SetString( $"Save{tubLevel.Mission.Identifier}As", target );
            ExportThisMap();
        }
    }
}

[System.Serializable]
public class MissionHeader
{
    public int Version;

    public Bundle Meta;
    public Bundle[] Scenes;

    [System.Serializable]
    public class Bundle
    {
        public long Size;
    }
}
