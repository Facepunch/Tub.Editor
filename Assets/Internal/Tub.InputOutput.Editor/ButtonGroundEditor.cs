using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( ButtonGround ) ), CanEditMultipleObjects]
    public class GroundButtonEditor : ButtonEditor
    {
        ButtonGround Button { get { return target as ButtonGround; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}
