using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Facepunch;

namespace Tub
{
    [System.Serializable]
    class MetaFile
    {
        public int AppId;
        public string Title;
        public string Description;
        public string Meta;
        public string[] Tags;
        public Dictionary<string, string[]> KeyValues;
    }

    public class Workshop : MonoBehaviour
    {

        [MenuItem( "Tub/Upload To Workshop" )]
        public static void UploadToWorkshop()
        {
            var tubLevel = Object.FindObjectOfType<TubLevel>();
            if ( tubLevel == null )
                throw new System.Exception( "You need a TubLevel component to export a map" );

            if ( tubLevel.Mission == null )
                throw new System.Exception( "Your TubLevel doesn't have a mission set" );

            if ( System.IO.Directory.Exists( "TempWorkshop" ) )
                System.IO.Directory.Delete( "TempWorkshop", true );

            System.IO.Directory.CreateDirectory( "TempWorkshop" );
            var dirInfo = new System.IO.DirectoryInfo( "TempWorkshop" );

            try
            {
                CreateMetaFile( $"{dirInfo.FullName}/meta.json", tubLevel );
                CopyPreviewImage( $"{dirInfo.FullName}/preview.png", tubLevel );
                ExportMap.ExportMapAs( $"{dirInfo.FullName}/map.tub" );

                RunUploader( dirInfo.FullName );
            }
            finally
            {
             //   System.IO.Directory.Delete( "TempWorkshop", true );
            }
        }

        private static void CopyPreviewImage( string v, TubLevel tubLevel )
        {
            string assetPath = AssetDatabase.GetAssetPath( tubLevel.Mission.TileImage.texture );
            var tImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;
            tImporter.isReadable = true;
            AssetDatabase.ImportAsset( assetPath );
            AssetDatabase.Refresh();
            
            var pngData = tubLevel.Mission.TileImage.texture.EncodeToPNG();
            System.IO.File.WriteAllBytes( v, pngData );
        }

        private static void RunUploader( string fullName )
        {
            Debug.Log( $"Running {Application.dataPath}/../Workshop/WorkshopPublisher.exe" );
            System.Diagnostics.Process.Start( $"{Application.dataPath}/../Workshop/WorkshopPublisher.exe", $"\"{fullName}\"" );
        }

        static void CreateMetaFile( string filename, TubLevel tubLevel )
        {
            var meta = new MetaFile
            {
                AppId = 790910,
                Title = tubLevel.Mission.Name,
                Description = tubLevel.Mission.Description,
                Meta = "{}",
                Tags = new[] { "Mission", "v1" },
                KeyValues = new Dictionary<string, string[]>()
            };

            meta.KeyValues["env"] = new[] { $"unity{Application.unityVersion}" };

            System.IO.File.WriteAllText( filename, JsonUtility.ToJson( meta, true ) );
        }

    }

}