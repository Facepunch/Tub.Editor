using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExportMap : MonoBehaviour
{

    [MenuItem( "Tub/Export This Map" )]

	static public void ExportThisMap()
    {
        var tubLevel = Object.FindObjectOfType<TubLevel>();
        if ( tubLevel == null )
            throw new System.Exception( "You need a TubLevel component to export a map" );

        if ( tubLevel.Mission == null )
            throw new System.Exception( "Your TubLevel doesn't have a mission set" );

        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        var bundles = new List<AssetBundleBuild>();

        var sceneBundle = new AssetBundleBuild
        {
            assetBundleName = $"{tubLevel.Mission.Identifier}.map",
            assetNames = new[] { scene.path }
        };

        var metaBundle = new AssetBundleBuild
        {
            assetBundleName =  $"{tubLevel.Mission.Identifier}.meta",
            assetNames = new[] { UnityEditor.AssetDatabase.GetAssetPath( tubLevel.Mission ) }
        };

        bundles.Add( metaBundle );
        bundles.Add( sceneBundle );

        if ( !System.IO.Directory.Exists( "TempBundleBuild" ) )
            System.IO.Directory.CreateDirectory( "TempBundleBuild" );

        BuildPipeline.BuildAssetBundles( "TempBundleBuild", bundles.ToArray(), BuildAssetBundleOptions.UncompressedAssetBundle |  BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DisableWriteTypeTree, BuildTarget.StandaloneWindows64 );

        if ( !System.IO.Directory.Exists( "ExportedMaps" ) )
            System.IO.Directory.CreateDirectory( "ExportedMaps" );

        Copy( $"TempBundleBuild/{sceneBundle.assetBundleName}", $"C:\\GitHub\\RoomLevel\\Tub\\Maps\\{sceneBundle.assetBundleName}" );
        Copy( $"TempBundleBuild/{metaBundle.assetBundleName}", $"C:\\GitHub\\RoomLevel\\Tub\\Maps\\{metaBundle.assetBundleName}" );
    }

    private static void Copy( string v1, string v2 )
    {
        if ( System.IO.File.Exists( v2 ) )
            System.IO.File.Delete( v2 );

        System.IO.File.Copy( v1, v2 );
    }
}
