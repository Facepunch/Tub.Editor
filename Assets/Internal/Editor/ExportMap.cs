using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Tub
{

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

            var targetName = EditorPrefs.GetString( $"Save{tubLevel.Mission.Identifier}As" );
            if ( string.IsNullOrEmpty( targetName ) )
            {
                ExportThisMapAs();
                return;
            }

            ExportMapAs( targetName );
        }


        static public void ExportMapAs( string targetName, bool randomiseNames = false )
        { 
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            if ( tubLevel == null )
                throw new System.Exception( "You need a TubLevel component to export a map" );

            if ( tubLevel.Mission == null )
                throw new System.Exception( "Your TubLevel doesn't have a mission set" );

            EditorUtility.DisplayProgressBar( "Exporting Map", "...", 0.0f );

            var nameExtra = "";

            if ( randomiseNames )
                nameExtra = Random.Range( 1, 99999 ).ToString();


            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            var bundles = new List<AssetBundleBuild>();

            bundles.AddRange( UnityEditor.AssetDatabase.GetAllAssetBundleNames().Select( x => new AssetBundleBuild
            {
                assetBundleName = x,
                assetNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( x )
            } ) );


            var sceneBundle = new AssetBundleBuild
            {
                assetBundleName = $"{tubLevel.Mission.Identifier}{nameExtra}.map",
                assetNames = new[] { scene.path }
            };

            var metaBundle = new AssetBundleBuild
            {
                assetBundleName = $"{tubLevel.Mission.Identifier}{nameExtra}.meta",
                assetNames = new[] { UnityEditor.AssetDatabase.GetAssetPath( tubLevel.Mission ) }
            };

            bundles.Add( metaBundle );
            bundles.Add( sceneBundle );

            if ( !System.IO.Directory.Exists( "TempBundleBuild" ) )
                System.IO.Directory.CreateDirectory( "TempBundleBuild" );

            EditorUtility.DisplayProgressBar( "Exporting Map", "Bundling..", 5.0f );

            BuildPipeline.BuildAssetBundles( "TempBundleBuild", bundles.ToArray(), BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows64 );


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

            EditorUtility.DisplayProgressBar( "Exporting Map", "Deleting Old File..", 9.5f );

            if ( System.IO.File.Exists( targetName ) )
                System.IO.File.Delete( targetName );

            EditorUtility.DisplayProgressBar( "Exporting Map", "Writing..", 9.9f );

            using ( var stream = new System.IO.FileStream( targetName, System.IO.FileMode.OpenOrCreate ) )
            {
                using ( var writer = new System.IO.BinaryWriter( stream ) )
                {
                    writer.Write( json );
                    writer.Write( System.IO.File.ReadAllBytes( $"TempBundleBuild/{metaBundle.assetBundleName}" ) );
                    writer.Write( System.IO.File.ReadAllBytes( $"TempBundleBuild/{sceneBundle.assetBundleName}" ) );
                }
            }


            EditorUtility.ClearProgressBar();
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


        [MenuItem( "Tub/Open In Game.." )]
        static public void ExportThisMapAndRun()
        {
            // The game must be running already !

            var fileName = System.IO.Path.GetTempFileName();
            ExportMapAs( fileName, true );

            EditorUtility.DisplayProgressBar( "Communicate", "Opening Game..", 0.5f );
            ClientMessage.Send( $"run;{fileName}" );
            EditorUtility.ClearProgressBar();
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

}