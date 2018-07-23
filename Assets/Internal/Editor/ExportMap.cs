using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Tub
{

    public class ExportMap : MonoBehaviour
    {
        public static string TubExe
        {
            get { return EditorPrefs.GetString( "tubexe", "" ); }
            set { EditorPrefs.SetString( "tubexe", value ); }
        }

        static void CheckExport( TubLevel level )
        {
            if ( level == null ) throw new System.Exception( "No TubLevel component" );
            if ( string.IsNullOrEmpty( level.UniqueIdentifier ) ) throw new System.Exception( "TubLevel.UniqueIdentifier is blank - it needs to be a unique indientier" );
            if ( string.IsNullOrEmpty( level.Title ) ) throw new System.Exception( "TubLevel.Title is empty" );
            if ( level.Icon != null && (level.Icon.width != 512 || level.Icon.height != 512)  ) throw new System.Exception( "TubLevel.Icon should be 512x512" );
        }

        [MenuItem( "Tub/Export", validate = true )]
        static public bool ExportThisMapValidate()
        {
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            CheckExport( tubLevel );

            var targetName = EditorPrefs.GetString( $"Save{tubLevel.UniqueIdentifier}As" );
            return !string.IsNullOrEmpty( targetName );
        }

        [MenuItem( "Tub/Export" )]

        static public void ExportThisMap()
        {
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            CheckExport( tubLevel );

            var targetName = EditorPrefs.GetString( $"Save{tubLevel.UniqueIdentifier}As" );
            if ( string.IsNullOrEmpty( targetName ) )
            {
                ExportThisMapAs();
                return;
            }

            ExportMapAs( targetName );
        }


        static public void ExportMapAs( string targetName )
        { 
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            CheckExport( tubLevel );

            EditorUtility.DisplayProgressBar( "Exporting Map", "...", 0.0f );


            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            var bundles = new List<AssetBundleBuild>();

            var timer = Stopwatch.StartNew();

            bundles.AddRange( UnityEditor.AssetDatabase.GetAllAssetBundleNames().Select( x => new AssetBundleBuild
            {
                assetBundleName = x,
                assetNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( x )
            } ) );


            var sceneBundle = new AssetBundleBuild
            {
                assetBundleName = $"{tubLevel.UniqueIdentifier}.map",
                assetNames = new[] { scene.path }
            };

            bundles.Add( sceneBundle );

            if ( !System.IO.Directory.Exists( "TempBundleBuild" ) )
                System.IO.Directory.CreateDirectory( "TempBundleBuild" );

            EditorUtility.DisplayProgressBar( "Exporting Map", "Bundling..", 5.0f );

            BuildPipeline.BuildAssetBundles( "TempBundleBuild", bundles.ToArray(), BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows64 );

            UnityEngine.Debug.Log( $"Created bundles in {timer.Elapsed.TotalSeconds:0.00} seconds" );

            byte[] thumb = new byte[0];

            if ( tubLevel.Icon != null )
            {
                thumb = tubLevel.Icon.EncodeToPNG();
            }

            var sceneBundleInfo = new System.IO.FileInfo( $"TempBundleBuild/{sceneBundle.assetBundleName}" );

            var missionHeader = new MissionHeader
            {
                Version = 2,
                Identifier = tubLevel.UniqueIdentifier,
                MapVersion = tubLevel.Version,
                BundleSize = (int) sceneBundleInfo.Length,
                ThumbSize = thumb.Length,
                Title = tubLevel.Title,
                Description = tubLevel.Description
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
                    writer.Write( thumb );
                    writer.Write( System.IO.File.ReadAllBytes( $"TempBundleBuild/{sceneBundle.assetBundleName}" ) );
                }
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem( "Tub/Export As..." )]
        static public void ExportThisMapAs()
        {
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            CheckExport( tubLevel );

            var target = EditorUtility.SaveFilePanel( "Export Map As..", "", "", "tub" );

            if ( !string.IsNullOrEmpty( target ) )
            {
                EditorPrefs.SetString( $"Save{tubLevel.UniqueIdentifier}As", target );
                ExportThisMap();
            }
        }


        [MenuItem( "Tub/Open In Game.." )]
        static public void ExportThisMapAndRun()
        {
            var fileName = System.IO.Path.GetTempFileName();
            ExportMapAs( fileName );

            if ( string.IsNullOrEmpty( TubExe ) || !System.IO.File.Exists( TubExe ) )
            {
                FindTubExe();
            }

            if ( string.IsNullOrEmpty( TubExe ) )
            {
                UnityEngine.Debug.LogError( "Tub.exe not found" );
                return;
            }

            var command = $"run;{fileName}";

            //
            // Is the game running?
            //
            if ( !ClientMessage.Send( $"run;{fileName}" ) )
            {
                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName( TubExe ),
                    Arguments = $"-cmd \"{command}\"",
                    FileName = TubExe
                };

                Process.Start( startInfo );
            }
        }

        [MenuItem( "Tub/Settings/Find Tub.exe" )]
        static public void FindTubExe()
        {
            TubExe = "";

            var searchLocations = new[]
            {
                @"C:\Steam\steamapps\common\Tub\",
                @"C:\Program Files\Steam\SteamApps\Common",
                @"D:\Program Files\Steam\SteamApps\Common",
                @"C:\Program Files\Steam (x86)\SteamApps\Common",
                @"D:\Program Files\Steam (x86)\SteamApps\Common",
            };

            var startDirectory = searchLocations.FirstOrDefault( x => System.IO.Directory.Exists( x ) );

            var exe = EditorUtility.OpenFilePanel( "Find tub.exe", startDirectory, "exe");

            TubExe = exe;
        }
    }

    [System.Serializable]
    public class MissionHeader
    {
        public int Version;

        public string Title;
        public int MapVersion;
        public string Description;
        public string Identifier;

        public int ThumbSize;
        public int BundleSize;
    }

}