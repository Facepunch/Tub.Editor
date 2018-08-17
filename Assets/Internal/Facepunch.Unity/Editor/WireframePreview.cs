using UnityEngine;
using UnityEditor;

namespace Facepunch.Editor
{
    public static class WireframePreview
    {
        private static Material lineMat;

        private static Color StartColor { get { return Color.yellow; } }
        private static Color EndColor { get { return Color.green; } }

        public static void Draw( Transform root, Vector3 position, Quaternion rotation, Color color )
        {
            var globalMat = root.localToWorldMatrix;
            var newGlobalMat = Matrix4x4.TRS( position, rotation, globalMat.lossyScale );

            if ( lineMat  == null )
            {
                lineMat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>( "Assets/Editor/Wireframe.mat" );
                lineMat = GameObject.Instantiate<Material>( lineMat );
            }

            GL.wireframe = true;
            lineMat.SetColor( "_Color", color );

            lineMat.SetPass( 0 );

            foreach ( var r in root.gameObject.GetComponentsInChildren<MeshRenderer>( true ) )
            {
                var c = r.GetComponent<Collider>();
                if ( c != null && c.isTrigger ) continue;

                var mat = r.transform.localToWorldMatrix;

                mat = globalMat.inverse * mat;
                mat = newGlobalMat * mat;

                var mf = r.GetComponent<MeshFilter>();
                if ( mf.sharedMesh == null ) continue;

                Graphics.DrawMeshNow( mf.sharedMesh, mat );
            }
            GL.wireframe = false;
        }
    }
}