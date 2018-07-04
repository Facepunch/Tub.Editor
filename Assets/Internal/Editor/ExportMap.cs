using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExportMap : MonoBehaviour
{

    [MenuItem( "Tub/Export This Map" )]

	static public void ExportThisMap()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        var bundles = new List<AssetBundleBuild>();

        var thisBundle = new AssetBundleBuild
        {
            assetBundleName = $"{scene.name.ToLower()}.map",
            assetNames = new[] { scene.path }
        };

        bundles.Add( thisBundle );

        if ( !System.IO.Directory.Exists( "TempBundleBuild" ) )
            System.IO.Directory.CreateDirectory( "TempBundleBuild" );

        BuildPipeline.BuildAssetBundles( "TempBundleBuild", bundles.ToArray(), BuildAssetBundleOptions.UncompressedAssetBundle |  BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DisableWriteTypeTree, BuildTarget.StandaloneWindows64 );

        if ( !System.IO.Directory.Exists( "ExportedMaps" ) )
            System.IO.Directory.CreateDirectory( "ExportedMaps" );

        if ( System.IO.File.Exists( $"C:\\GitHub\\RoomLevel\\Tub\\Maps\\{thisBundle.assetBundleName}" ) )
            System.IO.File.Delete( $"C:\\GitHub\\RoomLevel\\Tub\\Maps\\{thisBundle.assetBundleName}" );

        System.IO.File.Copy( $"TempBundleBuild/{thisBundle.assetBundleName}", $"C:\\GitHub\\RoomLevel\\Tub\\Maps\\{thisBundle.assetBundleName}" );

      //  System.IO.Directory.Delete( "TempBundleBuild", true );
    }
}
